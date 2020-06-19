using Fenix.nsCliente;
using Microsoft.EntityFrameworkCore;

namespace Fenix.WebApi.nsContext
{
    public abstract class AbstractContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
    }
}