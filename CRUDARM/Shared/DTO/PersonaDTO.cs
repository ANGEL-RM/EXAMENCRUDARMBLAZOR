using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUDARM.Shared.Persona;

namespace CRUDARM.Shared.DTO
{
    public class PersonaDTO
    {

        private const string validacionsololetras = @"^[a-zA-ZñÑ]+$";
        private const string validacioncurp = @"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$";
        public long PersonaId { get; set; }
        [Required(ErrorMessage = "El Nombre es requerido")]
        [RegularExpression(validacionsololetras, ErrorMessage = "Ingresar solo letras")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "El Apellido Paterno es requerido")]
        [RegularExpression(validacionsololetras, ErrorMessage = "Ingresar solo letras")]
        public string apellidoP { get; set; }
        public string apellidoM { get; set; }
        public string sexo { get; set; }
        [Required(ErrorMessage = "La Fecha de Nacimiento es requerida")]
        public DateTime? fechanacimiento { get; set; }
        [Required(ErrorMessage = "La Curp es requerida")]
        [RegularExpression(validacioncurp, ErrorMessage = "Ingresar Curp valida")]
        public string curp { get; set; }
        [Required(ErrorMessage = "La Direccion es requerida")]
        public string direccion { get; set; }
        [Required(ErrorMessage = "El Numero Interno es requerido")]
        public string ninterno { get; set; }
        public string nexterno { get; set; }
        public string colonia { get; set; }
        [Required(ErrorMessage = "El País es requerido")]
        public long pais { get; set; }
        public long estado { get; set; }
        public string Nombrepais { get; set; }
        public string Nombreestado { get; set; }

        public List<Tbl_HistoriaLab_DTO> ListHistoriaLab { get; set; } = new List<Tbl_HistoriaLab_DTO>();
        public List<ContactoDTO> ListaContactos { get; set; } = new List<ContactoDTO>();
    }
}
