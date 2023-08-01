using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using API_PIX.Domain.Log;

namespace API_PIX.Data.Interfaces
{
    public interface ILogsRepository
    {
        Log Add(Log item);
        void AddMultiple(List<Log> logs);
        Log Update(Log item);
        void Remove(string key);
        Log Get(string id);
        //IQueryable<Log> GetAll();
    }
}
