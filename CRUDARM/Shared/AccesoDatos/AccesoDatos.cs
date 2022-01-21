using CRUDARM.Shared.Externos;
using CRUDARM.Shared.Persona;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CRUDARM.Shared.AccesoDatos
{
    public class AccesoDatos: DbContext
    {
        public AccesoDatos(DbContextOptions<AccesoDatos> options) : base(options)
        {

        }
        public DbSet<Tbl_Personas> Tbl_Personas { get; set; }
        public DbSet<Tbl_Pais> Tbl_Pais { get; set; }
        public DbSet<Tbl_Estados> Tbl_Estados { get; set; }
        public DbSet<Tbl_Ubicacion> Tbl_Ubicacion { get; set; }
        public DbSet<Tbl_HistoriaLab> Tbl_HistoriaLab { get; set; }
        public DbSet<Tbl_Contacto> Tbl_Contacto { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var configuracion = config.Build();
            var valor = configuracion.GetValue<string>("Connection");
            options.UseSqlServer(valor);
        }
    }
}
