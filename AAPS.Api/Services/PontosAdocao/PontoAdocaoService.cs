using AAPS.Api.Context;
using AAPS.Api.Dtos.PontoAdocao;
using AAPS.Api.Dtos.PontosAdocao;
using AAPS.Api.Models;
using AAPS.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AAPS.Api.Services.PontosAdocao
{
    public class PontoAdocaoService : IPontoAdocaoService
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly AppDbContext _context;

        public PontoAdocaoService(AppDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<PontoAdocaoDto> CriarPontoAdocao(CriarPontoAdocaoDto pontoAdocaoDto)
        {
            var pontoAdocao = new PontoAdocao
            {
                NomeFantasia = pontoAdocaoDto.NomeFantasia,
                Responsavel = pontoAdocaoDto.Responsavel,
                Cnpj = pontoAdocaoDto.Cnpj,
                Logradouro = pontoAdocaoDto.Logradouro,
                Numero = pontoAdocaoDto.Numero,
                Complemento = pontoAdocaoDto.Complemento,
                Bairro = pontoAdocaoDto.Bairro,
                Uf = pontoAdocaoDto.Uf,
                Cidade = pontoAdocaoDto.Cidade,
                Cep = pontoAdocaoDto.Cep,
                Status = pontoAdocaoDto.Status
            };

            _context.PontosAdocao.Add(pontoAdocao);
            await _context.SaveChangesAsync();

            return new PontoAdocaoDto
            {
                NomeFantasia = pontoAdocaoDto.NomeFantasia,
                Responsavel = pontoAdocaoDto.Responsavel,
                Cnpj = pontoAdocaoDto.Cnpj,
                Logradouro = pontoAdocaoDto.Logradouro,
                Numero = pontoAdocaoDto.Numero,
                Complemento = pontoAdocaoDto.Complemento,
                Bairro = pontoAdocaoDto.Bairro,
                Uf = pontoAdocaoDto.Uf,
                Cidade = pontoAdocaoDto.Cidade,
                Cep = pontoAdocaoDto.Cep,
                Status = pontoAdocaoDto.Status
            };
        }

        public async Task<IEnumerable<PontoAdocaoDto>> ObterPontosAdocao(FiltroPontoAdocaoDto filtro)
        {
            var query = _context.PontosAdocao.AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Busca))
            {
                // busca por NomeFantasia, Responsavel e Cnpj
                query = query.Where(p =>
                    p.NomeFantasia.Contains(filtro.Busca.ToLower()) ||
                    p.Responsavel.Contains(filtro.Busca.ToLower()) ||
                    p.Cnpj.Contains(filtro.Busca)
                );
            }

            if (filtro.Status.HasValue)
            {
                query = query.Where(a => a.Status == filtro.Status.Value);
            }

            var pontoAdocaoDto = await query
                .Select(p => new PontoAdocaoDto
                {
                    Id = p.Id,
                    NomeFantasia = p.NomeFantasia,
                    Responsavel = p.Responsavel,
                    Cnpj = p.Cnpj,
                    Logradouro = p.Logradouro,
                    Numero = p.Numero,
                    Complemento = p.Complemento,
                    Bairro = p.Bairro,
                    Uf = p.Uf,
                    Cidade = p.Cidade,
                    Cep = p.Cep,
                    Status = p.Status
                })
                .ToListAsync();

            return pontoAdocaoDto;
        }

        public async Task<PontoAdocaoDto?> ObterPontoAdocaoPorId(int id)
        {
            var pontoAdocao = await BuscarPontoAdocaoPorId(id);

            if (pontoAdocao == null)
            {
                return null;
            }

            return new PontoAdocaoDto
            {
                Id = pontoAdocao.Id,
                NomeFantasia = pontoAdocao.NomeFantasia,
                Responsavel = pontoAdocao.Responsavel,
                Cnpj = pontoAdocao.Cnpj,
                Logradouro = pontoAdocao.Logradouro,
                Numero = pontoAdocao.Numero,
                Complemento = pontoAdocao.Complemento,
                Bairro = pontoAdocao.Bairro,
                Uf = pontoAdocao.Uf,
                Cidade = pontoAdocao.Cidade,
                Cep = pontoAdocao.Cep,
                Status = pontoAdocao.Status
            };
        }

        public async Task<IEnumerable<PontoAdocaoDto>> ObterPontosAdocaoAtivos()
        {
            var pontosAdocao = _context.PontosAdocao
                .Where(p => p.Status == StatusEnum.Ativo)
                .Select(p => new PontoAdocaoDto
                {
                    Id = p.Id,
                    NomeFantasia = p.NomeFantasia,
                    Responsavel = p.Responsavel,
                    Cnpj = p.Cnpj,
                    Logradouro = p.Logradouro,
                    Numero = p.Numero,
                    Complemento = p.Complemento,
                    Bairro = p.Bairro,
                    Uf = p.Uf,
                    Cidade = p.Cidade,
                    Cep = p.Cep,
                    Status = p.Status
                });

            return await pontosAdocao.ToListAsync();
        }

        public async Task<PontoAdocaoDto> AtualizarPontoAdocao(int id, AtualizaPontoAdocaoDto pontoAdocaoDto)
        {
            var pontoAdocao = await BuscarPontoAdocaoPorId(id);

            if (pontoAdocao is null)
            {
                return null;
            }

            pontoAdocao.NomeFantasia = string.IsNullOrEmpty(pontoAdocaoDto.NomeFantasia) ? pontoAdocao.NomeFantasia : pontoAdocaoDto.NomeFantasia;
            pontoAdocao.Responsavel = string.IsNullOrEmpty(pontoAdocaoDto.Responsavel) ? pontoAdocao.Responsavel : pontoAdocaoDto.Responsavel;
            pontoAdocao.Cnpj = string.IsNullOrEmpty(pontoAdocaoDto.Cnpj) ? pontoAdocao.Cnpj : pontoAdocaoDto.Cnpj;
            pontoAdocao.Logradouro = string.IsNullOrEmpty(pontoAdocaoDto.Logradouro) ? pontoAdocao.Logradouro : pontoAdocaoDto.Logradouro;
            pontoAdocao.Numero = pontoAdocaoDto.Numero.HasValue ? pontoAdocaoDto.Numero.Value : pontoAdocao.Numero;
            pontoAdocao.Complemento = string.IsNullOrEmpty(pontoAdocaoDto.Complemento) ? pontoAdocao.Complemento : pontoAdocaoDto.Complemento;
            pontoAdocao.Bairro = string.IsNullOrEmpty(pontoAdocaoDto.Bairro) ? pontoAdocao.Bairro : pontoAdocaoDto.Bairro;
            pontoAdocao.Uf = string.IsNullOrEmpty(pontoAdocaoDto.Uf) ? pontoAdocao.Uf : pontoAdocaoDto.Uf;
            pontoAdocao.Cidade = string.IsNullOrEmpty(pontoAdocaoDto.Cidade) ? pontoAdocao.Cidade : pontoAdocaoDto.Cidade;
            pontoAdocao.Cep = pontoAdocaoDto.Cep.HasValue ? pontoAdocaoDto.Cep.Value : pontoAdocao.Cep;
            pontoAdocao.Status = pontoAdocaoDto.Status.HasValue ? pontoAdocaoDto.Status.Value : pontoAdocao.Status;

            await _context.SaveChangesAsync();

            var pontoAdocaoAtualizado = new PontoAdocaoDto
            {
                Id = pontoAdocao.Id,
                NomeFantasia = pontoAdocao.NomeFantasia,
                Responsavel = pontoAdocao.Responsavel,
                Cnpj = pontoAdocao.Cnpj,
                Logradouro = pontoAdocao.Logradouro,
                Numero = pontoAdocao.Numero,
                Complemento = pontoAdocao.Complemento,
                Bairro = pontoAdocao.Bairro,
                Uf = pontoAdocao.Uf,
                Cidade = pontoAdocao.Cidade,
                Cep = pontoAdocao.Cep,
                Status = pontoAdocao.Status
            };

            return pontoAdocaoAtualizado;
        }

        public async Task<bool> ExcluirPontoAdocao(int id)
        {
            var pontoAdocao = await BuscarPontoAdocaoPorId(id);

            if (pontoAdocao == null)
            {
                return false;
            }

            //_context.PontosAdocao.Remove(pontoAdocao);

            pontoAdocao.Status = StatusEnum.Inativo;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<string>> ValidarCriacaoPontoAdocao(CriarPontoAdocaoDto pontoAdocaoDto)
        {
            var erros = new List<string>();

            if (string.IsNullOrEmpty(pontoAdocaoDto.NomeFantasia))
                erros.Add("O campo 'Nome Fantasia' é obrigatório!");
            if (string.IsNullOrEmpty(pontoAdocaoDto.Responsavel))
                erros.Add("O campo 'Responsavel' é obrigatório!");
            if (string.IsNullOrEmpty(pontoAdocaoDto.Cnpj))
                erros.Add("O campo 'CNPJ' é obrigatório!");
            if (string.IsNullOrEmpty(pontoAdocaoDto.Logradouro))
                erros.Add("O campo 'Logradouro' é obrigatório!");
            if (string.IsNullOrEmpty(pontoAdocaoDto.Numero.ToString()) || pontoAdocaoDto.Numero <= 0)
                erros.Add("O campo 'Número' é obrigatório e deve ser maior que zero!");
            if (string.IsNullOrEmpty(pontoAdocaoDto.Bairro))
                erros.Add("O campo 'Bairro' é obrigatório!");
            if (string.IsNullOrEmpty(pontoAdocaoDto.Uf) || pontoAdocaoDto.Uf.Length != 2)
                erros.Add("O campo 'UF' é obrigatório!");
            if (string.IsNullOrEmpty(pontoAdocaoDto.Cidade))
                erros.Add("O campo 'Cidade' é obrigatório!");
            if (pontoAdocaoDto.Cep <= 0 || pontoAdocaoDto.Cep.ToString().Length != 8)
                erros.Add("O campo 'CEP' é obrigatório e deve ter exatamente 8 dígitos!");
            if (string.IsNullOrEmpty(pontoAdocaoDto.Status.ToString()))
                erros.Add("O campo 'Status' é obrigatório!");

            var pontoAdocaoExistente = await _context.PontosAdocao
                .Where(p =>
                    p.NomeFantasia == pontoAdocaoDto.NomeFantasia &&
                    p.Responsavel == pontoAdocaoDto.Responsavel &&
                    p.Cnpj == pontoAdocaoDto.Cnpj &&
                    p.Logradouro == pontoAdocaoDto.Logradouro &&
                    p.Numero == pontoAdocaoDto.Numero &&
                    p.Complemento == pontoAdocaoDto.Complemento &&
                    p.Bairro == pontoAdocaoDto.Bairro &&
                    p.Uf == pontoAdocaoDto.Uf &&
                    p.Cidade == pontoAdocaoDto.Cidade &&
                    p.Cep == pontoAdocaoDto.Cep
                //p.Status == pontoAdocaoDto.Status
                )
                .FirstOrDefaultAsync();

            if (pontoAdocaoExistente != null)
            {
                erros.Add($"Ponto de adoção já cadastrado. Código {pontoAdocaoExistente.Id}");
            }

            return erros;
        }

        public List<string> ValidarAtualizacaoPontoAdocao(AtualizaPontoAdocaoDto pontoAdocaoDto)
        {
            var erros = new List<string>();

            if (pontoAdocaoDto.NomeFantasia != null && string.IsNullOrEmpty(pontoAdocaoDto.NomeFantasia))
                erros.Add("O campo 'Nome Fantasia' não pode ser vazio!");
            if (pontoAdocaoDto.Responsavel != null && string.IsNullOrEmpty(pontoAdocaoDto.Responsavel))
                erros.Add("O campo 'Responsavel' não pode ser vazio!");
            if (pontoAdocaoDto.Cnpj != null && string.IsNullOrEmpty(pontoAdocaoDto.Cnpj))
                erros.Add("O campo 'CNPJ' não pode ser vazio!");
            if (pontoAdocaoDto.Logradouro != null && string.IsNullOrEmpty(pontoAdocaoDto.Logradouro))
                erros.Add("O campo 'Logradouro' não pode ser vazio!");
            if (pontoAdocaoDto.Numero != null && pontoAdocaoDto.Numero <= 0)
                erros.Add("O campo 'Número' é obrigatório e deve ser maior que zero!");
            if (pontoAdocaoDto.Bairro != null && string.IsNullOrEmpty(pontoAdocaoDto.Bairro))
                erros.Add("O campo 'Bairro' não pode ser vazio!");
            if (pontoAdocaoDto.Uf != null && (string.IsNullOrEmpty(pontoAdocaoDto.Uf) || pontoAdocaoDto.Uf.Length != 2))
                erros.Add("O campo 'UF' não pode ser vazio!");
            if (pontoAdocaoDto.Cidade != null && string.IsNullOrEmpty(pontoAdocaoDto.Cidade))
                erros.Add("O campo 'Cidade' não pode ser vazio!");
            if (pontoAdocaoDto.Cep != null && (pontoAdocaoDto.Cep <= 0 || pontoAdocaoDto.Cep.ToString().Length != 8))
                erros.Add("O campo 'CEP' é obrigatório e deve ter exatamente 8 dígitos!");
            if (pontoAdocaoDto.Status != null && string.IsNullOrEmpty(pontoAdocaoDto.Status.ToString()))
                erros.Add("O campo 'Status' não pode ser vazio!");

            return erros;
        }

        #region MÉTODOS PRIVADOS

        private async Task<PontoAdocao?> BuscarPontoAdocaoPorId(int id)
        {
            var pontoAdocao = await _context.PontosAdocao.FindAsync(id);
            return pontoAdocao;
        }

        #endregion
    }
}