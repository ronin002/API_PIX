using API_PIX.Domain.ClientModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_PIX.Application.Interfaces
{
    public interface IAuthService
    {
        Client AuthenticateClient(string Email, string PashHash);
    }
}
