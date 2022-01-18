using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDARM.Shared.Persona
{
    public class Tbl_Contacto
    {
        [Key]
        public long ContactoId { get; set; }
        public string Telefono { get; set; }
        public string Tipo { get; set; }
    }
}
