using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using API_PIX.Domain.ClientModel;
using API_PIX.Domain.Log;

namespace API_PIX.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {

        }

        public DbSet<Client> Clientes { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
