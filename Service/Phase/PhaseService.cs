using Microsoft.EntityFrameworkCore;
using Repository;
using Shared.DTO;
using Shared.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.Phases
{
    public class PhaseService : IPhaseService
    {
        private readonly RepositoryContext _context;
        public PhaseService(RepositoryContext context)
        {
            _context = context;
        }

        public ResponseData<PhaseDTO> CreateNew(string token, PhaseDTO request)
        {
            {
                string errors = request.ValidateInput(false);

                if (errors != null)
                {
                    return new ResponseData<PhaseDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = errors
                    };
                }
                try
                {
                    var foundPhase = _context.Phases.FirstOrDefault(x => x.Name == request.Name);
                    if (foundPhase != null)
                    {
                        return new ResponseData<PhaseDTO>
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            ErrMsg = "Phase already exited!"
                        };
                    }

                    var phase = PhaseMapper.mapToPhase(request);
                    phase.active = true;
                    phase.createdDate = DateTime.Now;
                    _context.Phases.Add(phase);
                    _context.SaveChanges();


                    return new ResponseData<PhaseDTO>
                    {
                        Data = PhaseMapper.mapToPhaseDTO(phase),
                        StatusCode = HttpStatusCode.OK,
                    };

                }
                catch (Exception ex)
                {
                    return new ResponseData<PhaseDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Created Error"
                    };
                }
            }
        }

        public ResponseData<PhaseDTO> Delete(string token, int Id)
        {
            var foundPhase = _context.Phases.FirstOrDefault(x => x.Id == Id);
            if (foundPhase == null)
            {
                return new ResponseData<PhaseDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Phase not found!"
                };
            }

            var assignedPhase = _context.ProjectPhases.Where(x => x.PhaseId == Id);
            //if (assignedPhase.Count() > 0){
            //    return new ResponseData<PhaseDTO>
            //    {
            //        StatusCode = HttpStatusCode.InternalServerError,
            //        ErrMsg = "Phase is being use!"
            //    };
            //}

            var response = new ResponseData<PhaseDTO>
            {
                Data = PhaseMapper.mapToPhaseDTO(foundPhase),
                StatusCode = HttpStatusCode.OK,
            };

            _context.RemoveRange(assignedPhase);
            _context.Remove(foundPhase);
            _context.SaveChanges();

            return response;


        }

        public ResponseData<PhaseDTO> GetById(string token, int Id)
        {
            var response = _context.Phases.FirstOrDefault(x => x.Id == Id);
            if (response == null)
            {
                return new ResponseData<PhaseDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Phase not found!"
                };
            }

            return new ResponseData<PhaseDTO>
            {
                Data = PhaseMapper.mapToPhaseDTO(response),
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<List<PhaseDTO>> GetList(string token)
        {
            var list = _context.Phases.ToList();
            var result = list.Select(x => PhaseMapper.mapToPhaseDTO(x)).ToList();
            return new ResponseData<List<PhaseDTO>>
            {
                Data = result,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<List<PhaseDTO>> Search(string token, PhaseDTO request)
        {
            var list = _context.Phases.Where(x =>
            (x.Name.ToLower().Contains(request.Name.ToLower()) || request.Name == null) &&
            (x.Description.ToLower().Contains(request.Description.ToLower()) || request.Description == null))
                .ToList();
            var data = list.Select(u => PhaseMapper.mapToPhaseDTO(u)).ToList();

            return new ResponseData<List<PhaseDTO>>()
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<PhaseDTO> Update(string token, int id, PhaseDTO request)
        {
            string errors = request.ValidateInput(false);

            if (errors != null)
            {
                return new ResponseData<PhaseDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = errors
                };
            }

            try
            {
                var foundPhase = _context.Phases.FirstOrDefault(o => o.Id == id);
                if (foundPhase == null)
                {
                    return new ResponseData<PhaseDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Phase not found!"
                    };
                }

                if (foundPhase.Name != request.Name &&
                        _context.Phases.FirstOrDefault(x => x.Name == request.Name) != null)
                {   
                    return new ResponseData<PhaseDTO>
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrMsg = "Phase Name already existed!"
                    };
                }
                foundPhase = PhaseMapper.mapToPhaseForUpdate(request, foundPhase);
                _context.Phases.Update(foundPhase);
                _context.SaveChanges();

                return new ResponseData<PhaseDTO>
                {
                    Data = PhaseMapper.mapToPhaseDTO(foundPhase),
                    StatusCode = HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<PhaseDTO>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrMsg = "Updated Error"
                };
            }
        }
    }
}
