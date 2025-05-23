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
                Celular = doadorDto.Celular,
                Contato = doadorDto.Contato,
                ResponsavelContato = doadorDto.ResponsavelContato,
                Logradouro = doadorDto.Logradouro,
                Numero = doadorDto.Numero,
                Complemento = doadorDto.Complemento,
                Bairro = doadorDto.Bairro,
                Uf = doadorDto.Uf,
                Cidade = doadorDto.Cidade,
                Cep = doadorDto.Cep,
            };

            _context.Pessoas.Add(doador);
            await _context.SaveChangesAsync();

            //var telefones = doadorDto.Telefones
            //    .Select(t => new Telefone
            //    {
            //        NumeroTelefone = t,
            //        PessoaId = doador.Id
            //    }).ToList();

            //_context.Telefones.AddRange(telefones);
            //await _context.SaveChangesAsync();

            //var endereco = new Endereco
            //{
            //    Logradouro = doadorDto.Logradouro,
            //    Numero = doadorDto.Numero,
            //    Complemento = doadorDto.Complemento,
            //    Bairro = doadorDto.Bairro,
            //    Uf = doadorDto.Uf,
            //    Cidade = doadorDto.Cidade,
            //    Cep = doadorDto.Cep,
            //    PessoaId = doador.Id
            //};

            //_context.Enderecos.Add(endereco);
            //await _context.SaveChangesAsync();

            return new DoadorDto
            {
                Id = doador.Id,
                Nome = doadorDto.Nome,
                Rg = doadorDto.Rg,
                Cpf = doadorDto.Cpf,
                Status = doadorDto.Status,
                //Telefones = telefones.Select(t => t.NumeroTelefone).ToList(),
                Celular = doadorDto.Celular,
                Contato = doadorDto.Contato,
                ResponsavelContato = doadorDto.ResponsavelContato,
                Logradouro = doadorDto.Logradouro,
                Numero = doadorDto.Numero,
                Complemento = doadorDto.Complemento,
                Bairro = doadorDto.Bairro,
                Uf = doadorDto.Uf,
                Cidade = doadorDto.Cidade,
                Cep = doadorDto.Cep,
            };
        }

        public async Task<IEnumerable<DoadorDto>> ObterDoadores(FiltroDoadorDto filtro)
        {
            var query = _context.Pessoas
                //.Include(d => d.Telefones)
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
                    Status = d.Status,
                    //Telefones = d.Telefones.Select(t => t.NumeroTelefone).ToList(),
                    Celular = d.Celular,
                    Contato = d.Contato,
                    ResponsavelContato = d.ResponsavelContato,
                    Logradouro = d.Logradouro,
                    Numero = d.Numero,
                    Complemento = d.Complemento,
                    Bairro = d.Bairro,
                    Uf = d.Uf,
                    Cidade = d.Cidade,
                    Cep = d.Cep,
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
                Rg = doador?.Rg,
                Cpf = doador?.Cpf,
                Status = doador.Status,
                //Telefones = doador.Telefones.Select(t => t.NumeroTelefone).ToList(),
                Celular = doador.Celular,
                Contato = doador.Contato,
                ResponsavelContato = doador.ResponsavelContato,
                Logradouro = doador.Logradouro,
                Numero = doador.Numero,
                Complemento = doador.Complemento,
                Bairro = doador.Bairro,
                Uf = doador.Uf,
                Cidade = doador.Cidade,
                Cep = doador.Cep,
            };
        }

        public async Task<IEnumerable<DoadorDto>> ObterDoadoresAtivos()
        {
            var doadores = _context.Pessoas
                //.Include(d => d.Telefones)
                .Where(d => d.Status == StatusEnum.Ativo && d.Tipo == TipoPessoaEnum.Doador)
                .Select(d => new DoadorDto
                {
                    Id = d.Id,
                    Nome = d.Nome,
                    Rg = d.Rg,
                    Cpf = d.Cpf,
                    Status = d.Status,
                    //Telefones = d.Telefones.Select(t => t.NumeroTelefone).ToList(),
                    Celular = d.Celular,
                    Contato = d.Contato,
                    ResponsavelContato = d.ResponsavelContato,
                    Logradouro = d.Logradouro,
                    Numero = d.Numero,
                    Complemento = d.Complemento,
                    Bairro = d.Bairro,
                    Uf = d.Uf,
                    Cidade = d.Cidade,
                    Cep = d.Cep,
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

            doador.Celular = string.IsNullOrEmpty(doadorDto.Celular) ? doador.Celular : doadorDto.Celular;
            doador.Contato = string.IsNullOrEmpty(doadorDto.Contato) ? doador.Contato : doadorDto.Contato;
            doador.ResponsavelContato = string.IsNullOrEmpty(doadorDto.ResponsavelContato) ? doador.ResponsavelContato : doadorDto.ResponsavelContato;

            doador.Logradouro = string.IsNullOrEmpty(doadorDto.Logradouro) ? doador.Logradouro : doadorDto.Logradouro;
            doador.Numero = doadorDto.Numero.HasValue ? doadorDto.Numero.Value : doador.Numero;
            doador.Complemento = string.IsNullOrEmpty(doadorDto.Complemento) ? doador.Complemento : doadorDto.Complemento;
            doador.Bairro = string.IsNullOrEmpty(doadorDto.Bairro) ? doador.Bairro : doadorDto.Bairro;
            doador.Uf = string.IsNullOrEmpty(doadorDto.Uf) ? doador.Uf : doadorDto.Uf;
            doador.Cidade = string.IsNullOrEmpty(doadorDto.Cidade) ? doador.Cidade : doadorDto.Cidade;
            doador.Cep = string.IsNullOrEmpty(doadorDto.Cep) ? doador.Cep : doadorDto.Cep;

            //if (doadorDto.Telefones != null)
            //{
            //    var telefonesAtuais = doador.Telefones.Select(t => t.NumeroTelefone).ToList();

            //    var telefonesMantidos = doador.Telefones
            //        .Where(t => doadorDto.Telefones.Contains(t.NumeroTelefone))
            //        .ToList();

            //    var telefonesParaRemover = doador.Telefones
            //        .Where(t => !doadorDto.Telefones.Contains(t.NumeroTelefone))
            //        .ToList();

            //    var telefonesParaAdicionar = doadorDto.Telefones
            //        .Where(t => !telefonesAtuais.Contains(t))
            //        .ToList();

            //    _context.RemoveRange(telefonesParaRemover);

            //    foreach (var novoTelefone in telefonesParaAdicionar)
            //    {
            //        doador.Telefones.Add(new Telefone
            //        {
            //            NumeroTelefone = novoTelefone
            //        });
            //    }
            //}

            await _context.SaveChangesAsync();

            return new DoadorDto
            {
                Id = doador.Id,
                Nome = doador.Nome,
                Rg = doador.Rg,
                Cpf = doador.Cpf,
                Status = doador.Status,
                //Telefones = doador.Telefones.Select(t => t.NumeroTelefone).ToList(),
                Celular = doador.Celular,
                Contato = doador.Contato,
                ResponsavelContato = doador.ResponsavelContato,
                Logradouro = doador.Logradouro,
                Numero = doador.Numero,
                Complemento = doador.Complemento,
                Bairro = doador.Bairro,
                Uf = doador.Uf,
                Cidade = doador.Cidade,
                Cep = doador.Cep,
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
            if (string.IsNullOrEmpty(doadorDto.Status.ToString()) || !Enum.IsDefined(typeof(StatusEnum), doadorDto.Status))
                erros.Add("O campo 'Status' é obrigatório!");

            if (string.IsNullOrEmpty(doadorDto.Celular) || doadorDto.Celular.Length != 11)
                erros.Add("O campo 'Celular' é obrigatório!");
            if (string.IsNullOrEmpty(doadorDto.Contato) || doadorDto.Contato.Length != 11)
                erros.Add("O campo 'Contato' é obrigatório!");
            if (string.IsNullOrEmpty(doadorDto.ResponsavelContato))
                erros.Add("O campo 'Responsavel Contato' é obrigatório!");

            if (string.IsNullOrEmpty(doadorDto.Logradouro))
                erros.Add("O campo 'Logradouro' é obrigatório!");
            if (string.IsNullOrEmpty(doadorDto.Numero.ToString()) || doadorDto.Numero <= 0)
                erros.Add("O campo 'Número' é obrigatório e deve ser maior que zero!");
            if (string.IsNullOrEmpty(doadorDto.Bairro))
                erros.Add("O campo 'Bairro' é obrigatório!");
            if (string.IsNullOrEmpty(doadorDto.Uf) || doadorDto.Uf.Length != 2)
                erros.Add("O campo 'UF' é obrigatório e deve ter 2 caracteres!");
            if (string.IsNullOrEmpty(doadorDto.Cidade))
                erros.Add("O campo 'Cidade' é obrigatório!");
            if (string.IsNullOrEmpty(doadorDto.Cep) || doadorDto.Cep.Length != 8)
                erros.Add("O campo 'CEP' é obrigatório e deve conter exatamente 8 dígitos!");

            //if (doadorDto.Telefones == null || doadorDto.Telefones.Count < 2)
            //    erros.Add("É necessário informar pelo menos dois telefones!");

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

            if (doadorDto.Nome != null && string.IsNullOrWhiteSpace(doadorDto.Nome))
                erros.Add("O campo 'Nome' não pode ter ser vazio!");
            if (doadorDto.Rg != null && string.IsNullOrWhiteSpace(doadorDto.Rg))
                erros.Add("O campo 'Rg' não pode ter ser vazio!");
            if (doadorDto.Cpf != null && string.IsNullOrWhiteSpace(doadorDto.Cpf))
                erros.Add("O campo 'Cpf' não pode ter ser vazio!");
            if (doadorDto.Status != null && string.IsNullOrWhiteSpace(doadorDto.Status.ToString()))
                erros.Add("O campo 'Status' não pode ter ser vazio!");

            if (doadorDto.Celular != null && (string.IsNullOrWhiteSpace(doadorDto.Celular) || doadorDto.Celular.ToString().Length != 11))
                erros.Add("O campo 'Celular' não pode ter ser vazio e deve ter exatamente 8 dígitos!");
            if (doadorDto.Contato != null && (string.IsNullOrWhiteSpace(doadorDto.Contato) || doadorDto.Contato.ToString().Length != 11))
                erros.Add("O campo 'Contato' não pode ter ser vazio e deve ter exatamente 8 dígitos!");
            if (doadorDto.ResponsavelContato != null && string.IsNullOrWhiteSpace(doadorDto.ResponsavelContato))
                erros.Add("O campo 'Responsavel Contato' não pode ter ser vazio!");

            if (doadorDto.Logradouro != null && string.IsNullOrWhiteSpace(doadorDto.Logradouro))
                erros.Add("O campo 'Logradouro' não pode ter ser vazio!");
            if (doadorDto.Numero != null && doadorDto.Numero <= 0)
                erros.Add("O campo 'Número' não pode ter ser vazio e deve ser maior que zero!");
            if (doadorDto.Bairro != null && string.IsNullOrWhiteSpace(doadorDto.Bairro))
                erros.Add("O campo 'Bairro' não pode ter ser vazio!");
            if (doadorDto.Uf != null && (doadorDto.Uf.Length != 2 || string.IsNullOrWhiteSpace(doadorDto.Uf)))
                erros.Add("O campo 'UF' não pode ter ser vazio e deve ter 2 caracteres!");
            if (doadorDto.Cidade != null && string.IsNullOrWhiteSpace(doadorDto.Cidade))
                erros.Add("O campo 'Cidade' não pode ter ser vazio!");
            if (doadorDto.Cep != null && (string.IsNullOrWhiteSpace(doadorDto.Cep) || doadorDto.Cep.ToString().Length != 8))
                erros.Add("O campo 'CEP' não pode ter ser vazio e deve ter exatamente 8 dígitos!");

            //if (doadorDto.Telefones != null && doadorDto.Telefones.Count < 2)
            //    erros.Add("É necessário informar pelo menos dois telefones!");

            return erros;
        }

        #region MÉTODOS PRIVADAS

        private async Task<Pessoa?> BuscarDoadorPorId(int id)
        {
            return await _context.Pessoas
                //.Include(p => p.Endereco)
                //.Include(p => p.Telefones)
                .Where(p => p.Tipo == TipoPessoaEnum.Doador)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        #endregion
    }
}