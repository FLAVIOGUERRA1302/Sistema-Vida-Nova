using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaVidaNova.Models.FromSql
{
    
    public class DoadorComQuantidadeDeDoacoes
    {
        [Key]
        public int Id { get; set; }


        public string Tipo { get; set; }

        public string NomeRazaoSocial { get; set; }

        public string CpfCnpj { get; set; }

        public double ValorDoado { get; set; }
    }
}
