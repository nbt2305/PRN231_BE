using Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IBaseService <T>
    {
        ResponseData<List<T>> GetList(string token);
        ResponseData<T> CreateNew(string token, T request);
        ResponseData<T> Update(string token, int id, T request);
        ResponseData<T> Delete(string token, int Id);
        ResponseData<T> GetById(string token, int Id);
    }
}
