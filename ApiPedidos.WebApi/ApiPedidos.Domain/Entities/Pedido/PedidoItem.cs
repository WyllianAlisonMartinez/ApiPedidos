using ApiPedidos.Domain.Enums;


namespace ApiPedidos.Domain.Entities.Pedido
{
    public class PedidoItem
    {
        public long Id { get; private set; }
        public long PedidoId { get; private set; }
        public long ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; } = string.Empty;
        public UnidadeMedida UnidadeMedida { get; private set; }  
        public decimal Quantidade { get; private set; }
        public decimal PrecoUnitario { get; private set; }
        public decimal Subtotal => Quantidade * PrecoUnitario;

        protected PedidoItem() { }

        public PedidoItem(long produtoId, string produtoNome, UnidadeMedida unidade, decimal quantidade, decimal precoUnitario)
        {
            if (produtoId <= 0) throw new ArgumentException("Produto inválido.");
            if (string.IsNullOrWhiteSpace(produtoNome)) throw new ArgumentException("Nome do produto é obrigatório.");
            if (quantidade <= 0) throw new ArgumentException("Quantidade deve ser maior que 0.");
            if (precoUnitario <= 0) throw new ArgumentException("Preço unitário deve ser maior que zero.");

            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            UnidadeMedida = unidade;
            Quantidade = quantidade;
            PrecoUnitario = precoUnitario;
        }

        public void AdicionarQuantidade(decimal adicional)
        {
            if (adicional <= 0) throw new ArgumentException("Quantidade adicional deve ser maior que zero.");
            Quantidade += adicional;
        }
    }
}
