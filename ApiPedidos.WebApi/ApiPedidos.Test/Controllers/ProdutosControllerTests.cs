using ApiPedidos.Application.DTOs;
using ApiPedidos.Application.Interfaces;
using ApiPedidos.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace ApiPedidos.Test.Controllers
{
    public class ProdutosControllerTests
    {
        private readonly Mock<IProdutoService> _serviceMock;
        private readonly ProdutosController _controller;

        public ProdutosControllerTests()
        {
            _serviceMock = new Mock<IProdutoService>();
            _controller = new ProdutosController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetById_DeveRetornar_NotFound_QuandoProdutoNaoExistir()
        {
            _serviceMock.Setup(s => s.ObterPorIdIncluindoInativoAsync(1))
                .ReturnsAsync((ProdutoDto)null);

            var resultado = await _controller.GetById(1);

            Assert.IsType<NotFoundObjectResult>(resultado);
        }

        [Fact]
        public async Task GetById_DeveRetornar_NotFound_ComMensagemDeInativo()
        {
            var produto = new ProdutoDto { Id = 1, Nome = "Produto X", IsAtivo = false };

            _serviceMock.Setup(s => s.ObterPorIdIncluindoInativoAsync(1))
                .ReturnsAsync(produto);

            var resultado = await _controller.GetById(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(resultado);
            Assert.Contains("está inativo", notFound.Value.ToString());
        }

        [Fact]
        public async Task GetById_DeveRetornar_OK_QuandoProdutoAtivo()
        {
            var produto = new ProdutoDto { Id = 2, Nome = "Produto A", IsAtivo = true };

            _serviceMock.Setup(s => s.ObterPorIdIncluindoInativoAsync(2))
                .ReturnsAsync(produto);

            var resultado = await _controller.GetById(2);

            Assert.IsType<OkObjectResult>(resultado);
        }
    }
}
