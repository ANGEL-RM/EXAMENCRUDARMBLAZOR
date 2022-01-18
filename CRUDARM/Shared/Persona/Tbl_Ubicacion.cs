using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDARM.Shared.Persona
{
    public class Tbl_Ubicacion
    {
        public long UbicacionId { get; set; }
        public string Direccion { get; set; }
        public string Ninterno { get; set; }
        public string Nexterno { get; set; }
        public string Colonia { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public long PersonaId { get; set; }
    }
}
