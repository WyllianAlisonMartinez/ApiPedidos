using ApiPedidos.Domain.Enums;
using ApiPedidos.Domain.Exceptions;

namespace ApiPedidos.Domain.Entities.Pedido
{
    public class Pedido : IDataRegistro, IDataAlteracao
    {
        public long Id { get; set; }
        public DateTime DataRegistro { get; set; } = DateTime.Now;
        public DateTime? DataAlteracao { get; set; }

        public StatusPedido StatusPedido { get; set; } = StatusPedido.Aberto;

        private readonly List<PedidoItem> _itens = new();
        public IReadOnlyCollection<PedidoItem> Itens => _itens.AsReadOnly();


        public Pedido() { }

        private void PedidoAberto()
        {
            if (StatusPedido == StatusPedido.Fechado)
                throw new MessageException("Pedido fechado não pode ser alterado.");

            if (StatusPedido == StatusPedido.Cancelado)
                throw new MessageException("Pedido cancelado não pode ser alterado");
        }

        public void AdicionarItemPedido(Produto produto, decimal quantidade, decimal? precoUnitarioOverride = null)
        {
            PedidoAberto();

            if (produto is null)
                throw new ArgumentNullException(nameof(produto));

            if (!produto.IsAtivo)
                throw new MessageException($"O produto {produto.Nome} está inativo.");

            if (quantidade <= 0) throw new MessageException("Quantidade deve ser maior que zero.");

            var precoUnitario = precoUnitarioOverride ?? produto.PrecoVenda;
            if (precoUnitario <= 0) throw new MessageException("Preço unitário deve ser maior que zero.");

            var existente = _itens.FirstOrDefault(i =>
                i.ProdutoId == produto.Id && i.UnidadeMedida == produto.UnidadeMedida);

            if (existente is null)
            {
                _itens.Add(new PedidoItem(
                    produtoId: produto.Id,
                    produtoNome: produto.Nome,
                    unidade: produto.UnidadeMedida,
                    quantidade: quantidade,
                    precoUnitario: precoUnitario
                ));
            }
            else
            {
                existente.AdicionarQuantidade(quantidade);
            }

            DataAlteracao = DateTime.UtcNow;
        }
        public void RemoverItem(long produtoId)
        {
            if (StatusPedido == StatusPedido.Fechado)
                throw new MessageException("Não é possível remover itens de pedido já fechado.");

            if (StatusPedido == StatusPedido.Cancelado)
                throw new MessageException("Não é possível remover itens de pedido cancelado.");

            var item = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);
            if (item == null)
                throw new MessageException("item não encontrado no pedido.");

            _itens.Remove(item);

            DataAlteracao = DateTime.UtcNow;
        }


        public void Fechar()
        {
            if (!_itens.Any())
                throw new MessageException("Não é possível fechar um pedido sem itens.");
            StatusPedido = StatusPedido.Fechado;
        }

        public decimal CalcularTotal() => _itens.Sum(i => i.Subtotal);

        public void Cancelar()
{
    if (StatusPedido == StatusPedido.Fechado)
        throw new MessageException("Não é possível cancelar um pedido já fechado.");

    if (StatusPedido == StatusPedido.Cancelado)
        throw new MessageException("O pedido já está cancelado.");

    StatusPedido = StatusPedido.Cancelado;
    DataAlteracao = DateTime.UtcNow;
}

 
     }
}
