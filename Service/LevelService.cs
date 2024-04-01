using Repository;
using Shared.DTO;
using Shared.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LevelService : ICommonCodeService
    {
        private readonly RepositoryContext _context;
        public LevelService(RepositoryContext context)
        {
            _context = context;
        }
        public ResponseData<LevelDTO> CreateNew(string token, LevelDTO request)
        {
            throw new NotImplementedException();
        }

        public ResponseData<LevelDTO> Delete(string token, int Id)
        {
            throw new NotImplementedException();
        }

        public ResponseData<LevelDTO> GetById(string token, int Id)
        {
            throw new NotImplementedException();
        }

        public ResponseData<List<LevelDTO>> GetList(string token)
        {
            var res = _context.CommonCodes.Where(x => x.Type == "Level").ToList();
            var result = res.Select(x => CommonCodeMapper.mapToLevelDTO(x)).ToList();
            return new ResponseData<List<LevelDTO>>
            {
                Data = result,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public ResponseData<LevelDTO> Update(string token, int id, LevelDTO request)
        {
            throw new NotImplementedException();
        }
    }
}
