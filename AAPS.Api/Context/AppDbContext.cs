using AAPS.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AAPS.Api.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Adocao> Adocoes { get; set; }
    public DbSet<Adotante> Adotantes { get; set; }
    public DbSet<Animal> Animais { get; set; }
    public DbSet<Doador> Doadores { get; set; }
    public DbSet<AnimalEvento> AnimalEvento { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<PontoAdocao> PontosAdocao { get; set; }
    public DbSet<Animal> Telefones { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region ADOÇÃO - Mapeamento
        modelBuilder.Entity<Adocao>(entity =>
        {
            entity.ToTable("Adocao", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.DataAdocao).HasColumnType("date").IsRequired();
            entity.Property(x => x.AdotanteId).HasColumnType("int").IsRequired();
            entity.Property(x => x.AnimalId).HasColumnType("int").IsRequired();
            entity.Property(x => x.UsuarioId).HasColumnType("int").IsRequired();
            entity.Property(x => x.PontoAdocaoId).HasColumnType("int").IsRequired();

            // Relações
            entity.HasOne(x => x.Animal)
                .WithMany(x => x.Adocoes)
                .HasForeignKey(x => x.AnimalId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.Usuario)
                .WithMany(x => x.Adocoes)
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.Usuario)
                .WithMany(x => x.Adocoes)
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.Adotante)
                .WithMany(x => x.Adocoes)
                .HasForeignKey(x => x.AdotanteId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.PontoAdocao)
                .WithMany(x => x.Adocoes)
                .HasForeignKey(x => x.PontoAdocaoId)
                .OnDelete(DeleteBehavior.NoAction);
        });
        #endregion

        #region ADOTANTE - Mapeamento
        modelBuilder.Entity<Adotante>(entity =>
        {
            entity.ToTable("Adotante", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Nome).HasColumnType("nvarchar(60)").IsRequired();
            entity.Property(x => x.Rg).HasColumnType("nvarchar(9)").IsRequired();
            entity.Property(x => x.Cpf).HasColumnType("nvarchar(11)").IsRequired();
            entity.Property(x => x.LocalTrabalho).HasColumnType("nvarchar(80)");
            entity.Property(x => x.Status).HasColumnType("bit").IsRequired();
            entity.Property(x => x.Facebook).HasColumnType("nvarchar(150)").IsRequired();
            entity.Property(x => x.Instagram).HasColumnType("nvarchar(150)").IsRequired();

            entity.Property(x => x.Logradouro).HasColumnType("nvarchar(150)").IsRequired();
            entity.Property(x => x.Numero).HasColumnType("int").IsRequired();
            entity.Property(x => x.Complemento).HasColumnType("nvarchar(100)");
            entity.Property(x => x.Bairro).HasColumnType("nvarchar(100)").IsRequired();
            entity.Property(x => x.Uf).HasColumnType("nvarchar(2)").IsRequired();
            entity.Property(x => x.Cidade).HasColumnType("nvarchar(50)").IsRequired();
            entity.Property(x => x.Cep).HasColumnType("int").IsRequired();

            // Relações
            entity.HasMany(x => x.Telefones)
                .WithOne(x => x.Adotante);

            entity.HasMany(x => x.Adocoes)
                .WithOne(x => x.Adotante);
        });
        #endregion

        #region ANIMAL - Mapeamento
        modelBuilder.Entity<Animal>(entity =>
        {
            entity.ToTable("Animal", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Nome).HasColumnType("nvarchar(30)").IsRequired();
            entity.Property(x => x.Especie).HasColumnType("nvarchar(25)").IsRequired();
            entity.Property(x => x.Raca).HasColumnType("nvarchar(25)").IsRequired();
            entity.Property(x => x.Pelagem).HasColumnType("nvarchar(25)").IsRequired();
            entity.Property(x => x.Sexo).HasColumnType("nvarchar(10)").IsRequired();
            entity.Property(x => x.DataNascimento).HasColumnType("date");
            entity.Property(x => x.Status).HasColumnType("bit").IsRequired();
            entity.Property(x => x.DoadorId).HasColumnType("int").IsRequired();

            // Relações
            entity.HasOne(x => x.Doador)
                .WithMany(x => x.Animais)
                .HasForeignKey(x => x.DoadorId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasMany(x => x.AnimalEventos)
                .WithOne(x => x.Animal);

            entity.HasMany(x => x.Adocoes)
                .WithOne(x => x.Animal);
        });
        #endregion

        #region ANIMAL EVENTO - Mapeamento
        modelBuilder.Entity<AnimalEvento>(entity =>
        {
            entity.ToTable("AnimalEvento", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.DataAcompanhamento).HasColumnType("date").IsRequired();
            entity.Property(x => x.Observacao).HasColumnType("nvarchar(500)");
            entity.Property(x => x.AnimalId).HasColumnType("int").IsRequired();
            entity.Property(x => x.EventoId).HasColumnType("int").IsRequired();

            // Relações
            entity.HasOne(x => x.Evento)
                .WithMany(x => x.AnimalEventos)
                .HasForeignKey(x => x.EventoId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.Animal)
               .WithMany(x => x.AnimalEventos)
               .HasForeignKey(x => x.AnimalId)
               .OnDelete(DeleteBehavior.NoAction);
        });
        #endregion

        #region DOADOR - Mapeamento
        modelBuilder.Entity<Doador>(entity =>
        {
            entity.ToTable("Doador", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Nome).HasColumnType("nvarchar(100)").IsRequired();
            entity.Property(x => x.Rg).HasColumnType("nvarchar(100)").IsRequired();
            entity.Property(x => x.Cpf).HasColumnType("nvarchar(20)").IsRequired();

            entity.Property(x => x.Logradouro).HasColumnType("nvarchar(150)").IsRequired();
            entity.Property(x => x.Numero).HasColumnType("int").IsRequired();
            entity.Property(x => x.Complemento).HasColumnType("nvarchar(100)");
            entity.Property(x => x.Bairro).HasColumnType("nvarchar(100)").IsRequired();
            entity.Property(x => x.Uf).HasColumnType("nvarchar(2)").IsRequired();
            entity.Property(x => x.Cidade).HasColumnType("nvarchar(50)").IsRequired();
            entity.Property(x => x.Cep).HasColumnType("int").IsRequired();

            // Relações
            entity.HasMany(x => x.Animais)
                .WithOne(x => x.Doador);

            entity.HasMany(x => x.Telefones)
                .WithOne(x => x.Doador);
        });
        #endregion

        #region EVENTO - Mapeamento
        modelBuilder.Entity<Evento>(entity =>
        {
            entity.ToTable("Evento", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Code).HasColumnType("tinyint").IsRequired();

            entity.HasMany(x => x.AnimalEventos)
                .WithOne(x => x.Evento);
        });
        #endregion

        #region PONTO ADOÇÃO - Mapeamento
        modelBuilder.Entity<PontoAdocao>(entity =>
        {
            entity.ToTable("PontoAdocao", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.NomeFantasia).HasColumnType("nvarchar(60)").IsRequired();
            entity.Property(x => x.Responsavel).HasColumnType("nvarchar(60)").IsRequired();
            entity.Property(x => x.Cnpj).HasColumnType("nvarchar(14)").IsRequired();

            entity.Property(x => x.Logradouro).HasColumnType("nvarchar(150)").IsRequired();
            entity.Property(x => x.Numero).HasColumnType("int").IsRequired();
            entity.Property(x => x.Complemento).HasColumnType("nvarchar(100)");
            entity.Property(x => x.Bairro).HasColumnType("nvarchar(100)").IsRequired();
            entity.Property(x => x.Uf).HasColumnType("nvarchar(2)").IsRequired();
            entity.Property(x => x.Cidade).HasColumnType("nvarchar(50)").IsRequired();
            entity.Property(x => x.Cep).HasColumnType("int").IsRequired();

            // Relações
            entity.HasMany(x => x.Adocoes)
                .WithOne(x => x.PontoAdocao);

            entity.HasMany(x => x.Telefones)
                .WithOne(x => x.PontoAdocao);
        });
        #endregion

        #region TELEFONE - Mapeamento
        modelBuilder.Entity<Telefone>(entity =>
        {
            entity.ToTable("Telefone", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.NumeroTelefone).HasColumnType("nvarchar(11)").IsRequired();
            entity.Property(x => x.Responsavel).HasColumnType("nvarchar(60)").IsRequired();
            entity.Property(x => x.AdotanteId).HasColumnType("int").IsRequired();
            entity.Property(x => x.DoadorId).HasColumnType("int").IsRequired();
            entity.Property(x => x.PontoAdocaoId).HasColumnType("int").IsRequired();

            // Relações
            entity.HasOne(x => x.Adotante)
                .WithMany(x => x.Telefones)
                .HasForeignKey(x => x.AdotanteId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.PontoAdocao)
                .WithMany(x => x.Telefones)
                .HasForeignKey(x => x.PontoAdocaoId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(x => x.Doador)
                .WithMany(x => x.Telefones)
                .HasForeignKey(x => x.DoadorId)
                .OnDelete(DeleteBehavior.NoAction);
        });
        #endregion

        #region USUARIO - Mapeamento
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Nome).HasColumnType("nvarchar(60)").IsRequired();
            entity.Property(x => x.Cpf).HasColumnType("nvarchar(11)").IsRequired();
            entity.Property(x => x.Senha).HasColumnType("nvarchar(15)").IsRequired();
            entity.Property(x => x.Nivel).HasColumnType("tinyint").IsRequired();
            entity.Property(x => x.Status).HasColumnType("bit").IsRequired();

            // Relações
            entity.HasMany(x => x.Adocoes)
                .WithOne(x => x.Usuario);
        });
        #endregion
    }

}
