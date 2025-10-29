using ApiPedidos.Application.DTOs;


namespace ApiPedidos.Application.Interfaces
{
    public interface IProdutoService
    {
        Task<List<ProdutoDto>> ListarAsync();
        Task<ProdutoDto?> ObterPorIdAsync(long id);
        Task<ProdutoDto> CriarAsync(ProdutoDto produtoDto);
        Task<ProdutoDto?> AtualizarAsync(ProdutoDto produtoDto);
        Task<bool> DeletarAsync(long id);
        Task<List<ProdutoDto>> ListarInativosAsync();
        Task<ProdutoDto?> ObterPorIdIncluindoInativoAsync(long id);





    }
}
