using ApiPedidos.Domain.Enums;


namespace ApiPedidos.Application.DTOs
{
    public class ProdutoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal Quantidade { get; set; }
        public UnidadeMedida UnidadeMedida { get; set; }
        public bool IsAtivo { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime? DataAlteracao { get; set; }


    }
}
