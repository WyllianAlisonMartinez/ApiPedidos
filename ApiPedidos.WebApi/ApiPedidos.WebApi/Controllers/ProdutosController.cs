using ApiPedidos.Application.DTOs;
using ApiPedidos.Application.Interfaces;
using ApiPedidos.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ApiPedidos.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet("Ativos")]
        public async Task<IActionResult> GetAll()
        {
            var produtos = await _produtoService.ListarAsync();
            
            if (produtos == null)
                return NotFound(new { erro = "Nenhum produto encontrado" });

            return Ok(produtos);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var produto = await _produtoService.ObterPorIdIncluindoInativoAsync(id);

            if (produto == null)
                return NotFound(new { erro = "Produto não encontrado." });

            if (!produto.IsAtivo)
                return NotFound(new { erro = $"O produto {produto.Nome} está inativo." });

            return Ok(produto);
        }


        [HttpPost]
        public async Task<IActionResult> CriarProduto([FromBody] ProdutoDto produtoDto)
        {
            try
            {
                var produtoCriado = await _produtoService.CriarAsync(produtoDto);
                return CreatedAtAction(nameof(GetById), new { id = produtoCriado.Id }, produtoCriado);
            }
            catch (MessageException ex)
            {
                return BadRequest(new { erro = "Erro ao criar produto." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> AtualizarProduto(long id, [FromBody] ProdutoDto produtoDto)
        {
            var produto = await _produtoService.ObterPorIdIncluindoInativoAsync(id);

            if (produto == null)
                return NotFound(new { erro = "Produto não encontrado." });

            try
            {
                produtoDto.Id = id;
                var atualizado = await _produtoService.AtualizarAsync(produtoDto);
                return Ok(atualizado);
            }
            catch (MessageException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }


        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeletarProduto(long id)
        {
            try
            {
                await _produtoService.DeletarAsync(id);
                return NoContent();
            }
            catch (MessageException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpGet("Inativos")]
        public async Task<IActionResult> ListarInativos()
        {
            var produtos = await _produtoService.ListarInativosAsync();

            if(produtos == null)
                return NotFound(new { erro = "Nenhum produto encontrado." });

            return Ok(produtos);
        }

    }
}
