using AAPS.Api.Context;
using AAPS.Api.Dtos.PontoAdocao;
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
            var pessoa = new Pessoa
            {
                Tipo = TipoPessoaEnum.PontoAdocao,
                Status = pontoAdocaoDto.Status,
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var endereco = new Endereco
            {
                Logradouro = pontoAdocaoDto.Logradouro,
                Numero = pontoAdocaoDto.Numero,
                Complemento = pontoAdocaoDto.Complemento,
                Bairro = pontoAdocaoDto.Bairro,
                Uf = pontoAdocaoDto.Uf,
                Cidade = pontoAdocaoDto.Cidade,
                Cep = pontoAdocaoDto.Cep,
                PessoaId = pessoa.Id
            };

            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            var pontoAdocao = new PontoAdocao
            {
                NomeFantasia = pontoAdocaoDto.NomeFantasia,
                Responsavel = pontoAdocaoDto.Responsavel,
                Cnpj = pontoAdocaoDto.Cnpj,
                PessoaId = pessoa.Id
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
            var query = _context.PontosAdocao
                .Include(p => p.Pessoa)
                .Where(p => p.Pessoa.Tipo == TipoPessoaEnum.PontoAdocao)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Busca))
            {
                string buscaLower = filtro.Busca.ToLower();

                query = query.Where(p =>
                    p.NomeFantasia.ToLower().Contains(buscaLower) ||
                    p.Responsavel.ToLower().Contains(buscaLower) ||
                    p.Cnpj.Contains(buscaLower)
                );
            }

            if (filtro.Status.HasValue)
            {
                query = query.Where(a => a.Pessoa.Status == filtro.Status.Value);
            }

            var pontoAdocaoDto = await query
                .Select(p => new PontoAdocaoDto
                {
                    Id = p.Id,
                    NomeFantasia = p.NomeFantasia,
                    Responsavel = p.Responsavel,
                    Cnpj = p.Cnpj,
                    Logradouro = p.Pessoa.Endereco.Logradouro,
                    Numero = p.Pessoa.Endereco.Numero,
                    Complemento = p.Pessoa.Endereco.Complemento,
                    Bairro = p.Pessoa.Endereco.Bairro,
                    Uf = p.Pessoa.Endereco.Uf,
                    Cidade = p.Pessoa.Endereco.Cidade,
                    Cep = p.Pessoa.Endereco.Cep,
                    Status = p.Pessoa.Status
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
                Logradouro = pontoAdocao.Pessoa.Endereco.Logradouro,
                Numero = pontoAdocao.Pessoa.Endereco.Numero,
                Complemento = pontoAdocao.Pessoa.Endereco.Complemento,
                Bairro = pontoAdocao.Pessoa.Endereco.Bairro,
                Uf = pontoAdocao.Pessoa.Endereco.Uf,
                Cidade = pontoAdocao.Pessoa.Endereco.Cidade,
                Cep = pontoAdocao.Pessoa.Endereco.Cep,
                Status = pontoAdocao.Pessoa.Status
            };
        }

        public async Task<IEnumerable<PontoAdocaoDto>> ObterPontosAdocaoAtivos()
        {
            var pontosAdocao = await _context.PontosAdocao
                .Include(p => p.Pessoa)
                .ThenInclude(p => p.Endereco)
                .Where(p => p.Pessoa.Status == StatusEnum.Ativo)
                .Select(p => new PontoAdocaoDto
                {
                    Id = p.Id,
                    NomeFantasia = p.NomeFantasia,
                    Responsavel = p.Responsavel,
                    Cnpj = p.Cnpj,
                    Logradouro = p.Pessoa.Endereco.Logradouro,
                    Numero = p.Pessoa.Endereco.Numero,
                    Complemento = p.Pessoa.Endereco.Complemento,
                    Bairro = p.Pessoa.Endereco.Bairro,
                    Uf = p.Pessoa.Endereco.Uf,
                    Cidade = p.Pessoa.Endereco.Cidade,
                    Cep = p.Pessoa.Endereco.Cep,
                    Status = p.Pessoa.Status
                })
                .ToListAsync();

            return pontosAdocao;
        }

        public async Task<PontoAdocaoDto> AtualizarPontoAdocao(int id, AtualizaPontoAdocaoDto pontoAdocaoDto)
        {
            var pontoAdocao = await BuscarPontoAdocaoPorId(id);

            if (pontoAdocao is null)
            {
                return null;
            }

            pontoAdocao.Pessoa.Status = pontoAdocaoDto.Status.HasValue ? pontoAdocaoDto.Status.Value : pontoAdocao.Pessoa.Status;

            pontoAdocao.NomeFantasia = string.IsNullOrEmpty(pontoAdocaoDto.NomeFantasia) ? pontoAdocao.NomeFantasia : pontoAdocaoDto.NomeFantasia;
            pontoAdocao.Responsavel = string.IsNullOrEmpty(pontoAdocaoDto.Responsavel) ? pontoAdocao.Responsavel : pontoAdocaoDto.Responsavel;
            pontoAdocao.Cnpj = string.IsNullOrEmpty(pontoAdocaoDto.Cnpj) ? pontoAdocao.Cnpj : pontoAdocaoDto.Cnpj;

            pontoAdocao.Pessoa.Endereco.Logradouro = string.IsNullOrEmpty(pontoAdocaoDto.Logradouro) ? pontoAdocao.Pessoa.Endereco.Logradouro : pontoAdocaoDto.Logradouro;
            pontoAdocao.Pessoa.Endereco.Numero = pontoAdocaoDto.Numero.HasValue ? pontoAdocaoDto.Numero.Value : pontoAdocao.Pessoa.Endereco.Numero;
            pontoAdocao.Pessoa.Endereco.Complemento = string.IsNullOrEmpty(pontoAdocaoDto.Complemento) ? pontoAdocao.Pessoa.Endereco.Complemento : pontoAdocaoDto.Complemento;
            pontoAdocao.Pessoa.Endereco.Bairro = string.IsNullOrEmpty(pontoAdocaoDto.Bairro) ? pontoAdocao.Pessoa.Endereco.Bairro : pontoAdocaoDto.Bairro;
            pontoAdocao.Pessoa.Endereco.Uf = string.IsNullOrEmpty(pontoAdocaoDto.Uf) ? pontoAdocao.Pessoa.Endereco.Uf : pontoAdocaoDto.Uf;
            pontoAdocao.Pessoa.Endereco.Cidade = string.IsNullOrEmpty(pontoAdocaoDto.Cidade) ? pontoAdocao.Pessoa.Endereco.Cidade : pontoAdocaoDto.Cidade;
            pontoAdocao.Pessoa.Endereco.Cep = string.IsNullOrEmpty(pontoAdocaoDto.Cep) ? pontoAdocao.Pessoa.Endereco.Cep : pontoAdocaoDto.Cep;

            await _context.SaveChangesAsync();

            var pontoAdocaoAtualizado = new PontoAdocaoDto
            {
                Id = pontoAdocao.Id,
                NomeFantasia = pontoAdocao.NomeFantasia,
                Responsavel = pontoAdocao.Responsavel,
                Cnpj = pontoAdocao.Cnpj,
                Logradouro = pontoAdocao.Pessoa.Endereco.Logradouro,
                Numero = pontoAdocao.Pessoa.Endereco.Numero,
                Complemento = pontoAdocao.Pessoa.Endereco.Complemento,
                Bairro = pontoAdocao.Pessoa.Endereco.Bairro,
                Uf = pontoAdocao.Pessoa.Endereco.Uf,
                Cidade = pontoAdocao.Pessoa.Endereco.Cidade,
                Cep = pontoAdocao.Pessoa.Endereco.Cep,
                Status = pontoAdocao.Pessoa.Status
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

            pontoAdocao.Pessoa.Status = StatusEnum.Inativo;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<string>> ValidarCriacaoPontoAdocao(CriarPontoAdocaoDto pontoAdocaoDto)
        {
            var erros = new List<string>();

            if (string.IsNullOrEmpty(pontoAdocaoDto.Status.ToString()))
                erros.Add("O campo 'Status' é obrigatório!");

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
            if (string.IsNullOrEmpty(pontoAdocaoDto.Cep) || pontoAdocaoDto.Cep.Length != 8)
                erros.Add("O campo 'CEP' é obrigatório e deve conter exatamente 8 dígitos!");


            var pontoAdocaoExistente = await _context.PontosAdocao
                .Include(p => p.Pessoa)
                .ThenInclude(p => p.Endereco)
                .Where(p =>
                    p.NomeFantasia == pontoAdocaoDto.NomeFantasia &&
                    p.Responsavel == pontoAdocaoDto.Responsavel &&
                    p.Cnpj == pontoAdocaoDto.Cnpj &&
                    p.Pessoa.Endereco.Logradouro == pontoAdocaoDto.Logradouro &&
                    p.Pessoa.Endereco.Numero == pontoAdocaoDto.Numero &&
                    p.Pessoa.Endereco.Complemento == pontoAdocaoDto.Complemento &&
                    p.Pessoa.Endereco.Bairro == pontoAdocaoDto.Bairro &&
                    p.Pessoa.Endereco.Uf == pontoAdocaoDto.Uf &&
                    p.Pessoa.Endereco.Cidade == pontoAdocaoDto.Cidade &&
                    p.Pessoa.Endereco.Cep == pontoAdocaoDto.Cep
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

            if (pontoAdocaoDto.Status != null && string.IsNullOrEmpty(pontoAdocaoDto.Status.ToString()))
                erros.Add("O campo 'Status' não pode ser vazio!");

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
            if (pontoAdocaoDto.Cep != null && string.IsNullOrWhiteSpace(pontoAdocaoDto.Cep) || pontoAdocaoDto.Cep.ToString().Length != 8)
                erros.Add("O campo 'CEP' não pode ter ser vazio e deve ter exatamente 8 dígitos!");

            if (pontoAdocaoDto.NomeFantasia != null && string.IsNullOrEmpty(pontoAdocaoDto.NomeFantasia))
                erros.Add("O campo 'Nome Fantasia' não pode ser vazio!");
            if (pontoAdocaoDto.Responsavel != null && string.IsNullOrEmpty(pontoAdocaoDto.Responsavel))
                erros.Add("O campo 'Responsavel' não pode ser vazio!");
            if (pontoAdocaoDto.Cnpj != null && string.IsNullOrEmpty(pontoAdocaoDto.Cnpj))
                erros.Add("O campo 'CNPJ' não pode ser vazio!");

            return erros;
        }

        #region MÉTODOS PRIVADOS

        private async Task<PontoAdocao?> BuscarPontoAdocaoPorId(int id)
        {
            return await _context.PontosAdocao
                .Include(p => p.Pessoa)
                .ThenInclude(p => p.Endereco)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        #endregion
    }
}