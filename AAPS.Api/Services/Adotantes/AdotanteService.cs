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
                Celular = adotanteDto.Celular,
                Contato = adotanteDto.Contato,
                ResponsavelContato = adotanteDto.ResponsavelContato,
                Logradouro = adotanteDto.Logradouro,
                Numero = adotanteDto.Numero,
                Complemento = adotanteDto.Complemento,
                Bairro = adotanteDto.Bairro,
                Uf = adotanteDto.Uf,
                Cidade = adotanteDto.Cidade,
                Cep = adotanteDto.Cep,
                SituacaoEndereco = adotanteDto.SituacaoEndereco,
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            //var telefones = adotanteDto.Telefones
            //    .Select(t => new Telefone
            //    {
            //        NumeroTelefone = t,
            //        PessoaId = pessoa.Id
            //    }).ToList();

            //_context.Telefones.AddRange(telefones);
            //await _context.SaveChangesAsync();

            //var endereco = new Endereco
            //{
            //    Logradouro = adotanteDto.Logradouro,
            //    Numero = adotanteDto.Numero,
            //    Complemento = adotanteDto.Complemento,
            //    Bairro = adotanteDto.Bairro,
            //    Uf = adotanteDto.Uf,
            //    Cidade = adotanteDto.Cidade,
            //    Cep = adotanteDto.Cep,
            //    SituacaoEndereco = adotanteDto.SituacaoEndereco,
            //    PessoaId = pessoa.Id
            //};

            //_context.Enderecos.Add(endereco);
            //await _context.SaveChangesAsync();

            var adotante = new Adotante
            {
                LocalTrabalho = adotanteDto.LocalTrabalho,
                Facebook = adotanteDto.Facebook,
                Instagram = adotanteDto.Instagram,
                Bloqueio = adotanteDto.Bloqueio,
                ObservacaoBloqueio = adotanteDto.ObservacaoBloqueio,
                Email = adotanteDto.Email,
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
                //Telefones = telefones.Select(t => t.NumeroTelefone).ToList(),
                Celular = pessoa.Celular,
                Contato = pessoa.Contato,
                ResponsavelContato = pessoa.Contato,
                LocalTrabalho = adotante.LocalTrabalho,
                Facebook = adotante.Facebook,
                Instagram = adotante.Instagram,
                Bloqueio = adotante.Bloqueio,
                ObservacaoBloqueio = adotante.ObservacaoBloqueio,
                Email = adotante.Email,
                Logradouro = pessoa.Logradouro,
                Numero = pessoa.Numero,
                Complemento = pessoa.Complemento,
                Bairro = pessoa.Bairro,
                Uf = pessoa.Uf,
                Cidade = pessoa.Cidade,
                Cep = pessoa.Cep,
                SituacaoEndereco = pessoa.SituacaoEndereco
            };
        }

        public async Task<IEnumerable<AdotanteDto>> ObterAdotantes(FiltroAdotanteDto filtro)
        {
            var query = _context.Adotantes
                .Include(a => a.Pessoa)
                //    .ThenInclude(a => a.Telefones)
                //.Include(a => a.Pessoa.Endereco)
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
                    //Telefones = a.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList(),
                    Celular = a.Pessoa.Celular,
                    Contato = a.Pessoa.Contato,
                    ResponsavelContato = a.Pessoa.ResponsavelContato,
                    LocalTrabalho = a.LocalTrabalho,
                    Facebook = a.Facebook,
                    Instagram = a.Instagram,
                    Bloqueio = a.Bloqueio,
                    ObservacaoBloqueio = a.ObservacaoBloqueio,
                    Email = a.Email,
                    Logradouro = a.Pessoa.Logradouro,
                    Numero = a.Pessoa.Numero,
                    Complemento = a.Pessoa.Complemento,
                    Bairro = a.Pessoa.Bairro,
                    Uf = a.Pessoa.Uf,
                    Cidade = a.Pessoa.Cidade,
                    Cep = a.Pessoa.Cep,
                    SituacaoEndereco = a.Pessoa.SituacaoEndereco,
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
                //Telefones = adotante.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList(),
                Celular = adotante.Pessoa.Celular,
                Contato = adotante.Pessoa.Contato,
                ResponsavelContato = adotante.Pessoa.ResponsavelContato,
                LocalTrabalho = adotante.LocalTrabalho,
                Facebook = adotante.Facebook,
                Instagram = adotante.Instagram,
                Bloqueio = adotante.Bloqueio,
                ObservacaoBloqueio = adotante.ObservacaoBloqueio,
                Email = adotante.Email,
                Logradouro = adotante.Pessoa.Logradouro,
                Numero = adotante.Pessoa.Numero,
                Complemento = adotante.Pessoa.Complemento,
                Bairro = adotante.Pessoa.Bairro,
                Uf = adotante.Pessoa.Uf,
                Cidade = adotante.Pessoa.Cidade,
                Cep = adotante.Pessoa.Cep,
                SituacaoEndereco = adotante.Pessoa.SituacaoEndereco,
            };
        }

        public async Task<IEnumerable<AdotanteDto>> ObterAdotantesDesbloqueadosEAtivos()
        {
            var adotantes = await _context.Adotantes
                .Include(a => a.Pessoa)
                //    .ThenInclude(p => p.Endereco)
                //.Include(a => a.Pessoa)
                //    .ThenInclude(p => p.Telefones)
                .Where(a => a.Pessoa.Status == StatusEnum.Ativo && a.Bloqueio == BloqueioEnum.Desbloquado && a.Pessoa.Tipo == TipoPessoaEnum.Adotante)
                .Select(a => new AdotanteDto
                {
                    Id = a.Id,
                    Nome = a.Pessoa.Nome,
                    Rg = a.Pessoa.Rg,
                    Cpf = a.Pessoa.Cpf,
                    Status = a.Pessoa.Status,
                    //Telefones = a.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList(),
                    Celular = a.Pessoa.Celular,
                    Contato = a.Pessoa.Contato,
                    ResponsavelContato = a.Pessoa.ResponsavelContato,
                    LocalTrabalho = a.LocalTrabalho,
                    Facebook = a.Facebook,
                    Instagram = a.Instagram,
                    Bloqueio = a.Bloqueio,
                    ObservacaoBloqueio = a.ObservacaoBloqueio,
                    Email = a.Email,
                    Logradouro = a.Pessoa.Logradouro,
                    Numero = a.Pessoa.Numero,
                    Complemento = a.Pessoa.Complemento,
                    Bairro = a.Pessoa.Bairro,
                    Uf = a.Pessoa.Uf,
                    Cidade = a.Pessoa.Cidade,
                    Cep = a.Pessoa.Cep,
                    SituacaoEndereco = a.Pessoa.SituacaoEndereco,
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

            adotante.Pessoa.Celular = string.IsNullOrEmpty(adotanteDto.Celular) ? adotante.Pessoa.Celular : adotanteDto.Celular;
            adotante.Pessoa.Contato = string.IsNullOrEmpty(adotanteDto.Contato) ? adotante.Pessoa.Contato : adotanteDto.Contato;
            adotante.Pessoa.ResponsavelContato = string.IsNullOrEmpty(adotanteDto.ResponsavelContato) ? adotante.Pessoa.ResponsavelContato : adotanteDto.ResponsavelContato;

            adotante.Pessoa.Logradouro = string.IsNullOrEmpty(adotanteDto.Logradouro) ? adotante.Pessoa.Logradouro : adotanteDto.Logradouro;
            adotante.Pessoa.Numero = adotanteDto.Numero.HasValue ? adotanteDto.Numero.Value : adotante.Pessoa.Numero;
            adotante.Pessoa.Complemento = string.IsNullOrEmpty(adotanteDto.Complemento) ? adotante.Pessoa.Complemento : adotanteDto.Complemento;
            adotante.Pessoa.Bairro = string.IsNullOrEmpty(adotanteDto.Bairro) ? adotante.Pessoa.Bairro : adotanteDto.Bairro;
            adotante.Pessoa.Uf = string.IsNullOrEmpty(adotanteDto.Uf) ? adotante.Pessoa.Uf : adotanteDto.Uf;
            adotante.Pessoa.Cidade = string.IsNullOrEmpty(adotanteDto.Cidade) ? adotante.Pessoa.Cidade : adotanteDto.Cidade;
            adotante.Pessoa.Cep = string.IsNullOrEmpty(adotanteDto.Cep) ? adotante.Pessoa.Cep : adotanteDto.Cep;
            adotante.Pessoa.SituacaoEndereco = string.IsNullOrEmpty(adotanteDto.SituacaoEndereco) ? adotante.Pessoa.SituacaoEndereco : adotanteDto.SituacaoEndereco;

            adotante.LocalTrabalho = string.IsNullOrEmpty(adotanteDto.LocalTrabalho) ? adotante.LocalTrabalho : adotanteDto.LocalTrabalho;
            adotante.Facebook = string.IsNullOrEmpty(adotanteDto.Facebook) ? adotante.Facebook : adotanteDto.Facebook;
            adotante.Instagram = string.IsNullOrEmpty(adotanteDto.Instagram) ? adotante.Instagram : adotanteDto.Instagram;
            adotante.Bloqueio = adotanteDto.Bloqueio.HasValue ? adotanteDto.Bloqueio.Value : adotante.Bloqueio;
            adotante.ObservacaoBloqueio = string.IsNullOrEmpty(adotanteDto.ObservacaoBloqueio) ? adotante.ObservacaoBloqueio : adotanteDto.ObservacaoBloqueio;
            adotante.Email = string.IsNullOrEmpty(adotanteDto.Email) ? adotante.Email : adotanteDto.Email;

            //if (adotanteDto.Telefones != null)
            //{
            //    var telefonesAtuais = adotante.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList();

            //    var telefonesMantidos = adotante.Pessoa.Telefones
            //        .Where(t => adotanteDto.Telefones.Contains(t.NumeroTelefone))
            //        .ToList();

            //    var telefonesParaRemover = adotante.Pessoa.Telefones
            //        .Where(t => !adotanteDto.Telefones.Contains(t.NumeroTelefone))
            //        .ToList();

            //    var telefonesParaAdicionar = adotanteDto.Telefones
            //        .Where(t => !telefonesAtuais.Contains(t))
            //        .ToList();

            //    _context.RemoveRange(telefonesParaRemover);

            //    foreach (var novoTelefone in telefonesParaAdicionar)
            //    {
            //        adotante.Pessoa.Telefones.Add(new Telefone
            //        {
            //            NumeroTelefone = novoTelefone
            //        });
            //    }
            //}

            await _context.SaveChangesAsync();

            return new AdotanteDto
            {
                Id = adotante.Id,
                Nome = adotante.Pessoa.Nome,
                Rg = adotante.Pessoa.Rg,
                Cpf = adotante.Pessoa.Cpf,
                Status = adotante.Pessoa.Status,
                //Telefones = adotante.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList(),
                Celular = adotante.Pessoa.Celular,
                Contato = adotante.Pessoa.Contato,
                ResponsavelContato = adotante.Pessoa.ResponsavelContato,
                LocalTrabalho = adotante.LocalTrabalho,
                Facebook = adotante.Facebook,
                Instagram = adotante.Instagram,
                Bloqueio = adotante.Bloqueio,
                ObservacaoBloqueio = adotante.ObservacaoBloqueio,
                Email = adotante.Email,
                Logradouro = adotante.Pessoa.Logradouro,
                Numero = adotante.Pessoa.Numero,
                Complemento = adotante.Pessoa.Complemento,
                Bairro = adotante.Pessoa.Bairro,
                Uf = adotante.Pessoa.Uf,
                Cidade = adotante.Pessoa.Cidade,
                Cep = adotante.Pessoa.Cep,
                SituacaoEndereco = adotante.Pessoa.SituacaoEndereco,
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

            if (string.IsNullOrEmpty(adotanteDto.Celular) || adotanteDto.Celular.Length != 11)
                erros.Add("O campo 'Celular' é obrigatório!");
            if (string.IsNullOrEmpty(adotanteDto.Contato) || adotanteDto.Contato.Length != 11)
                erros.Add("O campo 'Contato' é obrigatório!");
            if (string.IsNullOrEmpty(adotanteDto.ResponsavelContato))
                erros.Add("O campo 'Responsável Contato' é obrigatório!");

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
            if (string.IsNullOrEmpty(adotanteDto.Email))
                erros.Add("O campo 'Email' é obrigatório!");

            //if (adotanteDto.Telefones == null || adotanteDto.Telefones.Count < 2)
            //    erros.Add("É necessário informar pelo menos dois telefones!");

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

            if (adotanteDto.Celular != null && (string.IsNullOrWhiteSpace(adotanteDto.Celular) || adotanteDto.Celular.ToString().Length != 11))
                erros.Add("O campo 'Celular' não pode ter ser vazio e deve ter exatamente 8 dígitos!");
            if (adotanteDto.Contato != null && (string.IsNullOrWhiteSpace(adotanteDto.Contato) || adotanteDto.Contato.ToString().Length != 11))
                erros.Add("O campo 'Contato' não pode ter ser vazio e deve ter exatamente 8 dígitos!");
            if (adotanteDto.ResponsavelContato != null && string.IsNullOrWhiteSpace(adotanteDto.ResponsavelContato))
                erros.Add("O campo 'Responsavel Contato' não pode ter ser vazio!");

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
            if (adotanteDto.Email != null && string.IsNullOrWhiteSpace(adotanteDto.Email))
                erros.Add("O campo 'Email' não pode ter ser vazio!");

            //if (adotanteDto.Telefones != null && adotanteDto.Telefones.Count < 2)
            //    erros.Add("É necessário informar pelo menos dois telefones!");

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
                //    .ThenInclude(p => p.Endereco)
                //.Include(a => a.Pessoa)
                //    .ThenInclude(p => p.Telefones)
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