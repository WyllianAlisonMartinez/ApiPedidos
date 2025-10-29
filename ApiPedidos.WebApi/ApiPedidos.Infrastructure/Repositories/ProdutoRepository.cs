using ApiPedidos.Application.Interfaces;
using ApiPedidos.Domain.Entities;
using ApiPedidos.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ApiPedidos.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Produto produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Produto>> GetAllAsync()
        {
            return await _context.Produtos
                .Where(p => p.IsAtivo)
                .ToListAsync();
        }

        public async Task<Produto?> GetByIdAsync(long id)
        {
            return await _context.Produtos
                .FirstOrDefaultAsync(p => p.Id == id && p.IsAtivo);
        }

        public async Task UpdateAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Produto produto)
        {
            produto.Inativar();
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Produto>> GetInativosAsync()
        {
            return await _context.Produtos
                .Where(p => !p.IsAtivo)
                .ToListAsync();
        }

        public async Task<Produto?> GetByIdIncluindoInativoAsync(long id)
        {
            return await _context.Produtos
                .FirstOrDefaultAsync(p => p.Id == id);
        }



    }
}
