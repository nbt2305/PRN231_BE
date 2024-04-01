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
    public class LogWorkMapper
    {
        public static LogWorkDTO mapToLogWorkDTO(LogWork request)
        {
            return new LogWorkDTO
            {
                Id = request.Id,
                ProjectId = request.ProjectId,
                PhaseId = request.PhaseId,
                Task = request.Task,
                Date = request.Date,
                From = request.From,
                To = request.To,
                Description = request.Description,
            };
        }

        public static LogWorkResponse mapToLogWorkResponse(LogWork request, Project project, Phase phase)
        {
            return new LogWorkResponse
            {
                Id = request.Id,
                ProjectName = project.ProjectName,
                PhaseName = phase.Name,
                Task = request.Task,
                Date = request.Date,
                From = request.From,
                To = request.To,
                Description = request.Description,
            };
        }

        public static LogWork mapToLogWork(LogWorkDTO request)
        {
            return new LogWork
            {
                ProjectId = request.ProjectId,
                PhaseId = request.PhaseId,
                Task = request.Task,
                Date = request.Date,
                From = request.From,
                To = request.To,
                Description = request.Description,
            };
        }

        public static LogWork mapToLogWorkForUpdate(LogWorkDTO request, LogWork response)
        {
            response.ProjectId = request.ProjectId;
            response.PhaseId = request.PhaseId;
            response.Task = request.Task;
            response.Date = request.Date;
            response.From = request.From;
            response.To = request.To;
            response.Description = request.Description;
            return response;
        }
    }
}
