using API_PIX.Domain.ClientModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_PIX.Data.Interfaces
{
    public interface IClientRepository
    {
        Client Add(Client Client);
        Client Update(Client Client);
        void Remove(Guid Id);
        Client GetCPF(string CPF);
        Client GetEmail(string Email);
        Client GetCNPJ(string CNPJ);
        Client GetByName(string name);
        Client Get(Guid Id);
        //IQueryable<ClientPF> GetAll(); //Lembrar de inserir paginacao
    }
}
