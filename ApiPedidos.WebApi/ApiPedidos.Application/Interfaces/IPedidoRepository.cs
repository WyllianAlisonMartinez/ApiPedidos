using ApiPedidos.Domain.Entities.Pedido;


namespace ApiPedidos.Application.Interfaces
{
    public interface IPedidoRepository
    {

        Task AddAsync(Pedido pedido);  
        Task UpdateAsync(Pedido pedido);
        Task<Pedido?> GetByIdAsync(long id);
        Task<List<Pedido>> ListAsync();
    }
}
