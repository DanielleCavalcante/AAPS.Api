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
            //var pessoa = new Pessoa
            //{
            //    Tipo = TipoPessoaEnum.PontoAdocao,
            //    Status = pontoAdocaoDto.Status,
            //};

            //_context.Pessoas.Add(pessoa);
            //await _context.SaveChangesAsync();

            //var telefones = pontoAdocaoDto.Telefones
            //    .Select(t => new Telefone
            //    {
            //        NumeroTelefone = t,
            //        PessoaId = pessoa.Id
            //    }).ToList();

            //_context.Telefones.AddRange(telefones);
            //await _context.SaveChangesAsync();

            //var endereco = new Endereco
            //{
            //    Logradouro = pontoAdocaoDto.Logradouro,
            //    Numero = pontoAdocaoDto.Numero,
            //    Complemento = pontoAdocaoDto.Complemento,
            //    Bairro = pontoAdocaoDto.Bairro,
            //    Uf = pontoAdocaoDto.Uf,
            //    Cidade = pontoAdocaoDto.Cidade,
            //    Cep = pontoAdocaoDto.Cep,
            //    PessoaId = pessoa.Id
            //};

            //_context.Enderecos.Add(endereco);
            //await _context.SaveChangesAsync();

            var pontoAdocao = new PontoAdocao
            {
                NomeFantasia = pontoAdocaoDto.NomeFantasia,
                Cnpj = pontoAdocaoDto.Cnpj,

                Celular = pontoAdocaoDto.Celular,
                Contato = pontoAdocaoDto.Contato,
                ResponsavelContato = pontoAdocaoDto.ResponsavelContato,

                Logradouro = pontoAdocaoDto.Logradouro,
                Numero = pontoAdocaoDto.Numero,
                Complemento = pontoAdocaoDto.Complemento,
                Bairro = pontoAdocaoDto.Bairro,
                Uf = pontoAdocaoDto.Uf,
                Cidade = pontoAdocaoDto.Cidade,
                Cep = pontoAdocaoDto.Cep,
            };

            _context.PontosAdocao.Add(pontoAdocao);
            await _context.SaveChangesAsync();

            return new PontoAdocaoDto
            {
                NomeFantasia = pontoAdocaoDto.NomeFantasia,
                Cnpj = pontoAdocaoDto.Cnpj,
                Status = pontoAdocaoDto.Status,
                //Telefones = telefones.Select(t => t.NumeroTelefone).ToList(),

                Celular = pontoAdocaoDto.Celular,
                Contato = pontoAdocaoDto.Contato,
                ResponsavelContato = pontoAdocaoDto.ResponsavelContato,

                Logradouro = pontoAdocaoDto.Logradouro,
                Numero = pontoAdocaoDto.Numero,
                Complemento = pontoAdocaoDto.Complemento,
                Bairro = pontoAdocaoDto.Bairro,
                Uf = pontoAdocaoDto.Uf,
                Cidade = pontoAdocaoDto.Cidade,
                Cep = pontoAdocaoDto.Cep,
            };
        }

        public async Task<IEnumerable<PontoAdocaoDto>> ObterPontosAdocao(FiltroPontoAdocaoDto filtro)
        {
            var query = _context.PontosAdocao
                //.Include(p => p.Pessoa)
                //    .ThenInclude(p => p.Telefones)
                //.Where(p => p.Pessoa.Tipo == TipoPessoaEnum.PontoAdocao)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filtro.Busca))
            {
                string buscaLower = filtro.Busca.ToLower();

                query = query.Where(p =>
                    p.NomeFantasia.ToLower().Contains(buscaLower) ||
                    p.Cnpj.Contains(buscaLower)
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
                    Cnpj = p.Cnpj,
                    Status = p.Status,

                    Celular = p.Celular,
                    Contato = p.Contato,
                    ResponsavelContato = p.ResponsavelContato,
                    //Telefones = p.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList(),
                    Logradouro = p.Logradouro,
                    Numero = p.Numero,
                    Complemento = p.Complemento,
                    Bairro = p.Bairro,
                    Uf = p.Uf,
                    Cidade = p.Cidade,
                    Cep = p.Cep,
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
                Cnpj = pontoAdocao.Cnpj,
                Status = pontoAdocao.Status,
                //Telefones = pontoAdocao.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList(),

                Celular = pontoAdocao.Celular,
                Contato = pontoAdocao.Contato,
                ResponsavelContato = pontoAdocao.ResponsavelContato,

                Logradouro = pontoAdocao.Logradouro,
                Numero = pontoAdocao.Numero,
                Complemento = pontoAdocao.Complemento,
                Bairro = pontoAdocao.Bairro,
                Uf = pontoAdocao.Uf,
                Cidade = pontoAdocao.Cidade,
                Cep = pontoAdocao.Cep,
            };
        }

        public async Task<IEnumerable<PontoAdocaoDto>> ObterPontosAdocaoAtivos()
        {
            var pontosAdocao = await _context.PontosAdocao
                //.Include(p => p.Pessoa)
                //    .ThenInclude(p => p.Endereco)
                //.Include(p => p.Pessoa)
                //    .ThenInclude(p => p.Endereco)
                .Where(p => p.Status == StatusEnum.Ativo /*&& p.Pessoa.Tipo == TipoPessoaEnum.PontoAdocao*/)
                .Select(p => new PontoAdocaoDto
                {
                    Id = p.Id,
                    NomeFantasia = p.NomeFantasia,
                    Cnpj = p.Cnpj,
                    Status = p.Status,
                    //Telefones = p.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList(),

                    Celular = p.Celular,
                    Contato = p.Contato,
                    ResponsavelContato = p.ResponsavelContato,

                    Logradouro = p.Logradouro,
                    Numero = p.Numero,
                    Complemento = p.Complemento,
                    Bairro = p.Bairro,
                    Uf = p.Uf,
                    Cidade = p.Cidade,
                    Cep = p.Cep,
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

            pontoAdocao.Status = pontoAdocaoDto.Status.HasValue ? pontoAdocaoDto.Status.Value : pontoAdocao.Status;

            pontoAdocao.NomeFantasia = string.IsNullOrEmpty(pontoAdocaoDto.NomeFantasia) ? pontoAdocao.NomeFantasia : pontoAdocaoDto.NomeFantasia;
            pontoAdocao.Cnpj = string.IsNullOrEmpty(pontoAdocaoDto.Cnpj) ? pontoAdocao.Cnpj : pontoAdocaoDto.Cnpj;

            pontoAdocao.Logradouro = string.IsNullOrEmpty(pontoAdocaoDto.Logradouro) ? pontoAdocao.Logradouro : pontoAdocaoDto.Logradouro;
            pontoAdocao.Numero = pontoAdocaoDto.Numero.HasValue ? pontoAdocaoDto.Numero.Value : pontoAdocao.Numero;
            pontoAdocao.Complemento = string.IsNullOrEmpty(pontoAdocaoDto.Complemento) ? pontoAdocao.Complemento : pontoAdocaoDto.Complemento;
            pontoAdocao.Bairro = string.IsNullOrEmpty(pontoAdocaoDto.Bairro) ? pontoAdocao.Bairro : pontoAdocaoDto.Bairro;
            pontoAdocao.Uf = string.IsNullOrEmpty(pontoAdocaoDto.Uf) ? pontoAdocao.Uf : pontoAdocaoDto.Uf;
            pontoAdocao.Cidade = string.IsNullOrEmpty(pontoAdocaoDto.Cidade) ? pontoAdocao.Cidade : pontoAdocaoDto.Cidade;
            pontoAdocao.Cep = string.IsNullOrEmpty(pontoAdocaoDto.Cep) ? pontoAdocao.Cep : pontoAdocaoDto.Cep;

            pontoAdocao.Celular = string.IsNullOrEmpty(pontoAdocaoDto.Celular) ? pontoAdocao.Celular : pontoAdocaoDto.Celular;
            pontoAdocao.Contato = string.IsNullOrEmpty(pontoAdocaoDto.Contato) ? pontoAdocao.Contato : pontoAdocaoDto.Contato;
            pontoAdocao.ResponsavelContato = string.IsNullOrEmpty(pontoAdocaoDto.ResponsavelContato) ? pontoAdocao.ResponsavelContato : pontoAdocaoDto.ResponsavelContato;


            //if (pontoAdocaoDto.Telefones != null)
            //{
            //    var telefonesAtuais = pontoAdocao.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList();

            //    var telefonesMantidos = pontoAdocao.Pessoa.Telefones
            //        .Where(t => pontoAdocaoDto.Telefones.Contains(t.NumeroTelefone))
            //        .ToList();

            //    var telefonesParaRemover = pontoAdocao.Pessoa.Telefones
            //        .Where(t => !pontoAdocaoDto.Telefones.Contains(t.NumeroTelefone))
            //        .ToList();

            //    var telefonesParaAdicionar = pontoAdocaoDto.Telefones
            //        .Where(t => !telefonesAtuais.Contains(t))
            //        .ToList();

            //    _context.RemoveRange(telefonesParaRemover);

            //    foreach (var novoTelefone in telefonesParaAdicionar)
            //    {
            //        pontoAdocao.Pessoa.Telefones.Add(new Telefone
            //        {
            //            NumeroTelefone = novoTelefone
            //        });
            //    }
            //}

            await _context.SaveChangesAsync();

            return new PontoAdocaoDto
            {
                Id = pontoAdocao.Id,
                NomeFantasia = pontoAdocao.NomeFantasia,
                Cnpj = pontoAdocao.Cnpj,
                Status = pontoAdocao.Status,
                //Telefones = pontoAdocao.Pessoa.Telefones.Select(t => t.NumeroTelefone).ToList(),

                Celular = pontoAdocao.Celular,
                Contato = pontoAdocao.Contato,
                ResponsavelContato = pontoAdocao.ResponsavelContato,

                Logradouro = pontoAdocao.Logradouro,
                Numero = pontoAdocao.Numero,
                Complemento = pontoAdocao.Complemento,
                Bairro = pontoAdocao.Bairro,
                Uf = pontoAdocao.Uf,
                Cidade = pontoAdocao.Cidade,
                Cep = pontoAdocao.Cep,
            };
        }

        public async Task<bool> ExcluirPontoAdocao(int id)
        {
            var pontoAdocao = await BuscarPontoAdocaoPorId(id);

            if (pontoAdocao == null)
            {
                return false;
            }

            pontoAdocao.Status = StatusEnum.Inativo;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<string>> ValidarCriacaoPontoAdocao(CriarPontoAdocaoDto pontoAdocaoDto)
        {
            var erros = new List<string>();

            if (string.IsNullOrEmpty(pontoAdocaoDto.Status.ToString()) || !Enum.IsDefined(typeof(StatusEnum), pontoAdocaoDto.Status))
                erros.Add("O campo 'Status' é obrigatório!");

            if (string.IsNullOrEmpty(pontoAdocaoDto.NomeFantasia))
                erros.Add("O campo 'Nome Fantasia' é obrigatório!");
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

            //if (pontoAdocaoDto.Telefones == null || pontoAdocaoDto.Telefones.Count < 2)
            //    erros.Add("É necessário informar pelo menos dois telefones!");

            if (string.IsNullOrEmpty(pontoAdocaoDto.Celular) || pontoAdocaoDto.Celular.Length != 11)
                erros.Add("O campo 'Celular' é obrigatório!");
            if (string.IsNullOrEmpty(pontoAdocaoDto.Contato) || pontoAdocaoDto.Contato.Length != 11)
                erros.Add("O campo 'Contato' é obrigatório!");
            if (string.IsNullOrEmpty(pontoAdocaoDto.ResponsavelContato))
                erros.Add("O campo 'Responsavel Contato' é obrigatório!");


            var pontoAdocaoExistente = await _context.PontosAdocao
                //.Include(p => p.Pessoa)
                //.ThenInclude(p => p.Endereco)
                .Where(p =>
                    p.Cnpj == pontoAdocaoDto.Cnpj
                )
                .FirstOrDefaultAsync();

            if (pontoAdocaoExistente != null)
            {
                erros.Add($"Ponto de adoção já cadastrado. Código {pontoAdocaoExistente.Id}");
            }

            return erros;
        }

        public async Task<List<string>> ValidarAtualizacaoPontoAdocao(int id, AtualizaPontoAdocaoDto pontoAdocaoDto)
        {
            var erros = new List<string>();

            if (pontoAdocaoDto.Status != null && string.IsNullOrWhiteSpace(pontoAdocaoDto.Status.ToString()))
                erros.Add("O campo 'Status' não pode ter ser vazio!");

            if (pontoAdocaoDto.Logradouro != null && string.IsNullOrWhiteSpace(pontoAdocaoDto.Logradouro))
                erros.Add("O campo 'Logradouro' não pode ter ser vazio!");
            if (pontoAdocaoDto.Numero != null && pontoAdocaoDto.Numero <= 0)
                erros.Add("O campo 'Número' não pode ter ser vazio e deve ser maior que zero!");
            if (pontoAdocaoDto.Bairro != null && string.IsNullOrWhiteSpace(pontoAdocaoDto.Bairro))
                erros.Add("O campo 'Bairro' não pode ter ser vazio!");
            if (pontoAdocaoDto.Uf != null && (pontoAdocaoDto.Uf.Length != 2 || string.IsNullOrWhiteSpace(pontoAdocaoDto.Uf)))
                erros.Add("O campo 'UF' não pode ter ser vazio e deve ter 2 caracteres!");
            if (pontoAdocaoDto.Cidade != null && string.IsNullOrWhiteSpace(pontoAdocaoDto.Cidade))
                erros.Add("O campo 'Cidade' não pode ter ser vazio!");
            if (pontoAdocaoDto.Cep != null && (string.IsNullOrWhiteSpace(pontoAdocaoDto.Cep) || pontoAdocaoDto.Cep.ToString().Length != 8))
                erros.Add("O campo 'CEP' não pode ter ser vazio e deve ter exatamente 8 dígitos!");

            if (pontoAdocaoDto.NomeFantasia != null && string.IsNullOrWhiteSpace(pontoAdocaoDto.NomeFantasia))
                erros.Add("O campo 'Nome Fantasia' não pode ser vazio!");
            if (pontoAdocaoDto.Cnpj != null && string.IsNullOrWhiteSpace(pontoAdocaoDto.Cnpj))
                erros.Add("O campo 'CNPJ' não pode ser vazio!");

            //if (pontoAdocaoDto.Telefones != null && pontoAdocaoDto.Telefones.Count < 2)
            //    erros.Add("É necessário informar pelo menos dois telefones!");

            if (pontoAdocaoDto.Celular != null && (string.IsNullOrWhiteSpace(pontoAdocaoDto.Celular) || pontoAdocaoDto.Celular.ToString().Length != 11))
                erros.Add("O campo 'Contato 1' não pode ter ser vazio e deve ter exatamente 8 dígitos!");
            if (pontoAdocaoDto.Contato != null && (string.IsNullOrWhiteSpace(pontoAdocaoDto.Contato) || pontoAdocaoDto.Contato.ToString().Length != 11))
                erros.Add("O campo 'Contato 2' não pode ter ser vazio e deve ter exatamente 8 dígitos!");
            if (pontoAdocaoDto.ResponsavelContato != null && string.IsNullOrWhiteSpace(pontoAdocaoDto.ResponsavelContato))
                erros.Add("O campo 'Responsavel Contato' não pode ser vazio!");


            var pontoAdocaoExistente = await _context.PontosAdocao
                .Where(p =>
                    p.Cnpj == pontoAdocaoDto.Cnpj && p.Id != id
                )
                .FirstOrDefaultAsync();

            if (pontoAdocaoExistente != null)
            {
                erros.Add($"Ponto de adoção já cadastrado. Código {pontoAdocaoExistente.Id}");
            }

            return erros;
        }

        #region MÉTODOS PRIVADOS

        private async Task<PontoAdocao?> BuscarPontoAdocaoPorId(int id)
        {
            return await _context.PontosAdocao
                //.Include(p => p.Pessoa)
                //    .ThenInclude(p => p.Endereco)
                //.Include(p => p.Pessoa)
                //    .ThenInclude(p => p.Telefones)
                //.Where(p => p.Pessoa.Tipo == TipoPessoaEnum.PontoAdocao)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        #endregion
    }
}