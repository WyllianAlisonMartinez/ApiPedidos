using ApiPedidos.Application.Interfaces;
using ApiPedidos.Application.Services;
using ApiPedidos.Domain.Entities;
using ApiPedidos.Domain.Entities.Pedido;
using ApiPedidos.Domain.Enums;
using ApiPedidos.Domain.Exceptions;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApiPedidos.Test.Application
{
    public class PedidoServiceTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepoMock;
        private readonly Mock<IProdutoRepository> _produtoRepoMock;
        private readonly PedidoService _service;

        public PedidoServiceTests()
        {
            _pedidoRepoMock = new Mock<IPedidoRepository>();
            _produtoRepoMock = new Mock<IProdutoRepository>();
            _service = new PedidoService(_pedidoRepoMock.Object, _produtoRepoMock.Object);
        }

        private Produto NovoProdutoAtivo()
        {
            var p = new Produto("Teste", 5, 10, 100, UnidadeMedida.Unidade, true, System.DateTime.UtcNow);
            p.Id = 10;
            return p;
        }

        private Pedido NovoPedido()
        {
            var p = new Pedido();
            p.Id = 1;
            return p;
        }

        [Fact]
        public async Task CriarPedido_DeveRetornarPedidoDto()
        {
            var novo = new Pedido() { Id = 1 };

            _pedidoRepoMock.Setup(x => x.AddAsync(It.IsAny<Pedido>()))
                           .Callback<Pedido>(p => p.Id = 1)
                           .Returns(Task.CompletedTask);

            _pedidoRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(novo);

            var dto = await _service.CriarPedidoAsync();

            Assert.NotNull(dto);
            Assert.Equal(1, dto.Id);
        }

        [Fact]
        public async Task AdicionarItem_DeveAdicionar_ComSucesso()
        {
            var pedido = NovoPedido();
            var produto = NovoProdutoAtivo();

            _pedidoRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(pedido);
            _produtoRepoMock.Setup(x => x.GetByIdIncluindoInativoAsync(10)).ReturnsAsync(produto);

            var dto = await _service.AdicionarItemAsync(1, 10, 3);

            Assert.Single(dto.Itens);
            Assert.Equal(3, dto.Itens.First().Quantidade);
        }

        [Fact]
        public async Task AdicionarItem_DeveLancarException_QuandoPedidoNaoExiste()
        {
            _pedidoRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((Pedido)null);

            var ex = await Assert.ThrowsAsync<MessageException>(() =>
                _service.AdicionarItemAsync(1, 10, 1));

            Assert.Equal("Pedido não encontrado.", ex.Message);
        }

        [Fact]
        public async Task AdicionarItem_DeveLancarException_QuandoProdutoNaoExiste()
        {
            var pedido = NovoPedido();

            _pedidoRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(pedido);
            _produtoRepoMock.Setup(x => x.GetByIdIncluindoInativoAsync(10)).ReturnsAsync((Produto)null);

            var ex = await Assert.ThrowsAsync<MessageException>(() =>
                _service.AdicionarItemAsync(1, 10, 1));

            Assert.Equal("Produto não encontrado.", ex.Message);
        }

        [Fact]
        public async Task FecharPedido_DeveAtualizarStatus()
        {
            var pedido = NovoPedido();
            pedido.AdicionarItemPedido(NovoProdutoAtivo(), 1);

            _pedidoRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(pedido);

            var dto = await _service.FecharPedidoAsync(1);

            Assert.Equal(StatusPedido.Fechado, dto.StatusPedido);
        }

        [Fact]
        public async Task CancelarPedido_DeveAlterarStatus()
        {
            var pedido = NovoPedido();
            pedido.AdicionarItemPedido(NovoProdutoAtivo(), 1);

            _pedidoRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(pedido);

            var dto = await _service.CancelarPedidoAsync(1);

            Assert.Equal(StatusPedido.Cancelado, dto.StatusPedido);
        }
    }
}
