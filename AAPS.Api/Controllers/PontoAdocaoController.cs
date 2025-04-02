using AAPS.Api.Dtos.Animais;
using AAPS.Api.Dtos.PontosAdocao;
using AAPS.Api.Models;
using AAPS.Api.Responses;
using AAPS.Api.Services.Animais;
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
            var erros = await _pontoAdocaoService.ValidarCriacaoPontoAdocao(pontoAdocaoDto);

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
        public async Task<ActionResult<IAsyncEnumerable<PontoAdocao>>> ObterPontosAdocao([FromQuery] FiltroPontoAdocaoDto filtro)
        {
            var pontosAdocao = await _pontoAdocaoService.ObterPontosAdocao(filtro);

            if (pontosAdocao is null || !pontosAdocao.Any())
            {
                return NotFound(ApiResponse<object>.ErroResponse(new List<string> { "Nenhum ponto de adoção foi encontrado." }));
            }

            return Ok(ApiResponse<object>.SucessoResponse(pontosAdocao));
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
            var erros = _pontoAdocaoService.ValidarAtualizacaoPontoAdocao(pontoAdocaoDto);

            if (erros.Count > 0)
            {
                return BadRequest(ApiResponse<object>.ErroResponse(erros, "Erro ao atualizar ponto de adoção!"));
            }

            var pontoAdocao = await _pontoAdocaoService.AtualizarPontoAdocao(id, pontoAdocaoDto);

            if (pontoAdocao is null)
            {
                return NotFound(ApiResponse<object>.ErroResponse(new List<string> { $"Ponto de adoção de id = {id} não encontrado." }));
            }

            return Ok(ApiResponse<object>.SucessoResponse(pontoAdocao, $"Ponto de Adoção de id = {id} foi atualizado com sucesso!"));
        }

        [HttpPut("{id:int}")]
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