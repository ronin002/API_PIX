using API_PIX.Data.Interfaces;
using API_PIX.Domain.ClientModel;
using API_PIX.Domain.Error;
using API_PIX.Domain.Log;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace API_PIX.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        DataContext _context { get; set; }
        private ILogsService LogsService { get; set; }

        public AuthRepository(ILogsService logsService, DataContext context)
        {
            _context = context;
            LogsService = logsService;
        }

        public Client Login(string Email, string Password)
        {
            try
            {
                var client = _context.Clientes.Where(x => x.Email == Email && x.PassHash == Password).First();
                return client;
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Login error", "LX01 There was an error getting the user",
                    this.GetType().ToString());
            }
        }
    }
}
