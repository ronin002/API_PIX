using API_PIX.Application.Interfaces;
using API_PIX.Data.Interfaces;
using API_PIX.Data.Repositories;
using API_PIX.Domain.ClientModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API_PIX.Web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly IClientRepository _clientRepository;
        private IClientService clientService { get; set; }
        public BaseController(IServiceProvider sp)
        {
            clientService = (IClientService)sp.GetService(typeof(IClientService));
        }

        protected Client ReadToken()
        {
            var idUser = User.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(x => x.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(idUser))
            {
                var client = new Client();
                client = _clientRepository.Get(Guid.Parse(idUser));
                return client;
            }
            else
            {
                return null;
            }
            throw new UnauthorizedAccessException("Token invalid");
        }
    }
}
