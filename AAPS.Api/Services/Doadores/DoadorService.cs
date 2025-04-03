using AAPS.Api.Context;
using AAPS.Api.Dtos.Adotante;
using AAPS.Api.Dtos.Doador;
using AAPS.Api.Models;
using AAPS.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AAPS.Api.Services.Doadores
{
    public class DoadorService : IDoadorService
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly AppDbContext _context;

        public DoadorService(AppDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<DoadorDto> CriarDoador(CriarDoadorDto doadorDto)
        {
            var doador = new Pessoa
            {
                Nome = doadorDto.Nome,
                Rg = doadorDto.Rg,
                Cpf = doadorDto.Cpf,
                Tipo = TipoPessoaEnum.Doador,
                Status = doadorDto.Status,
            };

            _context.Pessoas.Add(doador);
            await _context.SaveChangesAsync();

            var endereco = new Endereco
            {
                Logradouro = doadorDto.Logradouro,
                Numero = doadorDto.Numero,
                Complemento = doadorDto.Complemento,
                Bairro = doadorDto.Bairro,
                Uf = doadorDto.Uf,
                Cidade = doadorDto.Cidade,
                Cep = doadorDto.Cep,
                PessoaId = doador.Id
            };

            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            return new DoadorDto
            {
                Id = doador.Id,
                Nome = doadorDto.Nome,
                Rg = doadorDto.Rg,
                Cpf = doadorDto.Cpf,
                Logradouro = doadorDto.Logradouro,
                Numero = doadorDto.Numero,
                Complemento = doadorDto.Complemento,
                Bairro = doadorDto.Bairro,
                Uf = doadorDto.Uf,
                Cidade = doadorDto.Cidade,
                Cep = doadorDto.Cep,
                Status = doadorDto.Status,
            };
        }

        public async Task<IEnumerable<DoadorDto>> ObterDoadores(FiltroDoadorDto filtro)
        {
            var query = _context.Pessoas
                .Where(d => d.Tipo == TipoPessoaEnum.Doador)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Busca))
            {
                string buscaLower = filtro.Busca.ToLower();

                query = query.Where(p =>
                    p.Nome.ToLower().Contains(buscaLower) ||
                    p.Cpf.Contains(buscaLower) ||
                    p.Rg.Contains(filtro.Busca)
                );
            }

            if (filtro.Status.HasValue)
            {
                query = query.Where(d => d.Status == filtro.Status.Value);
            }

            var doadoresDto = await query
                .Select(d => new DoadorDto
                {
                    Id = d.Id,
                    Nome = d.Nome,
                    Rg = d.Rg,
                    Cpf = d.Cpf,
                    Logradouro = d.Endereco.Logradouro,
                    Numero = d.Endereco.Numero,
                    Complemento = d.Endereco.Complemento,
                    Bairro = d.Endereco.Bairro,
                    Uf = d.Endereco.Uf,
                    Cidade = d.Endereco.Cidade,
                    Cep = d.Endereco.Cep,
                    Status = d.Status,
                })
                .ToListAsync();

            return doadoresDto;
        }

        public async Task<DoadorDto?> ObterDoadorPorId(int id)
        {
            var doador = await BuscarDoadorPorId(id);

            if (doador == null)
            {
                return null;
            }

            return new DoadorDto
            {
                Id = doador.Id,
                Nome = doador.Nome,
                Rg = doador.Rg,
                Cpf = doador.Cpf,
                Logradouro = doador.Endereco.Logradouro,
                Numero = doador.Endereco.Numero,
                Complemento = doador.Endereco.Complemento,
                Bairro = doador.Endereco.Bairro,
                Uf = doador.Endereco.Uf,
                Cidade = doador.Endereco.Cidade,
                Cep = doador.Endereco.Cep,
                Status = doador.Status,
            };
        }

        public async Task<IEnumerable<DoadorDto>> ObterDoadoresAtivos()
        {
            var doadores = _context.Pessoas
                .Where(d => d.Status == StatusEnum.Ativo)
                .Select(d => new DoadorDto
                {
                    Id = d.Id,
                    Nome = d.Nome,
                    Rg = d.Rg,
                    Cpf = d.Cpf,
                    Logradouro = d.Endereco.Logradouro,
                    Numero = d.Endereco.Numero,
                    Complemento = d.Endereco.Complemento,
                    Bairro = d.Endereco.Bairro,
                    Uf = d.Endereco.Uf,
                    Cidade = d.Endereco.Cidade,
                    Cep = d.Endereco.Cep,
                    Status = d.Status,
                });

            return await doadores.ToListAsync();
        }

        public async Task<DoadorDto?> AtualizarDoador(int id, AtualizarDoadorDto doadorDto)
        {
            var doador = await BuscarDoadorPorId(id);

            if (doador == null)
            {
                return null;
            }

            doador.Nome = string.IsNullOrEmpty(doadorDto.Nome) ? doador.Nome : doadorDto.Nome;
            doador.Rg = string.IsNullOrEmpty(doadorDto.Rg) ? doador.Rg : doadorDto.Rg;
            doador.Cpf = string.IsNullOrEmpty(doadorDto.Cpf) ? doador.Cpf : doadorDto.Cpf;
            doador.Status = doadorDto.Status.HasValue ? doadorDto.Status.Value : doador.Status;

            doador.Endereco.Logradouro = string.IsNullOrEmpty(doadorDto.Logradouro) ? doador.Endereco.Logradouro : doadorDto.Logradouro;
            doador.Endereco.Numero = doadorDto.Numero.HasValue ? doadorDto.Numero.Value : doador.Endereco.Numero;
            doador.Endereco.Complemento = string.IsNullOrEmpty(doadorDto.Complemento) ? doador.Endereco.Complemento : doadorDto.Complemento;
            doador.Endereco.Bairro = string.IsNullOrEmpty(doadorDto.Bairro) ? doador.Endereco.Bairro : doadorDto.Bairro;
            doador.Endereco.Uf = string.IsNullOrEmpty(doadorDto.Uf) ? doador.Endereco.Uf : doadorDto.Uf;
            doador.Endereco.Cidade = string.IsNullOrEmpty(doadorDto.Cidade) ? doador.Endereco.Cidade : doadorDto.Cidade;
            doador.Endereco.Cep = string.IsNullOrEmpty(doadorDto.Cep) ? doador.Endereco.Cep : doadorDto.Cep;

            await _context.SaveChangesAsync();

            return new DoadorDto
            {
                Id = doador.Id,
                Nome = doador.Nome,
                Rg = doador.Rg,
                Cpf = doador.Cpf,
                Logradouro = doador.Endereco.Logradouro,
                Numero = doador.Endereco.Numero,
                Complemento = doador.Endereco.Complemento,
                Bairro = doador.Endereco.Bairro,
                Uf = doador.Endereco.Uf,
                Cidade = doador.Endereco.Cidade,
                Cep = doador.Endereco.Cep,
                Status = doador.Status,
            };
        }

        public async Task<bool> ExcluirDoador(int id)
        {
            var doador = await BuscarDoadorPorId(id);

            if (doador is null)
            {
                return false;
            }

            doador.Status = StatusEnum.Inativo;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<string>> ValidarCriacaoDoador(CriarDoadorDto doadorDto)
        {
            var erros = new List<string>();

            if (string.IsNullOrEmpty(doadorDto.Nome))
                erros.Add("O campo 'Nome' é obrigatório!");
            if (string.IsNullOrEmpty(doadorDto.Rg))
                erros.Add("O campo 'RG' é obrigatório!");
            if (string.IsNullOrEmpty(doadorDto.Cpf))
                erros.Add("O campo 'CPF' é obrigatório!");
            if (string.IsNullOrEmpty(doadorDto.Status.ToString()))
                erros.Add("O campo 'Status' é obrigatório!");

            if (string.IsNullOrEmpty(doadorDto.Logradouro))
                erros.Add("O campo 'Logradouro' é obrigatório!");
            if (string.IsNullOrEmpty(doadorDto.Numero.ToString()) || doadorDto.Numero <= 0 || string.IsNullOrWhiteSpace(doadorDto.Numero.ToString()))
                erros.Add("O campo 'Número' é obrigatório e deve ser maior que zero!");
            if (string.IsNullOrEmpty(doadorDto.Bairro))
                erros.Add("O campo 'Bairro' é obrigatório!");
            if (string.IsNullOrEmpty(doadorDto.Uf) || doadorDto.Uf.Length != 2)
                erros.Add("O campo 'UF' é obrigatório e deve ter 2 caracteres!");
            if (string.IsNullOrEmpty(doadorDto.Cidade))
                erros.Add("O campo 'Cidade' é obrigatório!");
            if (string.IsNullOrEmpty(doadorDto.Cep) || doadorDto.Cep.Length != 8)
                erros.Add("O campo 'CEP' é obrigatório e deve conter exatamente 8 dígitos!");
            if (string.IsNullOrEmpty(doadorDto.Status.ToString()))
                erros.Add("O campo 'Status' é obrigatório!");

            var doadorExistente = await _context.Pessoas
                .Where(d =>
                    d.Nome == doadorDto.Nome &&
                    d.Rg == doadorDto.Rg &&
                    d.Cpf == doadorDto.Cpf
                )
                .FirstOrDefaultAsync();

            if (doadorExistente != null)
            {
                erros.Add($"Doador já cadastrado. Código {doadorExistente.Id}");
            }

            return erros;
        }

        public List<string> ValidarAtualizacaoDoador(AtualizarDoadorDto doadorDto)
        {
            var erros = new List<string>();

            if (doadorDto.Nome != null && string.IsNullOrEmpty(doadorDto.Nome))
                erros.Add("O campo 'Nome' não pode ser vazio!");
            if (doadorDto.Rg != null && string.IsNullOrEmpty(doadorDto.Rg))
                erros.Add("O campo 'RG' não pode ser vazio!");
            if (doadorDto.Cpf != null && string.IsNullOrEmpty(doadorDto.Cpf))
                erros.Add("O campo 'CPF' não pode ser vazio!");
            if (doadorDto.Status != null && string.IsNullOrWhiteSpace(doadorDto.Status.ToString()))
                erros.Add("O campo 'Status' não pode ser vazio!");

            if (doadorDto.Logradouro != null && string.IsNullOrEmpty(doadorDto.Logradouro))
                erros.Add("O campo 'Logradouro' não pode ser vazio!");
            if (doadorDto.Numero != null && (doadorDto.Numero <= 0 || string.IsNullOrWhiteSpace(doadorDto.Numero.ToString())))
                erros.Add("O campo 'Número' é obrigatório e deve ser maior que zero!");
            if (doadorDto.Bairro != null && string.IsNullOrEmpty(doadorDto.Bairro))
                erros.Add("O campo 'Bairro' não pode ser vazio!");
            if (doadorDto.Uf != null && (string.IsNullOrEmpty(doadorDto.Uf) || doadorDto.Uf.Length != 2))
                erros.Add("O campo 'UF' é obrigatório e deve ter 2 caracteres!");
            if (doadorDto.Cidade != null && string.IsNullOrEmpty(doadorDto.Cidade))
                erros.Add("O campo 'Cidade' não pode ser vazio!");
            if (doadorDto.Cep != null && string.IsNullOrEmpty(doadorDto.Cep) || doadorDto.Cep.ToString().Length != 8)
                erros.Add("O campo 'Logradouro' não pode ser vazio!");

            return erros;
        }

        #region MÉTODOS PRIVADAS

        private async Task<Pessoa?> BuscarDoadorPorId(int id)
        {
            return await _context.Pessoas
                .Include(p => p.Endereco)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        #endregion
    }
}