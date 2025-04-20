using AAPS.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AAPS.Api.Context;

public class AppDbContext : IdentityDbContext<Voluntario, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Telefone> Telefones { get; set; }
    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Adotante> Adotantes { get; set; }
    //public DbSet<Doador> Doadores { get; set; }
    public DbSet<PontoAdocao> PontosAdocao { get; set; }
    public DbSet<Voluntario> Voluntarios { get; set; }
    public DbSet<Animal> Animais { get; set; }
    public DbSet<Acompanhamento> Acompanhamentos { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<Adocao> Adocoes { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region ENDEREÇO - Mapeamento

        modelBuilder.Entity<Endereco>(entity =>
        {
            entity.ToTable("Enderecos", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Logradouro).HasColumnType("nvarchar(150)").IsRequired();
            entity.Property(x => x.Numero).HasColumnType("int").IsRequired();
            entity.Property(x => x.Complemento).HasColumnType("nvarchar(100)");
            entity.Property(x => x.Bairro).HasColumnType("nvarchar(100)").IsRequired();
            entity.Property(x => x.Uf).HasColumnType("char(2)").IsRequired();
            entity.Property(x => x.Cidade).HasColumnType("nvarchar(50)").IsRequired();
            entity.Property(x => x.Cep).HasColumnType("nvarchar(8)").IsRequired();
            entity.Property(x => x.SituacaoEndereco).HasColumnType("nvarchar(100)");

            entity.Property(x => x.PessoaId).HasColumnType("int").IsRequired();

            // Relacionamento com pessoa já está em Pessoa
        });

        #endregion

        #region TELEFONE - Mapeamento

        modelBuilder.Entity<Telefone>(entity =>
        {
            entity.ToTable("Telefones", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.NumeroTelefone).HasColumnType("nvarchar(11)").IsRequired();
            //entity.Property(x => x.Responsavel).HasColumnType("nvarchar(60)").IsRequired();

            entity.Property(x => x.PessoaId).HasColumnType("int").IsRequired();

            entity.HasOne(x => x.Pessoa)
                  .WithMany(x => x.Telefones)
                  .HasForeignKey(x => x.PessoaId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        #endregion

        #region PESSOA - Mapeamento

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.ToTable("Pessoas", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Nome).HasColumnType("nvarchar(60)");
            entity.Property(x => x.Rg).HasColumnType("nvarchar(9)");
            entity.Property(x => x.Cpf).HasColumnType("nvarchar(11)");
            entity.Property(x => x.Tipo).HasColumnType("int").IsRequired();
            entity.Property(x => x.Status).HasColumnType("int").IsRequired();

            // Relacionamentos 1:1
            entity.HasOne(x => x.Adotante)
                  .WithOne(x => x.Pessoa)
                  .HasForeignKey<Adotante>(x => x.PessoaId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.PontoAdocao)
                  .WithOne(x => x.Pessoa)
                  .HasForeignKey<PontoAdocao>(x => x.PessoaId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.Voluntario)
                  .WithOne(x => x.Pessoa)
                  .HasForeignKey<Voluntario>(x => x.PessoaId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.Endereco)
                  .WithOne(x => x.Pessoa)
                  .HasForeignKey<Endereco>(x => x.PessoaId)
                  .OnDelete(DeleteBehavior.NoAction);

            // Relacionamentos 1:N
            entity.HasMany(x => x.Telefones)
                  .WithOne(x => x.Pessoa);

            entity.HasMany(x => x.Animais)
                  .WithOne(x => x.Pessoa);
        });

        #endregion

        #region ADOTANTE - Mapeamento

        modelBuilder.Entity<Adotante>(entity =>
        {
            entity.ToTable("Adotantes", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.LocalTrabalho).HasColumnType("nvarchar(80)");
            entity.Property(x => x.Facebook).HasColumnType("nvarchar(150)").IsRequired();
            entity.Property(x => x.Instagram).HasColumnType("nvarchar(150)").IsRequired();
            entity.Property(x => x.Bloqueio).HasColumnType("int").IsRequired();

            entity.Property(x => x.PessoaId).HasColumnType("int").IsRequired();

            // Relacionamento com pessoa já esta em Pessoa

            entity.HasMany(x => x.Adocoes)
                  .WithOne(x => x.Adotante);
        });

        #endregion

        #region PONTO ADOÇÃO - Mapeamento

        modelBuilder.Entity<PontoAdocao>(entity =>
        {
            entity.ToTable("PontosAdocao", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.NomeFantasia).HasColumnType("nvarchar(60)").IsRequired();
            entity.Property(x => x.Responsavel).HasColumnType("nvarchar(60)").IsRequired();
            entity.Property(x => x.Cnpj).HasColumnType("nvarchar(14)").IsRequired();

            entity.Property(x => x.PessoaId).HasColumnType("int").IsRequired();

            // Relacionamento com pessoa já esta em Pessoa

            entity.HasMany(x => x.Adocoes)
                  .WithOne(x => x.PontoAdocao);
        });

        #endregion

        #region VOLUNTARIO - Mapeamento

        modelBuilder.Entity<Voluntario>(entity =>
        {
            entity.ToTable("Voluntarios", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.PessoaId).HasColumnType("int").IsRequired();

            /// Relacionamento com pessoa já esta em Pessoa

            entity.HasMany(x => x.Adocoes)
                  .WithOne(x => x.Voluntario);
        });

        #endregion

        #region ANIMAL - Mapeamento

        modelBuilder.Entity<Animal>(entity =>
        {
            entity.ToTable("Animais", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Nome).HasColumnType("nvarchar(30)").IsRequired();
            entity.Property(x => x.Especie).HasColumnType("nvarchar(25)").IsRequired();
            entity.Property(x => x.Raca).HasColumnType("nvarchar(25)").IsRequired();
            entity.Property(x => x.Pelagem).HasColumnType("nvarchar(25)").IsRequired();
            entity.Property(x => x.Sexo).HasColumnType("nvarchar(10)").IsRequired();
            entity.Property(x => x.DataNascimento).HasColumnType("date");
            entity.Property(x => x.Status).HasColumnType("int").IsRequired();
            entity.Property(x => x.Disponibilidade).HasColumnType("int").IsRequired();
            entity.Property(x => x.Devolvido).HasColumnType("int").IsRequired();

            entity.Property(x => x.PessoaId).HasColumnType("int").IsRequired();
            
            entity.HasOne(x => x.Pessoa)
                  .WithMany(x => x.Animais)
                  .HasForeignKey(x => x.PessoaId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasMany(x => x.Acompanhamentos)
                  .WithOne(x => x.Animal);

            entity.HasMany(x => x.Adocoes)
                  .WithOne(x => x.Animal);
        });

        #endregion

        #region ACOMPANHAMENTO - Mapeamento

        modelBuilder.Entity<Acompanhamento>(entity =>
        {
            entity.ToTable("Acompanhamentos", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Data).HasColumnType("date").IsRequired();
            entity.Property(x => x.Observacao).HasColumnType("nvarchar(600)");

            entity.Property(x => x.AnimalId).HasColumnType("int").IsRequired();
            entity.Property(x => x.EventoId).HasColumnType("int").IsRequired();

            entity.HasOne(x => x.Evento)
                  .WithMany(x => x.Acompanhamentos)
                  .HasForeignKey(x => x.EventoId)
                  .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.Animal)
                  .WithMany(x => x.Acompanhamentos)
                  .HasForeignKey(x => x.AnimalId)
                  .OnDelete(DeleteBehavior.NoAction);
        });

        #endregion

        #region EVENTO - Mapeamento

        modelBuilder.Entity<Evento>(entity =>
        {
            entity.ToTable("Eventos", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Descricao).HasColumnType("nvarchar(50)").IsRequired();
            entity.Property(x => x.Status).HasColumnType("int").IsRequired();

            entity.HasMany(x => x.Acompanhamentos)
                  .WithOne(x => x.Evento);
        });

        #endregion

        #region ADOÇÃO - Mapeamento

        modelBuilder.Entity<Adocao>(entity =>
        {
            entity.ToTable("Adocoes", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Data).HasColumnType("date").IsRequired();

            entity.Property(x => x.AnimalId).HasColumnType("int").IsRequired();
            entity.Property(x => x.AdotanteId).HasColumnType("int").IsRequired();
            entity.Property(x => x.VoluntarioId).HasColumnType("int").IsRequired();
            entity.Property(x => x.PontoAdocaoId).HasColumnType("int").IsRequired();

            entity.HasOne(x => x.Animal)
                    .WithMany(x => x.Adocoes)
                    .HasForeignKey(x => x.AnimalId)
                    .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.Adotante)
                    .WithMany(x => x.Adocoes)
                    .HasForeignKey(x => x.AdotanteId)
                    .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.PontoAdocao)
                    .WithMany(x => x.Adocoes)
                    .HasForeignKey(x => x.PontoAdocaoId)
                    .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.Voluntario)
                    .WithMany(x => x.Adocoes)
                    .HasForeignKey(x => x.VoluntarioId)
                    .OnDelete(DeleteBehavior.NoAction);
        });

        #endregion

        #region IDENTITY - Colunas desnecessárias

        modelBuilder.Entity<Voluntario>(entity =>
        {
            entity.Ignore(u => u.NormalizedEmail);
            entity.Ignore(u => u.EmailConfirmed);
            entity.Ignore(u => u.ConcurrencyStamp);
            entity.Ignore(u => u.PhoneNumberConfirmed);
            entity.Ignore(u => u.TwoFactorEnabled);
            entity.Ignore(u => u.LockoutEnabled);
            entity.Ignore(u => u.LockoutEnd);
            entity.Ignore(u => u.AccessFailedCount);
        });

        #endregion
    }
}