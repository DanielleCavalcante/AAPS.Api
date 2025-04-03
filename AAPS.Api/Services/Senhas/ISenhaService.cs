namespace AAPS.Api.Services.Senhas
{
    public interface ISenhaService
    {
        Task<bool> ResetarSenha(int voluntarioId);

        //Task<bool> AlterarSenhaAsync(int voluntarioId, string novaSenha);
    }
}
