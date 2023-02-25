using Microsoft.EntityFrameworkCore;

namespace P1_practica.Models
{
    public class equiposContext : DbContext
    {
        public equiposContext(DbContextOptions<equiposContext> options) : base(options) {
        
        }

        public DbSet<equipos> equipos { get; set; }
    }
}
