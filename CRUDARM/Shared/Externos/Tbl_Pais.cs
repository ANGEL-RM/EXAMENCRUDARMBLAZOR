using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDARM.Shared.Externos
{
    public class Tbl_Pais
    {
        [Key]
        public long PaisId { get; set; }
        public string Nombre { get; set; }
    }
}
