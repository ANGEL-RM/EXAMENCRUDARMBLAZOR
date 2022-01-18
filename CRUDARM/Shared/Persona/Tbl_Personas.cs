using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDARM.Shared.Persona
{
    public class Tbl_Personas
    {
        [Key]
        public long PersonaId { get; set; }
        public string Nombre { get; set; }
        public string ApellidoP { get; set; }
        public string ApellidoM { get; set; }
        public string Sexo { get; set; }
        public DateTime Fechanacimiento { get; set; }
        public string Curp { get; set; }
    }
}
