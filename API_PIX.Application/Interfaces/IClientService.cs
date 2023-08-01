using API_PIX.Domain.ClientModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_PIX.Application.Interfaces
{
    public interface IClientService
    {
        string ValidateClient(Client u);
        Client RegisterClient(Client u);
        Client GetClient(Guid id);
        Client GetClientByCPF(string CPF);
        Client GetClientByCNPJ(string CNPJ);
        void UpdateLastLogin(string CPF, DateTime updatedDate);
        Client GetClientByEmail(string email);
        Client GetClientByClientname(string Clientname);
        //bool UpdateClient(Client u);
 
        //Client AuthenticateClient(Client u);
        //Client ResetPassword(string email, string IpAddress);
        //IEnumerable<Client> SearchClients(string searchTerms);

    }
}
