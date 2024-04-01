using Entities;
using Shared.DTO;
using Shared.DTO.Request;
using Shared.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IProjectService : IBaseService<ProjectDTO>
    {
        public ResponseData<ProjectResponse> GetDetail(string token, int Id);
        ResponseData<List<ProjectDTO>> Search(string token, ProjectSearch request);
        ResponseData<PhasesInProject> GetPhasesInProject(string token, int projectId);
        ResponseData<List<ReportResponse>> GetReport(string token);
    }
}
