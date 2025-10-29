using ApiPedidos.Application.DTOs;
using ApiPedidos.Application.Interfaces;
using ApiPedidos.Domain.Entities.Pedido;
using ApiPedidos.Domain.Enums;
using ApiPedidos.Domain.Exceptions;


namespace ApiPedidos.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IProdutoRepository _produtoRepository;

        public PedidoService(IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<List<PedidoDto>> ListarPorStatusAsync(StatusPedido status)
        {
            var pedidos = await _pedidoRepository.ListAsync();
            var filtrados = pedidos.Where(p => p.StatusPedido == status)
                                   .Select(MapToDto)
                                   .ToList();

            return filtrados;
        }


        public async Task<PedidoDto> CriarPedidoAsync()
        {
            var pedido = new Pedido();
            await _pedidoRepository.AddAsync(pedido);

            pedido = await _pedidoRepository.GetByIdAsync(pedido.Id)
                     ?? throw new MessageException("Erro ao criar pedido.");

            return MapToDto(pedido);
        }

        public async Task<PagedResultDto<PedidoDto>> ListarPedidosPaginadoAsync(int page, int pageSize)
        {
            var pedidos = await _pedidoRepository.ListAsync();

            var totalItems = pedidos.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var skip = (page - 1) * pageSize;

            var pedidosPaginados = pedidos
                .Skip(skip)
                .Take(pageSize)
                .Select(MapToDto)
                .ToList();

            return new PagedResultDto<PedidoDto>
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Items = pedidosPaginados
            };
        }


        public async Task<List<PedidoDto>> ListarPedidosAsync()
        {
            var pedidos = await _pedidoRepository.ListAsync();
            return pedidos.Select(MapToDto).ToList();
        }

        public async Task<PedidoDto?> ObterPorIdAsync(long id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            return pedido == null ? null : MapToDto(pedido);
        }

        public async Task<PedidoDto> AdicionarItemAsync(long pedidoId, long produtoId, decimal quantidade)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId)
                ?? throw new MessageException("Pedido não encontrado.");



            var produto = await _produtoRepository.GetByIdIncluindoInativoAsync(produtoId)
                ?? throw new MessageException("Produto não encontrado.");

            if (!produto.IsAtivo)
                throw new MessageException($"O produto {produto.Nome} está inativo.");


            pedido.AdicionarItemPedido(produto, quantidade);

            await _pedidoRepository.UpdateAsync(pedido);

            pedido = await _pedidoRepository.GetByIdAsync(pedidoId)
                ?? throw new MessageException("Erro ao carregar pedido.");

            return MapToDto(pedido);
        }


        public async Task<PedidoDto> RemoverItemAsync(long pedidoId, long produtoId)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId)
                ?? throw new MessageException("Pedido não encontrado.");

            pedido.RemoverItem(produtoId);

            await _pedidoRepository.UpdateAsync(pedido);

            pedido = await _pedidoRepository.GetByIdAsync(pedidoId)
                     ?? throw new MessageException("Erro ao carregar pedido.");

            return MapToDto(pedido);
        }

        public async Task<PedidoDto> FecharPedidoAsync(long pedidoId)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId)
                ?? throw new MessageException("Pedido não encontrado.");

            pedido.Fechar();

            await _pedidoRepository.UpdateAsync(pedido);

            pedido = await _pedidoRepository.GetByIdAsync(pedidoId)
                     ?? throw new MessageException("Erro ao carregar pedido.");

            return MapToDto(pedido);
        }

        public async Task<PedidoDto> CancelarPedidoAsync(long pedidoId)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId)
                ?? throw new MessageException("Pedido não encontrado.");

            pedido.Cancelar();

            await _pedidoRepository.UpdateAsync(pedido);

            pedido = await _pedidoRepository.GetByIdAsync(pedidoId)
                     ?? throw new MessageException("Erro ao carregar pedido.");

            return MapToDto(pedido);
        }

        private static PedidoDto MapToDto(Pedido pedido)
        {
            return new PedidoDto
            {
                Id = pedido.Id,
                DataRegistro = pedido.DataRegistro,
                DataAlteracao = pedido.DataAlteracao,
                StatusPedido = pedido.StatusPedido,
                Total = pedido.CalcularTotal(),
                Itens = pedido.Itens.Select(i => new PedidoItemDto
                {
                    Id = i.Id,
                    ProdutoId = i.ProdutoId,
                    ProdutoNome = i.ProdutoNome,
                    UnidadeMedida = i.UnidadeMedida,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                }).ToList()
            };
        }
    }
}
