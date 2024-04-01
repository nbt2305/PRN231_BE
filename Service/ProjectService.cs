using Entities;
using Microsoft.EntityFrameworkCore;
using Repository;
using Shared.Contants;
using Shared.DTO;
using Shared.DTO.Request;
using Shared.DTO.Response;
using Shared.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProjectService : IProjectService
    {
        private readonly RepositoryContext _context;
        public ProjectService(RepositoryContext context)
        {
            _context = context;
        }
        public ResponseData<ProjectDTO> CreateNew(string token, ProjectDTO request)
        {
            string errors = request.ValidateInput(false);

            if (errors != null)
            {
                return new ResponseData<ProjectDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = errors
                };
            }
            try
            {
                var foundProject = _context.Projects.FirstOrDefault(x => x.ProjectCode == request.ProjectCode);
                if (foundProject != null)
                {
                    return new ResponseData<ProjectDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Project already exited!"
                    };
                }

                var duplicateUserIds = request.ProjectUsers.GroupBy(u => u.UserId)
                                       .Where(g => g.Count() > 1)
                                       .Select(g => g.Key);
                if (duplicateUserIds.Any())
                {
                    return new ResponseData<ProjectDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Duplicate user in Project!"
                    };
                }

                var project = ProjectMapper.mapToProject(request);
                _context.Projects.Add(project);
                _context.SaveChanges();

                var projectId = project.Id;
                List<ProjectUserDTO> projectUserRequest = request.ProjectUsers;
                List<ProjectUser> projectUsers = CreateOrUpdateProjectUser(Constants.TYPE_CREATE, projectUserRequest, null, projectId);

                _context.ProjectUsers.AddRange(projectUsers);
                _context.SaveChanges();

                List<int> phases = request.Phases;
                foreach(int phaseId in phases)
                {
                    ProjectPhase projectPhase = new ProjectPhase();
                    projectPhase.PhaseId = phaseId;
                    projectPhase.ProjectId = projectId;
                    _context.ProjectPhases.Add(projectPhase);
                    _context.SaveChanges();
                }

                return new ResponseData<ProjectDTO>
                {
                    Data = ProjectMapper.mapToProjectDTO(project),
                    StatusCode = HttpStatusCode.OK,
                };

            }
            catch (Exception ex)
            {
                return new ResponseData<ProjectDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Created Error"
                };
            }
        }

        public List<ProjectUser> CreateOrUpdateProjectUser(string type, List<ProjectUserDTO> requestCreate, List<ProjectUserDTO> requestUpdate, int projectId)
        {
            if (type.Equals(Constants.TYPE_CREATE))
            {
                List<ProjectUser> response = new List<ProjectUser>();
                foreach (ProjectUserDTO projectUserDTO in requestCreate)
                {
                    ProjectUser projectUser = ProjectMapper.mapToProjectUser(projectUserDTO, projectId);
                    response.Add(projectUser);
                }
                return response;
            }
            else
            {
                List<ProjectUser> response = new List<ProjectUser>();
                var list = _context.ProjectUsers.Where(x => x.ProjectId == projectId).ToList();
                _context.RemoveRange(list);
                _context.SaveChanges();

                var projectPhases = _context.ProjectPhases.Where(x => x.ProjectId == projectId).ToList();
                _context.RemoveRange(projectPhases);
                _context.SaveChanges();

                foreach (ProjectUserDTO projectUserDTO in requestUpdate)
                {
                    ProjectUser projectUser = ProjectMapper.mapToProjectUser(projectUserDTO, projectId);
                    response.Add(projectUser);
                }
                return response;
            }
        }

        public ResponseData<ProjectDTO> Delete(string token, int Id)
        {
            var foundProject = _context.Projects.Find(Id);
            if (foundProject == null)
            {
                return new ResponseData<ProjectDTO>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ErrMsg = "Project Not Found!"
                };
            }

            var projectPhases = _context.ProjectPhases.Where(x => x.ProjectId == Id).ToList();
            _context.RemoveRange(projectPhases);
            _context.SaveChanges();

            var projectUsers = _context.ProjectUsers.Where(x => x.ProjectId == Id).ToList();
            _context.RemoveRange(projectUsers);
            _context.SaveChanges();

            var projectLogWorks = _context.LogWorks.Where(x => x.ProjectId == Id).ToList();
            _context.RemoveRange(projectLogWorks);
            _context.SaveChanges();

            _context.Remove(foundProject);
            _context.SaveChanges();

            return new ResponseData<ProjectDTO>
            {
                StatusCode = HttpStatusCode.OK,
                ErrMsg = "Delete successfully!"
            };
        }

        public ResponseData<ProjectDTO> GetById(string token, int Id)
        {
            var foundProject = _context.Projects.Find(Id);
            if (foundProject == null)
            {
                return new ResponseData<ProjectDTO>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ErrMsg = "Project not found!"
                };
            }

            List<ProjectUserDTO> projectUsers = new List<ProjectUserDTO>();
            var usersOfProject = _context.ProjectUsers.Where(x => x.ProjectId == Id).ToList();
            foreach (ProjectUser user in usersOfProject)
            {
                ProjectUserDTO projectUserDTO = new ProjectUserDTO()
                {
                    UserId = user.UserId,
                    JobPositionId = user.JobPositionId,
                    LevelId=user.LevelId,
                };
                projectUsers.Add(projectUserDTO);
            }

            var phases = _context.ProjectPhases.Where(x => x.ProjectId == Id).Select(x => x.PhaseId).ToList();

            var response = ProjectMapper.mapToProjectDTO(foundProject);
            response.ProjectUsers = projectUsers;
            response.Phases = phases;

            return new ResponseData<ProjectDTO>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK,
            };

        }

        public ResponseData<ProjectResponse> GetDetail(string token, int Id)
        {
            var foundProject = _context.Projects.Find(Id);
            if (foundProject == null)
            {
                return new ResponseData<ProjectResponse>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ErrMsg = "Project not found!"
                };
            }

            var projectUsers = _context.ProjectUsers.Where(x => x.ProjectId == Id).ToList();

            var phases = _context.ProjectPhases.Where(x => x.ProjectId == Id).Select(x => x.PhaseId).ToList();

            List<UserInProjectResponse> userInProjectResponses = new List<UserInProjectResponse>();

            foreach (ProjectUser projectUser in projectUsers)
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == projectUser.UserId);
                var jobPosition = _context.JobPositions.FirstOrDefault(x => x.Id == projectUser.JobPositionId);
                var level = _context.CommonCodes.FirstOrDefault(x => x.Id == projectUser.LevelId);

                UserInProjectResponse userInProjectResponse = new UserInProjectResponse();
                userInProjectResponse.Id = user.Id;
                userInProjectResponse.FullName = user.FullName;
                userInProjectResponse.jobPosition = JobPositionMapper.mapToJobPositionDTO(jobPosition);
                userInProjectResponse.level = CommonCodeMapper.mapToLevelDTO(level);

                userInProjectResponses.Add(userInProjectResponse);
            }

            ProjectResponse response = new ProjectResponse();
            response.ProjectCode = foundProject.ProjectCode;
            response.ProjectName = foundProject.ProjectName;
            response.Description = foundProject.Description;
            response.Users = userInProjectResponses;
            response.Phases = phases;

            return new ResponseData<ProjectResponse>
            {
                Data = response,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<List<ProjectDTO>> GetList(string token)
        {
            var projects = _context.Projects.ToList();
            return new ResponseData<List<ProjectDTO>>
            {
                Data = projects.Select(x => ProjectMapper.mapToProjectDTO(x)).ToList(),
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<PhasesInProject> GetPhasesInProject(string token, int projectId)
        {
            var foundProject = _context.Phases.FirstOrDefault(x => x.Id == projectId);
            if (foundProject == null)
            {
                return new ResponseData<PhasesInProject>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ErrMsg = "Project not found!"
                };
            }

            var phaseIds = _context.ProjectPhases.Where(x => x.ProjectId == projectId).Select(x => x.PhaseId).ToList();
            List<PhaseDTO> phases = new List<PhaseDTO>();
            foreach (var phaseId in phaseIds)
            {
                var phase = _context.Phases.FirstOrDefault(x => x.Id == phaseId);
                var phaseDto = PhaseMapper.mapToPhaseDTO(phase);
                phases.Add(phaseDto);
            }

            var result = new PhasesInProject();
            result.Id = projectId;
            result.Name = foundProject.Name;
            result.Phases = phases;

            return new ResponseData<PhasesInProject>
            {
                Data = result,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<List<ReportResponse>> GetReport(string token)
        {
            var projects = _context.ProjectPhases.Select(x => x.ProjectId).ToList();
            List<ReportResponse> report = new List<ReportResponse>();
            foreach (var item in projects)
            {
                var foundProject = _context.Projects.FirstOrDefault(x => x.Id == item);
                var count = _context.ProjectUsers.Where(x => x.ProjectId == item).Count();
                ReportResponse reportResponse = new ReportResponse()
                {
                    Id = foundProject.Id,
                    ProjectName = foundProject.ProjectName,
                    NumberOfUsers = count,
                };
                report.Add(reportResponse);
            }
            return new ResponseData<List<ReportResponse>>()
            {
                Data = report,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<List<ProjectDTO>> Search(string token, ProjectSearch request)
        {
            var list = _context.Projects.Where(x =>
            (x.ProjectCode.Contains(request.ProjectCode) || request.ProjectCode == null) &&
            (x.ProjectName.ToLower().Contains(request.Name.ToLower()) || request.Name == null) &&
            (x.Description.ToLower().Contains(request.Description.ToLower()) || request.Description == null))
                .ToList();
            var data = list.Select(u => ProjectMapper.mapToProjectDTO(u)).ToList();

            return new ResponseData<List<ProjectDTO>>()
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<ProjectDTO> Update(string token, int id, ProjectDTO request)
        {
            string errors = request.ValidateInput(false);

            if (errors != null)
            {
                return new ResponseData<ProjectDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = errors
                };
            }
            try
            {
                var foundProject = _context.Projects.FirstOrDefault(x => x.Id == id);
                if (foundProject == null)
                {
                    return new ResponseData<ProjectDTO>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        ErrMsg = "Project not found!"
                    };
                }
                if (foundProject != null)
                {
                    if ((foundProject.ProjectCode != request.ProjectCode) && (_context.Projects.FirstOrDefault(x => x.ProjectCode == request.ProjectCode) != null))
                    {
                        return new ResponseData<ProjectDTO>
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrMsg = "Project code already exited!"
                        };
                    }
                }

                foundProject = ProjectMapper.mapToProjectForUpdate(request, foundProject);
                _context.Projects.Update(foundProject);
                _context.SaveChanges();

                List<ProjectUserDTO> projectUserRequest = request.ProjectUsers;
                List<ProjectUser> updateProjectUsers = CreateOrUpdateProjectUser(Constants.TYPE_UPDATE, null, projectUserRequest, id);

                _context.ProjectUsers.AddRange(updateProjectUsers);
                _context.SaveChanges();

                List<int> phases = request.Phases;
                foreach (int phaseId in phases)
                {
                    ProjectPhase projectPhase = new ProjectPhase();
                    projectPhase.PhaseId = phaseId;
                    projectPhase.ProjectId = id;
                    _context.ProjectPhases.Add(projectPhase);
                    _context.SaveChanges();
                }

                return new ResponseData<ProjectDTO>
                {
                    Data = ProjectMapper.mapToProjectDTO(foundProject),
                    StatusCode = HttpStatusCode.OK,
                };

            }
            catch (Exception ex)
            {
                return new ResponseData<ProjectDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Updated Error"
                };
            }
        }
    }
}
