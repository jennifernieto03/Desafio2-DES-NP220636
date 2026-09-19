using PedidosAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PedidosAPI.Data
{
    public class PedidosDbContext : DbContext
    {
        public PedidosDbContext(
        DbContextOptions<PedidosDbContext> options)
        : base(options)
        {
        }

        public DbSet<Pedido> Pedidos => Set<Pedido>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
                .HasIndex(p => p.NumeroPedido)
                .IsUnique();
        }
    }
}
