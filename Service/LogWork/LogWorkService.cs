using Entities;
using Repository;
using Service.Telegram;
using Shared.DTO;
using Shared.DTO.Response;
using Shared.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.LogWork
{
    public class LogWorkService : ILogWorkService
    {
        private readonly RepositoryContext _context;
        private readonly IUserService _userService;
        public LogWorkService(RepositoryContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public ResponseData<LogWorkDTO> CreateNew(string token, LogWorkDTO request)
        {
            string errors = request.ValidateInput(false);

            if (errors != null)
            {
                return new ResponseData<LogWorkDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = errors
                };
            }
            try
            {
                var userId = _userService.GetUserIdFromToken(token);
                var foundUser = _context.Users.FirstOrDefault(x => x.Id == userId);
                var logWork = LogWorkMapper.mapToLogWork(request);
                logWork.createdDate = DateTime.Now;
                logWork.UserId= userId;
                _context.LogWorks.Add(logWork);
                _context.SaveChanges();

                var telegram = new TelegramService("7125536679:AAF6Iu95kP0AnxanPVl2-9pXOspA37NbUBA");
                var message = foundUser.FullName + " đã tạo thành công LogWork mới với Task là " + logWork.Task;
                telegram.SendMessageAsync("1192099901", message);

                return new ResponseData<LogWorkDTO>
                {
                    Data = LogWorkMapper.mapToLogWorkDTO(logWork),
                    StatusCode = HttpStatusCode.OK,
                };

            }
            catch (Exception ex)
            {
                return new ResponseData<LogWorkDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Created Error"
                };
            }
        }

        public ResponseData<LogWorkDTO> Delete(string token, int Id)
        {
            var foundLogWork = _context.LogWorks.FirstOrDefault(x => x.Id == Id);
            if (foundLogWork == null)
            {
                return new ResponseData<LogWorkDTO>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ErrMsg = "Log Work not found!"
                };
            }
            var currentUser = _userService.GetUserIdFromToken(token);
            if (currentUser != foundLogWork.UserId)
            {
                return new ResponseData<LogWorkDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Permission denied!"
                };
            }

            _context.LogWorks.Remove(foundLogWork);
            _context.SaveChanges();
            return new ResponseData<LogWorkDTO>
            {
                StatusCode = HttpStatusCode.OK,
                ErrMsg = "Deleted!"
            };
        }


            public ResponseData<LogWorkDTO> GetById(string token, int Id)
        {
            var foundLogWork = _context.LogWorks.FirstOrDefault(x => x.Id == Id);
            if (foundLogWork == null)
            {
                return new ResponseData<LogWorkDTO>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ErrMsg = "Log Work Not Found!"
                };
            }

            return new ResponseData<LogWorkDTO>
            {
                Data = LogWorkMapper.mapToLogWorkDTO(foundLogWork),
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<List<LogWorkDTO>> GetList(string token)
        {
            throw new NotImplementedException();
        }

        public ResponseData<List<LogWorkResponse>> Search(string token, LogWorkDTO request)
        {
            var list = _context.LogWorks.Where(x =>
            (x.Task.ToLower().Contains(request.Task.ToLower()) || request.Task == null) &&
            (x.Description.ToLower().Contains(request.Description.ToLower()) || request.Description == null) &&
            (x.ProjectId == request.ProjectId || request.ProjectId == 0) &&
            (x.PhaseId == request.PhaseId || request.PhaseId == 0) &&
            (x.UserId == request.UserId || request.UserId == 0))
                .ToList();

            List<LogWorkResponse> results = new List<LogWorkResponse>();

            foreach (var item in list)
            {
                var project = _context.Projects.FirstOrDefault(x => x.Id == item.ProjectId);
                var phase = _context.Phases.FirstOrDefault(x => x.Id == item.PhaseId);
                var result = LogWorkMapper.mapToLogWorkResponse(item, project, phase);
                results.Add(result);
            }

            return new ResponseData<List<LogWorkResponse>>()
            {
                Data = results,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<LogWorkDTO> Update(string token, int id, LogWorkDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
