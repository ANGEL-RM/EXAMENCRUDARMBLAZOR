using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDARM.Shared
{
    public class Respuesta<T>
    {
        public EstadosDeRespuesta Estado { get; set; }
        public Estatus Estatus { get; set; } = new Estatus();
        public T Datos { get; set; }
    }
    public enum EstadosDeRespuesta : int
    {
        NoProceso = 0,
        Correcto = 1,
        Error = 3
    }
}
