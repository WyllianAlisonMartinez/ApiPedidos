using ApiPedidos.Domain.Enums;


namespace ApiPedidos.Application.DTOs
{
    public class PedidoDto
    {
        public long Id { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public StatusPedido StatusPedido { get; set; }
        public decimal Total { get; set; }
        public List<PedidoItemDto> Itens { get; set; } = new();
    }
}
