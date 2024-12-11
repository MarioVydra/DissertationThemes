using Microsoft.EntityFrameworkCore;

namespace DissertationThemes.SharedLibrary
{
    public class DissertationThemesContext : DbContext
    {
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }

        public DbSet<StProgram> StProgram { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ""MojaApp, ""as.db)
            options.UseSqlite($"Data Source=Blogging.db");
        }
    }
}
