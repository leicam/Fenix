using Fenix.nsCliente;
using Microsoft.EntityFrameworkCore;

namespace Fenix.nsContext
{
    public class Context : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Fenix");
        }
    }
}