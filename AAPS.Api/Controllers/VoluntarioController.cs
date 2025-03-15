using AAPS.Api.Dtos.Autenticacao;
using AAPS.Api.Dtos.Voluntarios;
using AAPS.Api.Services.Voluntarios;
using AAPS.Api.Services.Autenticacao;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoluntarioController : Controller
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly IVoluntarioService _voluntario;
        private readonly IAutenticacaoService _autenticacao;

        public VoluntarioController(IVoluntarioService voluntario, IAutenticacaoService autenticacao)
        {
            _voluntario = voluntario;
            _autenticacao = autenticacao;
        }

        #endregion

        [HttpPost("CriarVoluntario")]
        public async Task<IActionResult> CriarVoluntario([FromBody] CriarVoluntarioDto voluntarioDto)
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(voluntarioDto.NomeCompleto))
                erros.Add("O campo 'Nome' é obrigatório!");
            if (string.IsNullOrWhiteSpace(voluntarioDto.NomeUsuario))
                erros.Add("O campo 'Nome de Usuário' é obrigatório!");
            if (string.IsNullOrWhiteSpace(voluntarioDto.Cpf))
                erros.Add("O campo 'CPF' é obrigatório!");
            if (string.IsNullOrWhiteSpace(voluntarioDto.Email))
                erros.Add("O campo 'Email' é obrigatório!");
            if (string.IsNullOrWhiteSpace(voluntarioDto.Telefone))
                erros.Add("O campo 'Telefone' é obrigatório!");
            if (string.IsNullOrWhiteSpace(voluntarioDto.Acesso))
                erros.Add("O campo 'Acesso' é obrigatório!");
            if (string.IsNullOrWhiteSpace(voluntarioDto.Senha))
                erros.Add("O campo 'Senha' é obrigatório!");

            if (erros.Any())
            {
                return BadRequest(erros);
            }

            if (voluntarioDto.Senha != voluntarioDto.ConfirmarSenha)
            {
                return BadRequest("As senhas não conferem!");
            }

            var resultado = await _voluntario.RegistrarVoluntario(voluntarioDto);

            if (!resultado)
            {
                return StatusCode(500, "Erro ao criar o usuário.");
            }

            return Ok($"Usuário {voluntarioDto.NomeUsuario} criado com sucesso!");
        }
    }
}
