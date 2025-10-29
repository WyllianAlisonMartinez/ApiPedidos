using ApiPedidos.Domain.Enums;


namespace ApiPedidos.Application.DTOs
{
    public class PedidoItemDto
    {
        public long Id { get; set; }
        public long ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public UnidadeMedida UnidadeMedida { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal => Quantidade * PrecoUnitario;
    }
}
