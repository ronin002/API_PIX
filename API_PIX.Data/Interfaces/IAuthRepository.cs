using API_PIX.Domain.ClientModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_PIX.Data.Interfaces
{
    public interface IAuthRepository
    {
        Client Login(string Email, string Password);
    }
}
