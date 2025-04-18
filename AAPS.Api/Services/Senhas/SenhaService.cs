using AAPS.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AAPS.Api.Services.Senhas
{
    public class SenhaService : ISenhaService
    {
        #region ATRIBUTOS E CONSTRUTOR
        private readonly UserManager<Voluntario> _userManager;

        public SenhaService(UserManager<Voluntario> userManager)
        {
            _userManager = userManager;
        }

        #endregion

        public async Task<bool> ResetarSenha(int voluntarioId)
        {
            var voluntario = await _userManager.Users.FirstOrDefaultAsync(v => v.Id == voluntarioId);

            if (voluntario == null)
            {
                return false;
            }

            string novaSenha = "Aaps@123";

            var removeResult = await _userManager.RemovePasswordAsync(voluntario);
            if (!removeResult.Succeeded)
                return false;

            var addResult = await _userManager.AddPasswordAsync(voluntario, novaSenha);
            return addResult.Succeeded;
        }

        //public async Task<bool> AlterarSenhaAsync(int voluntarioId, string novaSenha)
        //{
        //    var voluntario = await ObterVoluntarioPorId(voluntarioId);
        //    if (voluntario == null)
        //        return false;

        //    // Validação de senha (ex: complexidade mínima, etc.)
        //    if (string.IsNullOrWhiteSpace(novaSenha) || novaSenha.Length < 6)
        //    {
        //        // Senha deve ter pelo menos 6 caracteres
        //        return false;
        //    }

        //    // Atualizando a senha (assumindo que a senha é salva de forma segura)
        //    voluntario.PasswordHash = novaSenha;  // A senha deve ser armazenada de forma segura, como um hash

        //    await _context.SaveChangesAsync();

        //    return true;
        //}

        //// senha com whatsapp
        //public async Task<string> GerarCodigoRecuperacao(int voluntarioId)
        //{
        //    // Gera um código aleatório de 6 dígitos
        //    var codigo = new Random().Next(100000, 999999).ToString();

        //    // Salva o código no banco
        //    await SalvarCodigoRecuperacaoAsync(voluntarioId, codigo);

        //    return codigo;
        //}

        //public async Task SalvarCodigoRecuperacaoAsync(int voluntarioId, string codigo)
        //{
        //    var expiracao = DateTime.UtcNow.AddMinutes(10); // Código expira em 10 minutos

        //    var codigoRecuperacao = new CodigoRecuperacao
        //    {
        //        VoluntarioId = voluntarioId,
        //        Codigo = codigo,
        //        ExpiraEm = expiracao,
        //        Usado = false
        //    };

        //    _context.CodigosRecuperacao.Add(codigoRecuperacao);
        //    await _context.SaveChangesAsync();
        //}

        //public async Task<bool> ValidarCodigoRecuperacao(int voluntarioId, string codigo)
        //{
        //    var codigoValido = await _context.CodigosRecuperacao
        //        .Where(c => c.VoluntarioId == voluntarioId && c.Codigo == codigo && c.ExpiraEm > DateTime.UtcNow && !c.Usado)
        //        .FirstOrDefaultAsync();

        //    if (codigoValido == null)
        //        return false;

        //    // Marcar o código como usado
        //    codigoValido.Usado = true;
        //    await _context.SaveChangesAsync();

        //    return true;
        //}
    }
}
