
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_PIX.Domain.ClientModel
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string CPF { get; set; }

        [Required]
        public string CNPJ { get; set; }

        [Required]
        public string NameCompleto { get; set; }

        [JsonIgnore]
        public string PassHash { get; set; }

        public DateTime DtRegistered { get; set; }
        public DateTime DtLastLogin { get; set; }
    }
}
