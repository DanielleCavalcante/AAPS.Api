using AAPS.Api.Dtos.Senha;

namespace AAPS.Api.Services.Senhas
{
    public interface ISenhaService
    {
        Task<bool> ResetarSenha(int voluntarioId);
        Task<bool> AlterarSenhaAsync(int voluntarioId, AlterarSenhaDto alterarSenhaDto);
    }
}