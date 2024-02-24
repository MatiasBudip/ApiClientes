using Microsoft.EntityFrameworkCore;

namespace Clientes.Models
{
    public class ClientesContext : DbContext
    { 
        public ClientesContext(DbContextOptions<ClientesContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
    }
}
