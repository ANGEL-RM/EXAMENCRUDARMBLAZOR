using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDARM.Shared;
using CRUDARM.Shared.AccesoDatos;
using CRUDARM.Shared.Externos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CRUDARM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternosController : ControllerBase
    {
        private IConfiguration config;
        private AccesoDatos datos;
        public ExternosController(IConfiguration configuration, AccesoDatos accesoDatos)
        {
            config = configuration;
            datos = accesoDatos;
        }

        [HttpPost]
        [Route("CrearActualizarPais")]
        public async Task<Respuesta<Tbl_Pais>> CrearActualizarPais([FromBody] Tbl_Pais pais)
        {
            var respuesta = new Respuesta<Tbl_Pais> { Estado = EstadosDeRespuesta.Correcto, Datos = new Tbl_Pais() };
            try
            {
                if (datos.Tbl_Pais.Any(p => p.PaisId == pais.PaisId))
                {
                    var personaBD = await datos.Tbl_Pais.FirstAsync(p => p.PaisId == pais.PaisId);
                    personaBD.Nombre = pais.Nombre;
                }
                else
                {
                    await datos.Tbl_Pais.AddAsync(pais).ConfigureAwait(false);
                    await datos.SaveChangesAsync(true).ConfigureAwait(false);
                    respuesta.Estatus.Mensaje = "Registro Agregado Correctamente";
                    respuesta.Datos = pais;
                }
            }
            catch (Exception ex)
            {
                respuesta.Estado = EstadosDeRespuesta.Error;
                respuesta.Estatus.Mensaje = $"Ocurrio un error en el proceso: {ex.Message}";
            }
            return respuesta;
        }

        [HttpGet]
        [Route("ObtenerPaises")]
        public async Task<Respuesta<List<Tbl_Pais>>> ObtenerPaises()
        {
            var respuesta = new Respuesta<List<Tbl_Pais>> { Estado = EstadosDeRespuesta.Correcto, Datos = new List<Tbl_Pais>() };
            try
            {
                respuesta.Datos = await datos.Tbl_Pais.AsNoTracking().ToListAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                respuesta.Estado = EstadosDeRespuesta.Error;
                respuesta.Estatus.Mensaje = $"ocurrio un error al obtener los Países: {ex.Message}";
            }
            return respuesta;
        }

        [HttpGet]
        [Route("ObtenerEstadosporPais/{PaisId}")]
        public async Task<Respuesta<Tbl_ConsultaDTO>> ObtenerEstadosporPais(long PaisId)
        {
            var respuesta = new Respuesta<Tbl_ConsultaDTO> { Estado = EstadosDeRespuesta.Correcto, Datos = new Tbl_ConsultaDTO() };
            try
            {
                var pais = await datos.Tbl_Pais.FirstAsync(p => p.PaisId == PaisId).ConfigureAwait(false);
                respuesta.Datos.NombrePais = pais.Nombre;
                respuesta.Datos.estados = await datos.Tbl_Estados.AsNoTracking().Where(c => c.PaisId == PaisId).ToListAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                respuesta.Estado = EstadosDeRespuesta.Error;
                respuesta.Estatus.Mensaje = $"ocurrio un error al obtener los Estados: {ex.Message}";
            }
            return respuesta;
        }

        [HttpPost]
        [Route("CrearActualizarEstado")]
        public async Task<Respuesta<Tbl_Estados>> CrearActualizarEstado([FromBody] Tbl_Estados estado)
        {
            var respuesta = new Respuesta<Tbl_Estados> { Estado = EstadosDeRespuesta.Correcto, Datos = new Tbl_Estados() };
            try
            {
                if (datos.Tbl_Estados.Any(p => p.EstadoId ==  estado.EstadoId))
                {
                    var personaBD = await datos.Tbl_Estados.FirstAsync(p => p.EstadoId == estado.EstadoId);
                    personaBD.Nombre = estado.Nombre;
                }
                else
                {
                    await datos.Tbl_Estados.AddAsync(estado).ConfigureAwait(false);
                    await datos.SaveChangesAsync(true).ConfigureAwait(false);
                    respuesta.Estatus.Mensaje = "Registro Agregado Correctamente";
                    respuesta.Datos = estado;
                }
            }
            catch (Exception ex)
            {
                respuesta.Estado = EstadosDeRespuesta.Error;
                respuesta.Estatus.Mensaje = $"Ocurrio un error en el proceso: {ex.Message}";
            }
            return respuesta;
        }

        [HttpGet]
        [Route("ObtenerEstados")]
        public async Task<Respuesta<List<Tbl_Estados>>> ObtenerEstados()
        {
            var respuesta = new Respuesta<List<Tbl_Estados>> { Estado = EstadosDeRespuesta.Correcto, Datos = new List<Tbl_Estados>() };
            try
            {
                respuesta.Datos = await datos.Tbl_Estados.AsNoTracking().ToListAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                respuesta.Estado = EstadosDeRespuesta.Error;
                respuesta.Estatus.Mensaje = $"ocurrio un error al obtener los estados: {ex.Message}";
            }
            return respuesta;
        }
    }
}
