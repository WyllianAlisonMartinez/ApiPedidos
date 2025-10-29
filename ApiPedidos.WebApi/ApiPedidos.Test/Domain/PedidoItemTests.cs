using ApiPedidos.Domain.Entities.Pedido;
using ApiPedidos.Domain.Enums;
using System;
using Xunit;

namespace ApiPedidos.Test.Domain
{
    public class PedidoItemTests
    {
        [Fact]
        public void DeveCriarPedidoItemComSucesso()
        {
            var item = new PedidoItem(1, "Produto Teste", UnidadeMedida.Unidade, 2, 10);

            Assert.Equal(1, item.ProdutoId);
            Assert.Equal("Produto Teste", item.ProdutoNome);
            Assert.Equal(2, item.Quantidade);
            Assert.Equal(10, item.PrecoUnitario);
            Assert.Equal(20, item.Subtotal);
        }

        [Fact]
        public void DeveSomarQuantidadeAoAdicionarQuantidade()
        {
            var item = new PedidoItem(1, "Produto Teste", UnidadeMedida.Unidade, 1, 10);

            item.AdicionarQuantidade(3);

            Assert.Equal(4, item.Quantidade);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void DeveLancarErroSeAdicionarQuantidadeInvalida(decimal qtd)
        {
            var item = new PedidoItem(1, "Produto Teste", UnidadeMedida.Unidade, 1, 10);

            Assert.Throws<ArgumentException>(() => item.AdicionarQuantidade(qtd));
        }
    }
}
