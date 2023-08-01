using API_PIX.Data.Interfaces;
using API_PIX.Domain.ClientModel;
using API_PIX.Domain.Error;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace API_PIX.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        DataContext _context { get; set; }
        private ILogsService LogsService { get; set; }

        public ClientRepository(ILogsService logsService, DataContext context)
        {
            _context = context;
            LogsService = logsService;
        }

        public Client GetCPF(string CPF)
        {
            try
            {
                return _context.Clientes.Where(x => x.CPF == CPF).FirstOrDefault();
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "User error", "There was an error getting the user",
                    this.GetType().ToString());
            }
        }

        public Client GetCNPJ(string CNPJ)
        {
            try
            {
                return _context.Clientes.Where(x => x.CNPJ == CNPJ).FirstOrDefault();
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "User error", "There was an error getting the user",
                    this.GetType().ToString());
            }
        }

        public Client Get(Guid Id)
        {
            try
            {
                return _context.Clientes.Find(Id);
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "User error", "There was an error getting the user",
                    this.GetType().ToString());
            }
        }

        public Client GetEmail(string Email)
        {
            try
            {
                return _context.Clientes.Where(x=> x.Email == Email).FirstOrDefault();
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "User error", "There was an error getting the user",
                    this.GetType().ToString());
            }
        }

        public Client GetByName(string name)
        {
            try
            {
                return _context.Clientes.Where(x => x.NameCompleto.IndexOf(name) >= 0).FirstOrDefault();
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "User error", "There was an error getting the user",
                    this.GetType().ToString());
            }
        }

        public Client Add(Client Client)
        {
            try
            {
                _context.Clientes.Add(Client);
                _context.SaveChanges();
                return Client;
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "User error", "There was an error adding the user",
                    this.GetType().ToString());
            }
        }

        public Client Find(Guid id)
        {
            try
            {
                return _context.Clientes.Find(id);
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "User error", "There was an error finding the user",
                    this.GetType().ToString());
            }
        }

        public void Remove(Guid id)
        {
            try
            {
                var user = Find(id);
                _context.Clientes.Remove(user);
                _context.SaveChanges();
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "User error", "There was an error removing the user",
                    this.GetType().ToString());
            }
        }

        public Client Update(Client Client)
        {
            try
            {
                var i = _context.Clientes.Find(Client.Id) ;
                _context.Entry(i).State = EntityState.Detached;
                //_context.Entry(user).Property(u => u.password).IsModified = false;
                if (string.IsNullOrEmpty(Client.PassHash))
                    Client.PassHash = i.PassHash;
                //if (!string.IsNullOrEmpty(Client.PassHash) && Client.PassHash.Length < 32)
                //    Client.PassHash = HashingService.GetHash(Client.PassHash);
                //if (string.IsNullOrEmpty(Client.PassHash))
                //    return Client;
                if (Client.DtRegistered.Year < 1901) Client.DtRegistered = DateTime.Now;
                if (Client.DtLastLogin.Year < 1901) Client.DtLastLogin = DateTime.Now;
                _context.Clientes.Update(Client);
                _context.SaveChanges();
                return Client;
            }
            catch (HandledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw LogsService.HandleException(ex, "User error", "There was an error updating the user",
                    this.GetType().ToString());
            }
        }
    }
}
