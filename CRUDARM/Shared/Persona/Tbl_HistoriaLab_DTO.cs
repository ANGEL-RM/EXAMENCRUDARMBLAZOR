using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDARM.Shared.Persona
{
    public class Tbl_HistoriaLab_DTO
    {
        public long historialId { get; set; }
        public string DTrabajo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public string DescAct { get; set; }
        public long PersonaId { get; set; }
        public string FechaminFechafinal { get; set; }
    }
}
