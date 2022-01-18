using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDARM.Shared;

namespace CRUDARM.Client.Administracion
{
    public interface IManager
    {
        Task<Respuesta<R>> Post<T, R>(string url, T enviar);

        Task<Respuesta<T>> Get<T>(string url);

        Task<Respuesta<string>> PostString<T>(string url, T enviar);
    }
}
