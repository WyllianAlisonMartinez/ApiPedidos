using ApiPedidos.Domain.Entities;
using ApiPedidos.Domain.Enums;
using System;
using Xunit;

namespace ApiPedidos.Test.Domain
{
    public class ProdutoTest
    {
        [Fact]
        public void DeveCriarProdutoAtivoCorretamente()
        {
            var produto = new Produto("Arroz", 5, 10, 20, UnidadeMedida.Quilograma, true, DateTime.UtcNow);

            Assert.True(produto.IsAtivo);
            Assert.Equal("Arroz", produto.Nome);
        }

        [Fact]
        public void DeveInativarProdutoComSucesso()
        {
            var produto = new Produto("Feijão", 5, 10, 20, UnidadeMedida.Quilograma, true, DateTime.UtcNow);

            produto.Inativar();

            Assert.False(produto.IsAtivo);
            Assert.NotNull(produto.DataAlteracao);
        }

        [Fact]
        public void DeveLancarExcecaoAoInativarProdutoJaInativo()
        {
            var produto = new Produto("Açúcar", 5, 10, 20, UnidadeMedida.Unidade, true, DateTime.UtcNow);
            produto.Inativar();

            Assert.Throws<InvalidOperationException>(() => produto.Inativar());
        }

        [Fact]
        public void DeveReativarProdutoComSucesso()
        {
            var produto = new Produto("Sal", 5, 10, 20, UnidadeMedida.Unidade, true, DateTime.UtcNow);
            produto.Inativar();

            produto.Reativar();

            Assert.True(produto.IsAtivo);
            Assert.NotNull(produto.DataAlteracao);
        }

        [Fact]
        public void DeveLancarExcecaoAoAtualizarProdutoInativo()
        {
            var produto = new Produto("Óleo", 5, 10, 20, UnidadeMedida.Litro, true, DateTime.UtcNow);
            produto.Inativar();

            Assert.Throws<InvalidOperationException>(() =>
                produto.Atualizar("Óleo 1L", 5, 12, 10, UnidadeMedida.Litro));
        }
    }
}
