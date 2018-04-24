using Xamarin.Forms;
using Microsoft.EntityFrameworkCore;
using DemoGAB2018CDMX.Modelos;
using DemoGAB2018CDMX.Servicios;

namespace DemoGAB2018CDMX.Datos
{
    public class BaseDatos : DbContext
    {
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<Emocion> Emocion { get; set; }

        private readonly string rutaBD;

        public BaseDatos(string rutaBD)
        {
            this.rutaBD = rutaBD;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = DependencyService.Get<IBaseDatos>().GetDatabasePath();
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}
