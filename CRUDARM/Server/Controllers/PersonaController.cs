using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDARM.Shared;
using CRUDARM.Shared.AccesoDatos;
using CRUDARM.Shared.DTO;
using CRUDARM.Shared.Persona;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CRUDARM.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private IConfiguration config;
        private AccesoDatos datos;
        public PersonaController(IConfiguration configuration, AccesoDatos accesoDatos)
        {
            config = configuration;
            datos = accesoDatos;
        }

        [HttpGet]
        [Route("ObtenerTotalPersonas")]
        public async Task<Respuesta<List<PersonaDTO>>> ObtenerTotalPersonas(long UsuarioId)
        {
            var respuesta = new Respuesta<List<PersonaDTO>> { Estado = EstadosDeRespuesta.Correcto, Datos = new List<PersonaDTO>() };
            try
            {
                var personasBD = await datos.Tbl_Personas.AsNoTracking().ToListAsync().ConfigureAwait(false);
                if (personasBD.Count != 0)
                {
                    foreach (var personaBD in personasBD)
                    {
                        PersonaDTO persona = new PersonaDTO()
                        {
                             PersonaId = personaBD.PersonaId,
                             nombre = personaBD.Nombre,
                              apellidoP = personaBD.ApellidoP,
                               apellidoM = personaBD.ApellidoM,
                                sexo = personaBD.Sexo,
                                 fechanacimiento = personaBD.Fechanacimiento,
                                  curp = personaBD.Curp
                        };
                        respuesta.Datos.Add(persona);
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.Estado = EstadosDeRespuesta.Error;
                respuesta.Estatus.Mensaje = $"ocurrio un error al obtener las Empresas: {ex.Message}";
            }
            return respuesta;
        }

        [HttpPost]
        [Route("CrearActualizarPersona")]
        public async Task<Respuesta<PersonaDTO>> CrearActualizarPersona([FromBody] PersonaDTO personaDTO)
        {
            var respuesta = new Respuesta<PersonaDTO> { Estado = EstadosDeRespuesta.Correcto, Datos = new PersonaDTO()};
            try
            {
                if (datos.Tbl_Personas.Any(p => p.PersonaId == personaDTO.PersonaId))
                {
                    var personaBD = await datos.Tbl_Personas.FirstAsync(p => p.PersonaId == personaDTO.PersonaId);
                        personaBD.Nombre = personaDTO.nombre;
                        personaBD.ApellidoP = personaDTO.apellidoP;
                        personaBD.ApellidoM = personaDTO.apellidoM;
                        personaBD.Sexo = personaDTO.sexo;
                        personaBD.Fechanacimiento = personaDTO.fechanacimiento;
                        personaBD.Curp = personaDTO.curp;
                }
                else
                {
                    var fechastrin = personaDTO.fechanacimiento.ToString("yyyy-MM-dd HH:mm:ss");
                    var fecha = DateTime.ParseExact(fechastrin, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    Tbl_Personas personaBD = new Tbl_Personas()
                    {
                           Nombre = personaDTO.nombre,
                           ApellidoP = personaDTO.apellidoP,
                             ApellidoM = personaDTO.apellidoM,
                              Sexo = personaDTO.sexo,
                               Fechanacimiento = fecha,
                                Curp = personaDTO.curp
                    };
                    await datos.Tbl_Personas.AddAsync(personaBD).ConfigureAwait(false);
                    personaDTO.PersonaId = personaBD.PersonaId;
                    respuesta.Estatus.Mensaje = "Registro Agregado Correctamente";
                    respuesta.Datos = personaDTO;
                }
                await datos.SaveChangesAsync(true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                respuesta.Estado = EstadosDeRespuesta.Error;
                respuesta.Estatus.Mensaje = $"Ocurrio un error al renombrar la empresa: {ex.Message}";
            }
            return respuesta;
        }

        [HttpGet]
        [Route("ObtenerInformacionPersonalPorId/{PersonaId}")]
        public async Task<Respuesta<PersonaDTO>> ObtenerInformacionPersonalPorId(long PersonaId)
        {
            var respuesta = new Respuesta<PersonaDTO> { Estado = EstadosDeRespuesta.Correcto, Datos = new PersonaDTO() };
            try
            {
                var personaBD = await datos.Tbl_Personas.AsNoTracking().FirstAsync(p => p.PersonaId == PersonaId).ConfigureAwait(false);
                if (personaBD != null)
                {
                    PersonaDTO personaDTO = new PersonaDTO()
                    {
                         PersonaId = personaBD.PersonaId,
                         nombre = personaBD.Nombre,
                         apellidoP = personaBD.ApellidoP,
                          apellidoM = personaBD.ApellidoM,
                           sexo = personaBD.Sexo,
                            fechanacimiento = personaBD.Fechanacimiento,
                            curp = personaBD.Curp
                    };
                    respuesta.Datos = personaDTO;
                }
            }
            catch (Exception ex)
            {
                respuesta.Estado = EstadosDeRespuesta.Error;
                respuesta.Estatus.Mensaje = $"ocurrio un error al obtener las Empresas: {ex.Message}";
            }
            return respuesta;
        }
    }
}