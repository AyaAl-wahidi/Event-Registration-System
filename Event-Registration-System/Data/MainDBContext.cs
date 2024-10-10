using Microsoft.EntityFrameworkCore;
using Event_Registration_System.Models;

namespace Event_Registration_System.Data
{
    public class MainDBContext : DbContext
    {

        public MainDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Event> Event { get; set; }

        public DbSet<Registration> Registration { get; set; } 
    }
}