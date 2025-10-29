using ApiPedidos.Domain.Entities;
using ApiPedidos.Domain.Entities.Pedido;
using ApiPedidos.Domain.Enums;
using ApiPedidos.Domain.Exceptions;
using System;
using System.Linq;
using Xunit;

namespace ApiPedidos.Test.Domain
{
    public class PedidoTests
    {
        private Produto CriarProdutoAtivo(long id = 1) =>
            new Produto("Produto A", 5, 10, 2, UnidadeMedida.Unidade, true, DateTime.UtcNow);


        private Produto CriarProdutoInativo(long id = 1) =>
            new Produto("Produto B", 5, 10, 2, UnidadeMedida.Unidade, false, DateTime.UtcNow);

        [Fact]
        public void IniciarPedidoComoAberto()
        {
            var pedido = new Pedido();

            Assert.Equal(StatusPedido.Aberto, pedido.StatusPedido);
        }

        [Fact]
        public void AdicionarItemQuandoProdutoAtivo()
        {
            var pedido = new Pedido();

            var produto = new Produto("Teste", 5, 10, 10, UnidadeMedida.Unidade, true, DateTime.UtcNow)
            {
                Id = 1 
            };

            pedido.AdicionarItemPedido(produto, 1);

            Assert.Single(pedido.Itens);
            Assert.Equal(1, pedido.Itens.First().ProdutoId);
        }




        [Fact]
        public void SomarQuantidadeSeItemJaExiste()
        {
            var pedido = new Pedido();
            var produto = CriarProdutoAtivo();
            produto.Id = 1;

            pedido.AdicionarItemPedido(produto, 1);
            pedido.AdicionarItemPedido(produto, 2);

            Assert.Equal(3, pedido.Itens.First().Quantidade);
        }

        [Fact]
        public void LancarErroSeProdutoInativo()
        {
            var pedido = new Pedido();
            var produto = CriarProdutoInativo();

            var ex = Assert.Throws<MessageException>(() =>
                pedido.AdicionarItemPedido(produto, 1));

            Assert.Contains("inativo", ex.Message);
        }

        [Fact]
        public void LancarErroSeQuantidadeMenorOuIgualZero()
        {
            var pedido = new Pedido();
            var produto = CriarProdutoAtivo();

            Assert.Throws<MessageException>(() => pedido.AdicionarItemPedido(produto, 0));
        }

        [Fact]
        public void FecharPedidoComItens()
        {
            var pedido = new Pedido();
            var produto = CriarProdutoAtivo();
            produto.Id = 1;


            pedido.AdicionarItemPedido(produto, 1);
            pedido.Fechar();

            Assert.Equal(StatusPedido.Fechado, pedido.StatusPedido);
        }

        [Fact]
        public void LancarErroAoFecharPedidoSemItens()
        {
            var pedido = new Pedido();

            var ex = Assert.Throws<MessageException>(() => pedido.Fechar());
            Assert.Equal("Não é possível fechar um pedido sem itens.", ex.Message);
        }

        [Fact]
        public void CancelarPedidoAberto()
        {
            var pedido = new Pedido();
            var produto = CriarProdutoAtivo();
            produto.Id = 1;

            pedido.AdicionarItemPedido(produto, 1);
            pedido.Cancelar();

            Assert.Equal(StatusPedido.Cancelado, pedido.StatusPedido);
        }

        [Fact]
        public void LancarErroSeCancelarPedidoFechado()
        {
            var pedido = new Pedido();
            var produto = new Produto("Teste", 5, 10, 10, UnidadeMedida.Unidade, true, DateTime.UtcNow);
            produto.Id = 1;

            pedido.AdicionarItemPedido(produto, 1);
            pedido.Fechar();

          
            Assert.Throws<MessageException>(() => pedido.Cancelar());

        }

        [Fact]
        public void RemoverItem()
        {
            var pedido = new Pedido();
            var produto = CriarProdutoAtivo(1);
            produto.Id = 1;

            pedido.AdicionarItemPedido(produto, 1);
            pedido.RemoverItem(produto.Id);

            Assert.Empty(pedido.Itens);
        }


        [Fact]
        public void CalcularTotalCorretamente()
        {
            var pedido = new Pedido();
            var produto = CriarProdutoAtivo(1);
            produto.Id = 1;

            pedido.AdicionarItemPedido(produto, 1);

            Assert.Equal(10, pedido.CalcularTotal());
        }
    }
}
