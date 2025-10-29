using ApiPedidos.Application.DTOs;
using ApiPedidos.Domain.Enums;


namespace ApiPedidos.Application.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoDto> CriarPedidoAsync();
        Task<List<PedidoDto>> ListarPedidosAsync();
        Task<PagedResultDto<PedidoDto>> ListarPedidosPaginadoAsync(int page, int pageSize);
        Task<PedidoDto?> ObterPorIdAsync(long id);
        Task<PedidoDto> AdicionarItemAsync(long pedidoId, long produtoId, decimal quantidade);
        Task<PedidoDto> RemoverItemAsync(long pedidoId, long produtoId);
        Task<PedidoDto> FecharPedidoAsync(long pedidoId);
        Task<PedidoDto> CancelarPedidoAsync(long pedidoId);
        Task<List<PedidoDto>> ListarPorStatusAsync(StatusPedido status);

    }
}
