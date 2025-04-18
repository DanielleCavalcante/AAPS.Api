using AAPS.Api.Context;
using AAPS.Api.Dtos.Adotante;
using AAPS.Api.Models;
using AAPS.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AAPS.Api.Services.Adotantes
{
    public class AdotanteService : IAdotanteService
    {
        #region ATRIBUTOS E CONSTRUTOR

        private readonly AppDbContext _context;

        public AdotanteService(AppDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<AdotanteDto> CriarAdotante(CriarAdotanteDto adotanteDto)
        {
            var pessoa = new Pessoa
            {
                Nome = adotanteDto.Nome,
                Rg = adotanteDto.Rg,
                Cpf = adotanteDto.Cpf,
                Tipo = TipoPessoaEnum.Adotante,
                Status = adotanteDto.Status,
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var telefones = adotanteDto.Telefones
                .Select(t => new Telefone
                {
                    NumeroTelefone = t,
                    PessoaId = pessoa.Id
                }).ToList();

            _context.Telefones.AddRange(telefones);
            await _context.SaveChangesAsync();

            var endereco = new Endereco
            {
                Logradouro = adotanteDto.Logradouro,
                Numero = adotanteDto.Numero,
                Complemento = adotanteDto.Complemento,
                Bairro = adotanteDto.Bairro,
                Uf = adotanteDto.Uf,
                Cidade = adotanteDto.Cidade,
                Cep = adotanteDto.Cep,
                SituacaoEndereco = adotanteDto.SituacaoEndereco,
                PessoaId = pessoa.Id
            };

            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            var adotante = new Adotante
            {
                LocalTrabalho = adotanteDto.LocalTrabalho,
                Facebook = adotanteDto.Facebook,
                Instagram = adotanteDto.Instagram,
                Bloqueio = adotanteDto.Bloqueio,
                PessoaId = pessoa.Id
            };

            _context.Adotantes.Add(adotante);
            await _context.SaveChangesAsync();

            return new AdotanteDto
            {
                Id = adotante.Id,
                Nome = pessoa.Nome,
                Rg = pessoa.Rg,
                Cpf = pessoa.Cpf,
                Status = pessoa.Status,
                Telefones = telefones.Select(t => t.NumeroTelefone).ToList(),
                LocalTrabalho = adotante.LocalTrabalho,
                Facebook = adotante.Facebook,
                Instagram = adotante.Instagram,
                Bloqueio = adotante.Bloqueio,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Complemento = endereco.Complemento,
                Bairro = endereco.Bairro,
                Uf = endereco.Uf,
                Cidade = endereco.Cidade,
                Cep = endereco.Cep,
                SituacaoEndereco = endereco.SituacaoEndereco
            };
        }

        public async Task<IEnumerable<AdotanteDto>> ObterAdotantes(FiltroAdotanteDto filtro)
        {
            var query = _context.Adotantes
                .Include(a => a.Pessoa)
                    .ThenInclude(a => a.Telefones)
                .Include(a => a.Pessoa.Endereco)
                .Where(a => a.Pessoa.Tipo == TipoPessoaEnum.Adotante)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Busca))
            {
                string buscaLower = filtro.Busca.ToLower();

                query = query.Where(a =>
                    a.Pessoa.Nome.ToLower().Contains(buscaLower) ||
                    a.Pessoa.Cpf.Contains(buscaLower) ||
                    a.Pessoa.Rg.Contains(filtro.Busca)
                );
            }

            if (filtro.Status.HasValue)
            {
                query = query.Where(a => a.Pessoa.Status == filtro.Status.Value);
            }

            if (filtro.Bloqueio.HasValue)
            {
                query = query.Where(a => a.Bloqueio == filtro.Bloqueio.Value);
            }

            var adotantesDto = await query
                .Select(a => new AdotanteDto
                {
                    Id = a.Id,
                    Nome = a.Pessoa.Nome,
                    Rg = a.Pessoa.Rg,
                    Cpf = a.Pessoa.Cpf,
                    Status = a.Pessoa.Status,
                    Telefones = a.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList(),
                    LocalTrabalho = a.LocalTrabalho,
                    Facebook = a.Facebook,
                    Instagram = a.Instagram,
                    Bloqueio = a.Bloqueio,
                    Logradouro = a.Pessoa.Endereco.Logradouro,
                    Numero = a.Pessoa.Endereco.Numero,
                    Complemento = a.Pessoa.Endereco.Complemento,
                    Bairro = a.Pessoa.Endereco.Bairro,
                    Uf = a.Pessoa.Endereco.Uf,
                    Cidade = a.Pessoa.Endereco.Cidade,
                    Cep = a.Pessoa.Endereco.Cep,
                    SituacaoEndereco = a.Pessoa.Endereco.SituacaoEndereco,
                })
                .ToListAsync();

            return adotantesDto;
        }

        public async Task<AdotanteDto?> ObterAdotantePorId(int id)
        {
            var adotante = await BuscarAdotantePorId(id);

            if (adotante == null)
            {
                return null;
            }

            return new AdotanteDto
            {
                Id = adotante.Id,
                Nome = adotante.Pessoa.Nome,
                Rg = adotante.Pessoa.Rg,
                Cpf = adotante.Pessoa.Cpf,
                Status = adotante.Pessoa.Status,
                Telefones = adotante.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList(),
                LocalTrabalho = adotante.LocalTrabalho,
                Facebook = adotante.Facebook,
                Instagram = adotante.Instagram,
                Bloqueio = adotante.Bloqueio,
                Logradouro = adotante.Pessoa.Endereco.Logradouro,
                Numero = adotante.Pessoa.Endereco.Numero,
                Complemento = adotante.Pessoa.Endereco.Complemento,
                Bairro = adotante.Pessoa.Endereco.Bairro,
                Uf = adotante.Pessoa.Endereco.Uf,
                Cidade = adotante.Pessoa.Endereco.Cidade,
                Cep = adotante.Pessoa.Endereco.Cep,
                SituacaoEndereco = adotante.Pessoa.Endereco.SituacaoEndereco,
            };
        }

        public async Task<IEnumerable<AdotanteDto>> ObterAdotantesAtivos()
        {
            var adotantes = await _context.Adotantes
                .Include(a => a.Pessoa)
                    .ThenInclude(p => p.Endereco)
                .Include(a => a.Pessoa)
                    .ThenInclude(p => p.Telefones)
                .Where(a => a.Pessoa.Status == StatusEnum.Ativo && a.Pessoa.Tipo == TipoPessoaEnum.Adotante)
                .Select(a => new AdotanteDto
                {
                    Id = a.Id,
                    Nome = a.Pessoa.Nome,
                    Rg = a.Pessoa.Rg,
                    Cpf = a.Pessoa.Cpf,
                    Status = a.Pessoa.Status,
                    Telefones = a.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList(),
                    LocalTrabalho = a.LocalTrabalho,
                    Facebook = a.Facebook,
                    Instagram = a.Instagram,
                    Bloqueio = a.Bloqueio,
                    Logradouro = a.Pessoa.Endereco.Logradouro,
                    Numero = a.Pessoa.Endereco.Numero,
                    Complemento = a.Pessoa.Endereco.Complemento,
                    Bairro = a.Pessoa.Endereco.Bairro,
                    Uf = a.Pessoa.Endereco.Uf,
                    Cidade = a.Pessoa.Endereco.Cidade,
                    Cep = a.Pessoa.Endereco.Cep,
                    SituacaoEndereco = a.Pessoa.Endereco.SituacaoEndereco,
                })
                .ToListAsync();

            return adotantes;
        }

        public async Task<AdotanteDto?> AtualizarAdotante(int id, AtualizarAdotanteDto adotanteDto)
        {
            var adotante = await BuscarAdotantePorId(id);

            if (adotante == null)
            {
                return null;
            }

            adotante.Pessoa.Nome = string.IsNullOrEmpty(adotanteDto.Nome) ? adotante.Pessoa.Nome : adotanteDto.Nome;
            adotante.Pessoa.Rg = string.IsNullOrEmpty(adotanteDto.Rg) ? adotante.Pessoa.Rg : adotanteDto.Rg;
            adotante.Pessoa.Cpf = string.IsNullOrEmpty(adotanteDto.Cpf) ? adotante.Pessoa.Cpf : adotanteDto.Cpf;
            adotante.Pessoa.Status = adotanteDto.Status.HasValue ? adotanteDto.Status.Value : adotante.Pessoa.Status;

            adotante.Pessoa.Endereco.Logradouro = string.IsNullOrEmpty(adotanteDto.Logradouro) ? adotante.Pessoa.Endereco.Logradouro : adotanteDto.Logradouro;
            adotante.Pessoa.Endereco.Numero = adotanteDto.Numero.HasValue ? adotanteDto.Numero.Value : adotante.Pessoa.Endereco.Numero;
            adotante.Pessoa.Endereco.Complemento = string.IsNullOrEmpty(adotanteDto.Complemento) ? adotante.Pessoa.Endereco.Complemento : adotanteDto.Complemento;
            adotante.Pessoa.Endereco.Bairro = string.IsNullOrEmpty(adotanteDto.Bairro) ? adotante.Pessoa.Endereco.Bairro : adotanteDto.Bairro;
            adotante.Pessoa.Endereco.Uf = string.IsNullOrEmpty(adotanteDto.Uf) ? adotante.Pessoa.Endereco.Uf : adotanteDto.Uf;
            adotante.Pessoa.Endereco.Cidade = string.IsNullOrEmpty(adotanteDto.Cidade) ? adotante.Pessoa.Endereco.Cidade : adotanteDto.Cidade;
            adotante.Pessoa.Endereco.Cep = string.IsNullOrEmpty(adotanteDto.Cep) ? adotante.Pessoa.Endereco.Cep : adotanteDto.Cep;
            adotante.Pessoa.Endereco.SituacaoEndereco = string.IsNullOrEmpty(adotanteDto.SituacaoEndereco) ? adotante.Pessoa.Endereco.SituacaoEndereco : adotanteDto.SituacaoEndereco;

            adotante.LocalTrabalho = string.IsNullOrEmpty(adotanteDto.LocalTrabalho) ? adotante.LocalTrabalho : adotanteDto.LocalTrabalho;
            adotante.Facebook = string.IsNullOrEmpty(adotanteDto.Facebook) ? adotante.Facebook : adotanteDto.Facebook;
            adotante.Instagram = string.IsNullOrEmpty(adotanteDto.Instagram) ? adotante.Instagram : adotanteDto.Instagram;
            adotante.Bloqueio = adotanteDto.Bloqueio.HasValue ? adotanteDto.Bloqueio.Value : adotante.Bloqueio;

            if (adotanteDto.Telefones != null)
            {
                var telefonesAtuais = adotante.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList();

                var telefonesMantidos = adotante.Pessoa.Telefones
                    .Where(t => adotanteDto.Telefones.Contains(t.NumeroTelefone))
                    .ToList();

                var telefonesParaRemover = adotante.Pessoa.Telefones
                    .Where(t => !adotanteDto.Telefones.Contains(t.NumeroTelefone))
                    .ToList();

                var telefonesParaAdicionar = adotanteDto.Telefones
                    .Where(t => !telefonesAtuais.Contains(t))
                    .ToList();

                _context.RemoveRange(telefonesParaRemover);

                foreach (var novoTelefone in telefonesParaAdicionar)
                {
                    adotante.Pessoa.Telefones.Add(new Telefone
                    {
                        NumeroTelefone = novoTelefone
                    });
                }
            }

            await _context.SaveChangesAsync();

            return new AdotanteDto
            {
                Id = adotante.Id,
                Nome = adotante.Pessoa.Nome,
                Rg = adotante.Pessoa.Rg,
                Cpf = adotante.Pessoa.Cpf,
                Status = adotante.Pessoa.Status,
                Telefones = adotante.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList(),
                LocalTrabalho = adotante.LocalTrabalho,
                Facebook = adotante.Facebook,
                Instagram = adotante.Instagram,
                Bloqueio = adotante.Bloqueio,
                Logradouro = adotante.Pessoa.Endereco.Logradouro,
                Numero = adotante.Pessoa.Endereco.Numero,
                Complemento = adotante.Pessoa.Endereco.Complemento,
                Bairro = adotante.Pessoa.Endereco.Bairro,
                Uf = adotante.Pessoa.Endereco.Uf,
                Cidade = adotante.Pessoa.Endereco.Cidade,
                Cep = adotante.Pessoa.Endereco.Cep,
                SituacaoEndereco = adotante.Pessoa.Endereco.SituacaoEndereco,
            };
        }

        public async Task<bool> ExcluirAdotante(int id)
        {
            var adotante = await BuscarAdotantePorId(id);

            if (adotante == null)
            {
                return false;
            }

            adotante.Pessoa.Status = StatusEnum.Inativo;

            //_context.Adotantes.Remove(adotante);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<string>> ValidarCriacaoAdotante(CriarAdotanteDto adotanteDto)
        {
            var erros = new List<string>();

            if (string.IsNullOrEmpty(adotanteDto.Nome))
                erros.Add("O campo 'Nome' é obrigatório!");
            if (string.IsNullOrEmpty(adotanteDto.Rg))
                erros.Add("O campo 'RG' é obrigatório!");
            if (string.IsNullOrEmpty(adotanteDto.Cpf))
                erros.Add("O campo 'CPF' é obrigatório!");
            if (string.IsNullOrEmpty(adotanteDto.Status.ToString()) || !Enum.IsDefined(typeof(StatusEnum), adotanteDto.Status))
                erros.Add("O campo 'Status' é obrigatório!");

            if (string.IsNullOrEmpty(adotanteDto.Logradouro))
                erros.Add("O campo 'Logradouro' é obrigatório!");
            if (string.IsNullOrEmpty(adotanteDto.Numero.ToString()) || adotanteDto.Numero <= 0)
                erros.Add("O campo 'Número' é obrigatório e deve ser maior que zero!");
            if (string.IsNullOrEmpty(adotanteDto.Bairro))
                erros.Add("O campo 'Bairro' é obrigatório!");
            if (string.IsNullOrEmpty(adotanteDto.Uf) || adotanteDto.Uf.Length != 2)
                erros.Add("O campo 'UF' é obrigatório e deve ter 2 caracteres!");
            if (string.IsNullOrEmpty(adotanteDto.Cidade))
                erros.Add("O campo 'Cidade' é obrigatório!");
            if (string.IsNullOrEmpty(adotanteDto.Cep) || adotanteDto.Cep.Length != 8)
                erros.Add("O campo 'CEP' é obrigatório e deve conter exatamente 8 dígitos!");
            if (string.IsNullOrEmpty(adotanteDto.SituacaoEndereco))
                erros.Add("O campo 'Situacao de Endereco' é obrigatório!");

            if (string.IsNullOrEmpty(adotanteDto.LocalTrabalho))
                erros.Add("O campo 'Local de Trabalho' é obrigatório!");
            if (string.IsNullOrEmpty(adotanteDto.Facebook))
                erros.Add("O campo 'Facebook' é obrigatório!");
            if (string.IsNullOrEmpty(adotanteDto.Instagram))
                erros.Add("O campo 'Instagram' é obrigatório!");
            if (string.IsNullOrEmpty(adotanteDto.Bloqueio.ToString()) || !Enum.IsDefined(typeof(BloqueioEnum), adotanteDto.Bloqueio))
                erros.Add("O campo 'Bloqueado' é obrigatório!");

            if (adotanteDto.Telefones == null || adotanteDto.Telefones.Count < 2)
                erros.Add("É necessário informar pelo menos dois telefones!");

            var adotanteExistente = await _context.Adotantes
                .Include(a => a.Pessoa)
                .Where(a =>
                    a.Pessoa.Nome == adotanteDto.Nome &&
                    a.Pessoa.Rg == adotanteDto.Rg &&
                    a.Pessoa.Cpf == adotanteDto.Cpf
                //a.LocalTrabalho == adotanteDto.LocalTrabalho &&
                //a.Facebook == adotanteDto.Facebook &&
                //a.Instagram == adotanteDto.Instagram
                )
                .FirstOrDefaultAsync();

            if (adotanteExistente != null)
            {
                erros.Add($"Adotante já cadastrado. Código {adotanteExistente.Id}");
            }

            return erros;
        }

        public List<string> ValidarAtualizacaoAdotante(AtualizarAdotanteDto adotanteDto)
        {
            var erros = new List<string>();

            if (adotanteDto.Nome != null && string.IsNullOrWhiteSpace(adotanteDto.Nome))
                erros.Add("O campo 'Nome' não pode ter ser vazio!");
            if (adotanteDto.Rg != null && string.IsNullOrWhiteSpace(adotanteDto.Rg))
                erros.Add("O campo 'Rg' não pode ter ser vazio!");
            if (adotanteDto.Cpf != null && string.IsNullOrWhiteSpace(adotanteDto.Cpf))
                erros.Add("O campo 'Cpf' não pode ter ser vazio!");
            if (adotanteDto.Status != null && string.IsNullOrWhiteSpace(adotanteDto.Status.ToString()))
                erros.Add("O campo 'Status' não pode ter ser vazio!");

            if (adotanteDto.Logradouro != null && string.IsNullOrWhiteSpace(adotanteDto.Logradouro))
                erros.Add("O campo 'Logradouro' não pode ter ser vazio!");
            if (adotanteDto.Numero != null && adotanteDto.Numero <= 0)
                erros.Add("O campo 'Número' não pode ter ser vazio e deve ser maior que zero!");
            if (adotanteDto.Bairro != null && string.IsNullOrWhiteSpace(adotanteDto.Bairro))
                erros.Add("O campo 'Bairro' não pode ter ser vazio!");
            if (adotanteDto.Uf != null && (adotanteDto.Uf.Length != 2 || string.IsNullOrWhiteSpace(adotanteDto.Uf)))
                erros.Add("O campo 'UF' não pode ter ser vazio e deve ter 2 caracteres!");
            if (adotanteDto.Cidade != null && string.IsNullOrWhiteSpace(adotanteDto.Cidade))
                erros.Add("O campo 'Cidade' não pode ter ser vazio!");
            if (adotanteDto.Cep != null && (string.IsNullOrWhiteSpace(adotanteDto.Cep) || adotanteDto.Cep.ToString().Length != 8))
                erros.Add("O campo 'CEP' não pode ter ser vazio e deve ter exatamente 8 dígitos!");
            if (adotanteDto.SituacaoEndereco != null && string.IsNullOrWhiteSpace(adotanteDto.SituacaoEndereco))
                erros.Add("O campo 'Situacao de Endereco' não pode ter ser vazio!");

            if (adotanteDto.LocalTrabalho != null && string.IsNullOrWhiteSpace(adotanteDto.LocalTrabalho))
                erros.Add("O campo 'Local de Trabalho' não pode ter ser vazio!");
            if (adotanteDto.Facebook != null && string.IsNullOrWhiteSpace(adotanteDto.Facebook))
                erros.Add("O campo 'Facebook' não pode ter ser vazio!");
            if (adotanteDto.Instagram != null && string.IsNullOrWhiteSpace(adotanteDto.Instagram))
                erros.Add("O campo 'Instagram' não pode ter ser vazio!");
            if (adotanteDto.Bloqueio != null && string.IsNullOrWhiteSpace(adotanteDto.Bloqueio.ToString()))
                erros.Add("O campo 'Bloqueio' não pode ter ser vazio!");

            if (adotanteDto.Telefones != null && adotanteDto.Telefones.Count < 2)
                erros.Add("É necessário informar pelo menos dois telefones!");

            return erros;
        }

        //public async Task<IEnumerable<Adotante>> ObterAdotantePorNome(string nome)
        //{
        //    return await BuscarAdotantePorNome(nome).ToListAsync();
        //}

        #region MÉTODOS PRIVADOS

        private async Task<Adotante?> BuscarAdotantePorId(int id)
        {
            return await _context.Adotantes
                .Include(a => a.Pessoa)
                    .ThenInclude(p => p.Endereco)
                .Include(a => a.Pessoa)
                    .ThenInclude(p => p.Telefones)
                .Where(a => a.Pessoa.Tipo == TipoPessoaEnum.Adotante)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        //private IQueryable<Adotante> BuscarAdotantePorNome(string nome)
        //{
        //    return _context.Adotantes.Where(a => a.Nome.Contains(nome));
        //}

        #endregion
    }
}