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
                respuesta.Estatus.Mensaje = $"ocurrio un error: {ex.Message}";
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
                if (!datos.Tbl_Personas.Any(p => p.PersonaId == personaDTO.PersonaId))
                { 
                    var fechastring = personaDTO.fechanacimiento.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    var fecha = DateTime.ParseExact(fechastring, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
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
                    await datos.SaveChangesAsync(true).ConfigureAwait(false);
                    Tbl_Ubicacion ubicacionBD = new Tbl_Ubicacion()
                    {
                        Direccion = personaDTO.direccion,
                        Ninterno = personaDTO.ninterno,
                        Nexterno = personaDTO.nexterno,
                        Colonia = personaDTO.colonia,
                        Pais = personaDTO.pais,
                        Estado = personaDTO.estado,
                        PersonaId = personaBD.PersonaId
                    };
                    await datos.Tbl_Ubicacion.AddAsync(ubicacionBD).ConfigureAwait(false);
                    List<ContactoDTO> ListaNuevaContacto = new List<ContactoDTO>();
                    foreach (var contactos in personaDTO.ListaContactos)
                    {
                        if (!string.IsNullOrEmpty(contactos.Telefono))
                        {
                            ListaNuevaContacto.Add(contactos);
                        }
                    }
                    List<Tbl_Contacto> ListacontactosBD = new List<Tbl_Contacto>();
                    foreach (var contactos in ListaNuevaContacto)
                    {
                        Tbl_Contacto contactoBD = new Tbl_Contacto()
                        {
                            ContactoId = contactos.ContactoId,
                            Telefono = contactos.Telefono,
                            Tipo = contactos.Tipo,
                             PersonaId = personaBD.PersonaId
                        };
                        ListacontactosBD.Add(contactoBD);
                    }
                    await datos.Tbl_Contacto.AddRangeAsync(ListacontactosBD).ConfigureAwait(false);
                    List<Tbl_HistoriaLab_DTO> ListaNuevaHistorial = new List<Tbl_HistoriaLab_DTO>();
                    foreach (var historial in personaDTO.ListHistoriaLab)
                    {
                        if (!string.IsNullOrEmpty(historial.FechaInicio.ToString()) && !string.IsNullOrEmpty(historial.DTrabajo))
                        {
                            ListaNuevaHistorial.Add(historial);
                        }
                    }
                    var conteo = ListaNuevaHistorial.Count;
                    List<Tbl_HistoriaLab> ListahistorialBD = new List<Tbl_HistoriaLab>();
                    datos.Database.ExecuteSqlRaw($"UPDATE Tbl_HistoriaLab SET historialId=historialId + {conteo}");
                    foreach (var historial in ListaNuevaHistorial)
                    {
                        Tbl_HistoriaLab historialBD = new Tbl_HistoriaLab()
                        {
                             historialId = conteo,
                              DTrabajo = historial.DTrabajo,
                               FechaInicio = historial.FechaInicio.Value,
                                FechaFinal = historial.FechaFinal.Value,
                                 DescAct = historial.DescAct,
                                  PersonaId = personaBD.PersonaId
                        };
                        ListahistorialBD.Add(historialBD);
                        conteo--;
                    }
                    await datos.Tbl_HistoriaLab.AddRangeAsync(ListahistorialBD).ConfigureAwait(false);
                    await datos.SaveChangesAsync(true).ConfigureAwait(false);
                    personaDTO.PersonaId = personaBD.PersonaId;
                    respuesta.Estatus.Mensaje = "Registro Agregado Correctamente";
                    respuesta.Datos = personaDTO;
                }
            }
            catch (Exception ex)
            {
                respuesta.Estado = EstadosDeRespuesta.Error;
                respuesta.Estatus.Mensaje = $"Ocurrio un error: {ex.Message}";
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
                var direccionBD = await datos.Tbl_Ubicacion.AsNoTracking().FirstAsync(p => p.PersonaId == PersonaId).ConfigureAwait(false);
                var paisBD = await datos.Tbl_Pais.AsNoTracking().FirstAsync(p => p.PaisId == direccionBD.Pais).ConfigureAwait(false);
                string NombreEstado = "";
                if (direccionBD.Estado != 0)
                {
                    var EstadoBD = await datos.Tbl_Estados.AsNoTracking().FirstAsync(p => p.EstadoId == direccionBD.Estado).ConfigureAwait(false);
                    NombreEstado = EstadoBD.Nombre;
                }
                if (personaBD != null && direccionBD != null)
                {
                    PersonaDTO personaDTO = new PersonaDTO()
                    {
                        PersonaId = personaBD.PersonaId,
                        nombre = personaBD.Nombre,
                        apellidoP = personaBD.ApellidoP,
                        apellidoM = personaBD.ApellidoM,
                        sexo = personaBD.Sexo,
                        fechanacimiento = personaBD.Fechanacimiento,
                        curp = personaBD.Curp,
                        direccion = direccionBD.Direccion,
                        ninterno = direccionBD.Ninterno,
                        nexterno = direccionBD.Nexterno,
                        colonia = direccionBD.Colonia,
                        Nombrepais = paisBD.Nombre,
                        Nombreestado = NombreEstado
                    };
                    var contactos = await datos.Tbl_Contacto.AsNoTracking().Where(c => c.PersonaId == PersonaId).ToListAsync().ConfigureAwait(false);
                    if (contactos.Count != 0)
                    {
                        foreach (var contacto in contactos)
                        {
                            ContactoDTO contactoDTO = new ContactoDTO()
                            {
                                 ContactoId = contacto.ContactoId,
                                  Telefono = contacto.Telefono,
                                   Tipo = contacto.Tipo
                            };
                            personaDTO.ListaContactos.Add(contactoDTO);
                        }
                    }
                    var listhistoria = await datos.Tbl_HistoriaLab.AsNoTracking().Where(c => c.PersonaId == PersonaId).ToListAsync().ConfigureAwait(false);
                    if (listhistoria.Count != 0)
                    {
                        foreach (var historia in listhistoria)
                        {
                            Tbl_HistoriaLab_DTO historiaDTO = new Tbl_HistoriaLab_DTO()
                            {
                                 historialId = historia.historialId,
                                  DTrabajo = historia.DTrabajo,
                                   FechaInicio = historia.FechaInicio,
                                    FechaFinal = historia.FechaFinal,
                                     DescAct = historia.DescAct,
                            };
                            personaDTO.ListHistoriaLab.Add(historiaDTO);
                        }
                    }
                    respuesta.Datos = personaDTO;
                }
            }
            catch (Exception ex)
            {
                respuesta.Estado = EstadosDeRespuesta.Error;
                respuesta.Estatus.Mensaje = $"ocurrio un error: {ex.Message}";
            }
            return respuesta;
        }
    }
}