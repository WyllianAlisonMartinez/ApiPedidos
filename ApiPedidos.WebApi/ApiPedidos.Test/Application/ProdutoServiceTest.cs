using ApiPedidos.Application.DTOs;
using ApiPedidos.Application.Interfaces;
using ApiPedidos.Application.Services;
using ApiPedidos.Domain.Entities;
using ApiPedidos.Domain.Enums;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ApiPedidos.Test.Application
{
    public class ProdutoServiceTest
    {
        private readonly Mock<IProdutoRepository> _repoMock;
        private readonly ProdutoService _service;

        public ProdutoServiceTest()
        {
            _repoMock = new Mock<IProdutoRepository>();
            _service = new ProdutoService(_repoMock.Object);
        }

        private Produto NovoProdutoAtivo()
        {
            var p = new Produto("Produto Teste", 5, 10, 20, UnidadeMedida.Unidade, true, DateTime.UtcNow);
            p.Id = 1;
            return p;
        }

        [Fact]
        public async Task CriarProduto_DeveRetornarDtoComId()
        {
            var dto = new ProdutoDto
            {
                Nome = "Novo",
                PrecoCusto = 5,
                PrecoVenda = 10,
                Quantidade = 15,
                UnidadeMedida = UnidadeMedida.Unidade
            };

            _repoMock.Setup(x => x.AddAsync(It.IsAny<Produto>()))
                     .Callback<Produto>(p => p.Id = 10)
                     .Returns(Task.CompletedTask);

            var resultado = await _service.CriarAsync(dto);

            Assert.Equal(10, resultado.Id);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarProduto()
        {
            var produto = NovoProdutoAtivo();
            _repoMock.Setup(x => x.GetByIdIncluindoInativoAsync(1)).ReturnsAsync(produto);

            var dto = await _service.ObterPorIdIncluindoInativoAsync(1);

            Assert.NotNull(dto);
            Assert.Equal(1, dto.Id);
        }

        [Fact]
        public async Task AtualizarProduto_DeveAlterarDados()
        {
            // Arrange
            var produtoInicial = new Produto("Teste", 5, 10, 10, UnidadeMedida.Unidade, true, DateTime.UtcNow);
            produtoInicial.Id = 1;

            var dto = new ProdutoDto
            {
                Id = 1,
                Nome = "Alterado",
                PrecoCusto = 6,
                PrecoVenda = 12,
                Quantidade = 20,
                UnidadeMedida = UnidadeMedida.Unidade
            };

            _repoMock.Setup(x => x.GetByIdIncluindoInativoAsync(1))
                     .ReturnsAsync(produtoInicial);

            _repoMock.Setup(x => x.UpdateAsync(It.IsAny<Produto>()))
                     .Returns(Task.CompletedTask);

            
            var atualizado = await _service.AtualizarAsync(dto);

            
            Assert.NotNull(atualizado);
            Assert.Equal("Alterado", atualizado.Nome);
            Assert.Equal(20, atualizado.Quantidade);
        }


        [Fact]
        public async Task Deletar_DeveRetornarTrue()
        {
            
            var produto = new Produto("Teste", 5, 10, 10, UnidadeMedida.Unidade, true, DateTime.UtcNow);
            produto.Id = 1;

            _repoMock.Setup(x => x.GetByIdAsync(1))
                     .ReturnsAsync(produto);

            _repoMock.Setup(x => x.UpdateAsync(It.IsAny<Produto>()))
                     .Returns(Task.CompletedTask);



            var resultado = await _service.DeletarAsync(1);

            
            Assert.True(resultado);
            Assert.False(produto.IsAtivo); 
        }




        [Fact]
        public async Task ListarAtivos_DeveRetornarItens()
        {
            _repoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(new[]
            {
                NovoProdutoAtivo()
            }.ToList());

            var lista = await _service.ListarAsync();

            Assert.Single(lista);
        }

    }
}
