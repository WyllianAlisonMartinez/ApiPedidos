using ApiPedidos.Domain.Enums;


namespace ApiPedidos.Domain.Entities
{
    public class Produto : IStatus, IDataRegistro, IDataAlteracao
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda {  get; set; }
        public decimal Quantidade {  get; set; }
        public UnidadeMedida UnidadeMedida {  get; set; }
        public bool IsAtivo { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public Produto() { }

        public Produto(string nome, decimal precoCusto, decimal precoVenda, decimal quantidade, UnidadeMedida unidadeMedida, bool isAtivo, DateTime dataRegistro)
        {
            Validar(nome, precoCusto, precoVenda, quantidade);

            Nome = nome;
            PrecoCusto = precoCusto;
            PrecoVenda = precoVenda;
            Quantidade = quantidade;
            UnidadeMedida = unidadeMedida;
            IsAtivo = isAtivo;
            DataRegistro = DateTime.UtcNow;
            DataAlteracao = null;
        }

        public void Atualizar(string nome, decimal precoCusto, decimal precoVenda, decimal quantidade, UnidadeMedida undidadeMedida)
        {
            if(!IsAtivo)
                throw new InvalidOperationException("O produto está intivo, não sendo possível alterar");

            Validar(nome, precoCusto, precoVenda, quantidade);

            Nome = nome;
            PrecoCusto = precoCusto;
            PrecoVenda = precoVenda;
            Quantidade = quantidade;
            UnidadeMedida = undidadeMedida;
            DataAlteracao = DateTime.UtcNow;
        }

        public void Inativar()
        {
            if (!IsAtivo)
                throw new InvalidOperationException("O produto já está inativo");

            IsAtivo = false;
            DataAlteracao = DateTime.UtcNow;
        }

        public void AtualizarEstoque(decimal novaQuantidade)
        {
            if (novaQuantidade < 0) throw new ArgumentOutOfRangeException("A quantidade tem que ser maior que zero");

            Quantidade = novaQuantidade;
            DataAlteracao = DateTime.UtcNow;
        }

        public static void Validar(string nome, decimal precoCusto, decimal precoVenda, decimal quantidade)
        {
            if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome do produto é obrigatório.");

            if (precoCusto <= 0) throw new ArgumentException("O preco de custo deve ser maior que zero.");

            if (precoVenda <= 0) throw new ArgumentException("O preco de venda deve ser maior que zero.");

            if (precoVenda < precoCusto) throw new ArgumentException("O preco de venda não pode ser menor que o preco de custo. ");

            if (quantidade <= 0) throw new ArgumentException("A quantidade deve ser maior que zero.");
        }

        public void Reativar()
        {
            IsAtivo = true;
            DataAlteracao = DateTime.UtcNow;
        }

    }
}
