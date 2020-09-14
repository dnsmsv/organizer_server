using Microsoft.EntityFrameworkCore;

namespace Organizer.Models
{
    public class OrganizerContext : DbContext
    {
        public OrganizerContext()
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.
                UseLazyLoadingProxies().
                UseSqlServer("Server=ASU-MDV\\MDV;Database=Organizer;Trusted_Connection=True;");
        }

        public DbSet<Date> Dates { get; set; }
        public DbSet<Task> Tasks { get; set; }
    }
}
