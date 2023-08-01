using API_PIX.Application.Interfaces;
using API_PIX.Data.Interfaces;
using API_PIX.Data.Repositories;
using API_PIX.Domain.ClientModel;
using API_PIX.Domain.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_PIX.Application.Services
{
    public class AuthService : IAuthService
    {
        IAuthRepository _AuthRepository { get; set; }
        ILogsService LogsService { get; set; }

        public AuthService(IAuthRepository authRepository,
            ILogsService logsService)
        {
            _AuthRepository = authRepository;
            LogsService = logsService;
        }
        public Client AuthenticateClient(string Email, string Pass)
        {
            try
            {
                //var result = ClientRepository.GetAll().FirstOrDefault(x =>
                // (x.emailAddress == u.emailAddress || x.ClientName == u.emailAddress) && x.passHash == u.passHash);
                Pass = HashingService.GetHash(Pass);
                return _AuthRepository.Login(Email, Pass); 
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "Client error", "There was an error authenticating the Client", this.GetType().ToString());
            }
        }
    }
}
