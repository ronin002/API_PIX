using API_PIX.Data.Interfaces;
using API_PIX.Domain.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_PIX.Data.Repositories
{
    public class LogsRepository : ILogsRepository
    {
        DataContext _context { get; set; }

        public LogsRepository(DataContext context)
        {
            _context = context;
        }

        public Log Get(string id)
        {
            return _context.Logs.Find(id);
        }

        public IQueryable<Log> GetAll()
        {
            return _context.Logs.AsQueryable();
        }

        public Log Add(Log log)
        {
            _context.Logs.Add(log);
            _context.SaveChanges();
            return log;
        }

        public void AddMultiple(List<Log> logs)
        {
            foreach (var log in logs)
            {
                _context.Logs.Add(log);
            }
            _context.SaveChanges();
        }

        public Log Find(string id)
        {
            return _context.Logs.Find(id);
        }

        public void Remove(string id)
        {
            var log = Find(id);
            _context.Logs.Remove(log);
            _context.SaveChanges();
        }

        public Log Update(Log log)
        {
            var i = _context.Logs.Find(log.Id);
            _context.Logs.Update(i);
            _context.SaveChangesAsync();
            return i;
        }
    }
}
