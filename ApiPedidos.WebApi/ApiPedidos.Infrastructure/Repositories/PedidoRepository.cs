using ApiPedidos.Application.Interfaces;
using ApiPedidos.Domain.Entities.Pedido;
using ApiPedidos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace ApiPedidos.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
        }

        public async Task<Pedido> GetByIdAsync(long id)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Pedido>> ListAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                .OrderByDescending(p => p.DataRegistro)
                .ToListAsync();
        }

        public async Task UpdateAsync(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }
    }
}
