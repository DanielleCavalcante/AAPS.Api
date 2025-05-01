using AAPS.Api.Dtos.Senha;
using AAPS.Api.Dtos.Voluntario;
using AAPS.Api.Models;
using AAPS.Api.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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

            var token = await _userManager.GeneratePasswordResetTokenAsync(voluntario);
            var resultado = await _userManager.ResetPasswordAsync(voluntario, token, novaSenha);

            return resultado.Succeeded;
        }

        public async Task<bool> AlterarSenhaAsync(int voluntarioId, AlterarSenhaDto alterarSenhaDto)
        {
            var voluntario = await _userManager.Users.FirstOrDefaultAsync(v => v.Id == voluntarioId);

            if (voluntario == null)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(voluntario);
            var resultado = await _userManager.ResetPasswordAsync(voluntario, token, alterarSenhaDto.NovaSenha);

            return resultado.Succeeded;
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
