using ApiPedidos.Domain.Entities;



namespace ApiPedidos.Application.Interfaces
{
    public interface IProdutoRepository
    {
        Task<List<Produto>> GetAllAsync();
        Task<Produto?> GetByIdAsync(long id);
        Task AddAsync(Produto produto);
        Task UpdateAsync(Produto produto);
        Task DeleteAsync(Produto produto);
        Task<List<Produto>> GetInativosAsync();
        Task<Produto?> GetByIdIncluindoInativoAsync(long id);


    }
}
