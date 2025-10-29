

namespace ApiPedidos.Domain.Entities
{
    public interface IDataAlteracao
    {
        public DateTime? DataAlteracao { get; set; }
        public DateTime DataRegistro { get; set; }

    }
}
