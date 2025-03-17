using AAPS.Api.Dtos.PontosAdocao;
using AAPS.Api.Models;
using AAPS.Api.Responses;
using AAPS.Api.Services.PontosAdocao;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class PontoAdocaoController : Controller
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly IPontoAdocaoService _pontoAdocaoService;

        public PontoAdocaoController(IPontoAdocaoService pontoAdocaoService)
        {
            _pontoAdocaoService = pontoAdocaoService;
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> CriarPontoAdocao([FromBody] CriarPontoAdocaoDto pontoAdocaoDto)
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(pontoAdocaoDto.NomeFantasia))
                erros.Add("O campo 'nome fantasia' é obrigatório!");
            if (string.IsNullOrWhiteSpace(pontoAdocaoDto.Responsavel))
                erros.Add("O campo 'nome fantasia' é obrigatório!");
            if (string.IsNullOrWhiteSpace(pontoAdocaoDto.Cnpj))
                erros.Add("O campo 'nome fantasia' é obrigatório!");
            if (string.IsNullOrWhiteSpace(pontoAdocaoDto.Logradouro))
                erros.Add("O campo 'nome fantasia' é obrigatório!");
            if (string.IsNullOrWhiteSpace(pontoAdocaoDto.Numero.ToString()))
                erros.Add("O campo 'nome fantasia' é obrigatório!");
            if (string.IsNullOrWhiteSpace(pontoAdocaoDto.Complemento))
                erros.Add("O campo 'nome fantasia' é obrigatório!");
            if (string.IsNullOrWhiteSpace(pontoAdocaoDto.Bairro))
                erros.Add("O campo 'nome fantasia' é obrigatório!");
            if (string.IsNullOrWhiteSpace(pontoAdocaoDto.Uf))
                erros.Add("O campo 'nome fantasia' é obrigatório!");
            if (string.IsNullOrWhiteSpace(pontoAdocaoDto.Cidade))
                erros.Add("O campo 'nome fantasia' é obrigatório!");
            if (string.IsNullOrWhiteSpace(pontoAdocaoDto.Cep.ToString()))
                erros.Add("O campo 'nome fantasia' é obrigatório!");

            if (erros.Count > 0)
            {
                return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao registrar ponto de adoção!"));
            }

            var pontoAdocao = await _pontoAdocaoService.CriarPontoAdocao(pontoAdocaoDto);

            if (pontoAdocao is null)
            {
                return StatusCode(500, ApiResponse<object>.ErroResponse(new List<string> { "Erro criar o ponto de adoção." }));
            }

            return Ok(ApiResponse<object>.SucessoResponse(pontoAdocao, "Ponto de Adoção criado com sucesso!"));
        }

        [HttpGet]
        public async Task<ActionResult<IAsyncEnumerable<PontoAdocao>>> ObterPontosAdocao()
        {
            var pontosAdocaoDto = await _pontoAdocaoService.ObterPontosAdocao();

            if (pontosAdocaoDto is null)
            {
                return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Nenhum ponto de adoção foi encontrado." }));
            }

            return Ok(ApiResponse<object>.SucessoResponse(pontosAdocaoDto));
        }

        [HttpGet]
        public async Task<ActionResult<IAsyncEnumerable<PontoAdocao>>> ObterPontoAdocaoPorNome([FromQuery] string nome)
        {
            var pontosAdocao = await _pontoAdocaoService.ObterPontoAdocaoPorNome(nome);

            if (pontosAdocao is null)
            {
                return BadRequest(ApiResponse<object>.ErroResponse(new List<string> { "Erro ao obter pontos de adoção." }));
            }

            return Ok(ApiResponse<object>.SucessoResponse(pontosAdocao));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> AtualizarPontoAdocao(int id, [FromBody] AtualizaPontoAdocaoDto pontoAdocaoDto)
        {
            // TODO: validar campos required

            var pontoAdocao = await _pontoAdocaoService.AtualizarPontoAdocao(id, pontoAdocaoDto);

            if (pontoAdocao is null)
            {
                return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Ponto de adoção de id = {id} não encontrado." }));
            }

            return Ok(ApiResponse<object>.SucessoResponse(pontoAdocao, $"Ponto de Adoção de id = {id} foi atualizado com sucesso!"));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> ExcluirPontoAdocao(int id)
        {
            bool pontoAdocao = await _pontoAdocaoService.ExcluirPontoAdocao(id);

            if (!pontoAdocao)
            {
                return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Ponto de adoção de id = {id} não encontrado." }));
            }

            return Ok(ApiResponse<object>.SucessoResponse($"Ponto de Adoção de id = {id} foi excluído com sucesso!"));
        }
    }
}
