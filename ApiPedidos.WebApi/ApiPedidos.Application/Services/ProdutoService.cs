using ApiPedidos.Application.DTOs;
using ApiPedidos.Application.Interfaces;
using ApiPedidos.Domain.Entities;
using ApiPedidos.Domain.Exceptions;


namespace ApiPedidos.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _repository;

        public ProdutoService(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProdutoDto>> ListarAsync()
        {
            var produtos = await _repository.GetAllAsync();
            return produtos.Select(MapToDto).ToList();
        }

        public async Task<ProdutoDto?> ObterPorIdAsync(long id)
        {
            var produto = await _repository.GetByIdAsync(id);
            return produto == null ? null : MapToDto(produto);
        }

        public async Task<ProdutoDto> CriarAsync(ProdutoDto dto)
        {
            var produto = new Produto(
                dto.Nome,
                dto.PrecoCusto,
                dto.PrecoVenda,
                dto.Quantidade,
                dto.UnidadeMedida,
                isAtivo: true,
                dataRegistro: DateTime.UtcNow
            );

            await _repository.AddAsync(produto);
            dto.Id = produto.Id;
            return dto;
        }

        public async Task<ProdutoDto?> AtualizarAsync(ProdutoDto dto)
        {
            var produto = await _repository.GetByIdIncluindoInativoAsync(dto.Id);
            if (produto == null)
                return null;

           
            if (!produto.IsAtivo && !dto.IsAtivo)
                throw new MessageException($"O produto {produto.Nome} está inativo e não pode ser alterado sem reativar.");

           
            if (!produto.IsAtivo && dto.IsAtivo)
                produto.Reativar();

           
            produto.Atualizar(
                dto.Nome,
                dto.PrecoCusto,
                dto.PrecoVenda,
                dto.Quantidade,
                dto.UnidadeMedida
            );

            await _repository.UpdateAsync(produto);
            return dto;
        }




        public async Task<List<ProdutoDto>> ListarInativosAsync()
        {
            var produtos = await _repository.GetInativosAsync();
            return produtos.Select(MapToDto).ToList();
        }




        private static ProdutoDto MapToDto(Produto p)
        {
            return new ProdutoDto
            {
                Id = p.Id,
                Nome = p.Nome,
                UnidadeMedida = p.UnidadeMedida,
                Quantidade = p.Quantidade,
                PrecoVenda = p.PrecoVenda,
                PrecoCusto = p.PrecoCusto,
                IsAtivo = p.IsAtivo,
                DataRegistro = p.DataRegistro,
                DataAlteracao = p.DataAlteracao
            };
        }

        public async Task<ProdutoDto?> ObterPorIdIncluindoInativoAsync(long id)
        {
            var produto = await _repository.GetByIdIncluindoInativoAsync(id);
            return produto == null ? null : MapToDto(produto);
        }

        public async Task<bool> DeletarAsync(long id)
        {
            var produto = await _repository.GetByIdAsync(id);
            if (produto == null)
                return false;

            produto.Inativar(); 

            await _repository.UpdateAsync(produto);

            return true;
        }



    }
}
