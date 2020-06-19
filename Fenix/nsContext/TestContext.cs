using Microsoft.EntityFrameworkCore;

namespace Fenix.WebApi.nsContext
{
    public class TestContext : AbstractContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Fenix");
        }
    }
}