using ApiPedidos.Domain.Entities;
using ApiPedidos.Domain.Entities.Pedido;
using Microsoft.EntityFrameworkCore;

namespace ApiPedidos.Infrastructure.Persistence
{
    public class AppDbContext : DbContext

    {
        public DbSet<Produto> Produtos => Set<Produto>();
        public DbSet<Pedido> Pedidos => Set<Pedido>();
        public DbSet<PedidoItem> PedidoItens => Set<PedidoItem>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Nome).IsRequired().HasMaxLength(150);
                entity.Property(p => p.PrecoCusto).HasPrecision(18, 2);
                entity.Property(p => p.PrecoVenda).HasPrecision(18, 2);
                entity.Property(p => p.Quantidade).HasPrecision(18, 3);
            });

            
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.DataRegistro);
                entity.Property(p => p.DataAlteracao);
                entity.Property(p => p.StatusPedido);
                entity.HasMany(p => p.Itens)
                       .WithOne()
      .HasForeignKey(i => i.PedidoId)
      .OnDelete(DeleteBehavior.Cascade);

                entity.Navigation(p => p.Itens)
                      .UsePropertyAccessMode(PropertyAccessMode.Field);

            });


            modelBuilder.Entity<PedidoItem>(entity =>
            {
                entity.HasKey(i => i.Id);

                
                entity.Property(i => i.Id)
                      .ValueGeneratedOnAdd();

                entity.Property(i => i.ProdutoNome).IsRequired().HasMaxLength(150);
                entity.Property(i => i.PrecoUnitario).HasPrecision(18, 2);
                entity.Property(i => i.Quantidade).HasPrecision(18, 3);
            });


        }
    }
 }
