using Entities;
using Shared.DTO;
using Shared.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Mapper
{
    public class ProjectMapper
    {
        public static Project mapToProject(ProjectDTO request)
        {
            return new Project
            {
                ProjectCode = request.ProjectCode,
                ProjectName = request.ProjectName,
                Description = request.Description,
            };
        }

        public static ProjectDTO mapToProjectDTO(Project request)
        {
            return new ProjectDTO
            {
                Id = request.Id,
                ProjectCode = request.ProjectCode,
                ProjectName = request.ProjectName,
                Description = request.Description,
                CreatedDate = request.createdDate,
                UpdatedDate = request.updatedDate,
            };
        }

        public static Project mapToProjectForUpdate(ProjectDTO request, Project response)
        {
            response.ProjectCode = request.ProjectCode;
            response.ProjectName = request.ProjectName;
            response.Description = request.Description;
            return response;
        }

        public static ProjectUser mapToProjectUser(ProjectUserDTO request, int projectId)
        {
            return new ProjectUser
            {
                ProjectId = projectId,
                UserId = request.UserId,
                JobPositionId = request.JobPositionId,
                LevelId = request.LevelId,
            };
        }
    }
}
