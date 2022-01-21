using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDARM.Shared.Persona
{
    public class Tbl_Ubicacion
    {
        [Key]
        public long UbicacionId { get; set; }
        public string Direccion { get; set; }
        public string Ninterno { get; set; }
        public string Nexterno { get; set; }
        public string Colonia { get; set; }
        public long Pais { get; set; }
        public long Estado { get; set; }
        public long PersonaId { get; set; }
    }
}
