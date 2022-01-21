using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDARM.Shared.Externos
{
    public class Tbl_ConsultaDTO
    {
        public string NombrePais { get; set; }
        public List<Tbl_Estados> estados { get; set; } = new List<Tbl_Estados>();
        
    }
}
