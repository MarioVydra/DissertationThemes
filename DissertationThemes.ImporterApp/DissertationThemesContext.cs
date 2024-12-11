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
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DissertationThemes");

            if (!Directory.Exists(appDataPath)) 
            {
                Directory.CreateDirectory(appDataPath);    
            }
            var dbPath = Path.Combine(appDataPath, "DissertationThemes.db");

            options.UseSqlite($"Data Source={dbPath}");
            //Console.WriteLine(dbPath);
            //options.UseSqlite($"Data Source=Blogging.db");
        }
    }
}
