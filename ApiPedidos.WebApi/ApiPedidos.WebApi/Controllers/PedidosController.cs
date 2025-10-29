using ApiPedidos.Application.Interfaces;
using ApiPedidos.Domain.Enums;
using ApiPedidos.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ApiPedidos.WebApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido()
        {
            var pedido = await _pedidoService.CriarPedidoAsync();
            return Ok(pedido);
        }

        [HttpGet]
        public async Task<IActionResult> ListarPedidos()
        {
            var pedidos = await _pedidoService.ListarPedidosAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> ObterPorId(long id)
        {
            var pedido = await _pedidoService.ObterPorIdAsync(id);
            if (pedido == null)
                return NotFound(new { erro = "Pedido não encontrado." });

            return Ok(pedido);
        }

        [HttpPost("{pedidoId:long}/produtos/{produtoId:long}")]
        public async Task<IActionResult> AdicionarItem(long pedidoId, long produtoId, [FromQuery] decimal quantidade)
        {
            try
            {
                var pedidoAtualizado = await _pedidoService.AdicionarItemAsync(pedidoId, produtoId, quantidade);
                return Ok(pedidoAtualizado);
            }
            catch (MessageException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpDelete("{pedidoId:long}/produtos/{produtoId:long}")]
        public async Task<IActionResult> RemoverItem(long pedidoId, long produtoId)
        {
            try
            {
                var pedidoAtualizado = await _pedidoService.RemoverItemAsync(pedidoId, produtoId);
                return Ok(pedidoAtualizado);
            }
            catch (MessageException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpPost("{pedidoId:long}/fechar")]
        public async Task<IActionResult> FecharPedido(long pedidoId)
        {
            try
            {
                var pedidoAtualizado = await _pedidoService.FecharPedidoAsync(pedidoId);
                return Ok(pedidoAtualizado);
            }
            catch (MessageException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpPost("{pedidoId:long}/cancelar")]
        public async Task<IActionResult> CancelarPedido(long pedidoId)
        {
            try
            {
                var pedidoAtualizado = await _pedidoService.CancelarPedidoAsync(pedidoId);
                return Ok(pedidoAtualizado);
            }
            catch (MessageException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpGet("paginado")]
        public async Task<IActionResult> ListarPedidosPaginado([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _pedidoService.ListarPedidosPaginadoAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> ListarPorStatus(StatusPedido status)
        {
            var pedidos = await _pedidoService.ListarPorStatusAsync(status);

            if (!pedidos.Any())
                return NotFound(new { erro = "Nenhum pedido encontrado." });

            return Ok(pedidos);
        }

    }
}
