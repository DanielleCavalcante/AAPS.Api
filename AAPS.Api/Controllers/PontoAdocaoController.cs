using AAPS.Api.Dtos.PontoAdocao;
using AAPS.Api.Models;
using AAPS.Api.Services.PontosAdocao;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AAPS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PontoAdocaoController : Controller
    {
        private readonly IPontoAdocaoService _pontoAdocaoService;

        public PontoAdocaoController(IPontoAdocaoService pontoAdocaoService)
        {
            _pontoAdocaoService = pontoAdocaoService;
        }

        [HttpGet]
        [Route("ObterTodosPontosAdocao")]
        public async Task<ActionResult<IAsyncEnumerable<PontoAdocao>>> ObterPontosAdocao()
        {
            var pontosAdocaoDto = await _pontoAdocaoService.ObterPontosAdocao();
            return Ok(pontosAdocaoDto);
        }

        [HttpGet]
        [Route("ObterPontosAdocaoPorNome")]
        public async Task<ActionResult<IAsyncEnumerable<PontoAdocao>>> ObterPontoAdocaoPorNome(string nome)
        {
            var pontosAdocao = await _pontoAdocaoService.ObterPontoAdocaoPorNome(nome);
            return Ok(pontosAdocao);
        }

        [HttpPost]
        [Route("Criar")]
        public async Task CriarPontoAdocao(PontoAdocaoDto pontoAdocaoDto)
        {
            await _pontoAdocaoService.CriarPontoAdocao(pontoAdocaoDto);
        }

        [HttpPut]
        [Route("Atualizar/{id:int}")]
        public async Task AtualizarPontoAdocao(int id, PontoAdocaoDto pontoAdocaoDto)
        {
            await _pontoAdocaoService.AtualizarPontoAdocao(id, pontoAdocaoDto);
        }

        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<ActionResult> ExcluirPontoAdocao(int id)
        {
            await _pontoAdocaoService.ExcluirPontoAdocao(id);
            return Ok($"Ponto de Adoção de id = {id} foi excluído com sucesso!");
        }
    }
}
