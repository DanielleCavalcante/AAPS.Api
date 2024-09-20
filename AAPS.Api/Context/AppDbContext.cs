using AAPS.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AAPS.Api.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Doador> Doadores { get; set; }
    public DbSet<Animal> Animais { get; set; }
    public DbSet<AnimalEvento> AnimalEvento { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Adocao> Adocoes { get; set; }
    public DbSet<Animal> Telefones { get; set; }
    public DbSet<Adotante> Adotantes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adotante>(entity =>
        {
            entity.ToTable("Adotante", "dbo");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Nome).HasColumnType("nvarchar(100)").IsRequired();
            entity.Property(x => x.Rg).HasColumnType("nvarchar(100)").IsRequired();
            entity.Property(x => x.Cpf).HasColumnType("nvarchar(20)").IsRequired();
            entity.Property(x => x.LocalTrabalho).HasColumnType("nvarchar(200)").IsRequired();
            entity.Property(x => x.Status).HasColumnType("bigint").IsRequired();
            entity.Property(x => x.Facebook).HasColumnType("nvarchar(50)").IsRequired();
            entity.Property(x => x.Instagram).HasColumnType("nvarchar(50)").IsRequired();

            entity.Property(x => x.Logradouro).HasColumnType("nvarchar(150)").IsRequired();
            entity.Property(x => x.Numero).HasColumnType("tinyint").IsRequired();
            entity.Property(x => x.Complemento).HasColumnType("nvarchar(100)").IsRequired();
            entity.Property(x => x.Bairro).HasColumnType("nvarchar(100)").IsRequired();
            entity.Property(x => x.Uf).HasColumnType("nvarchar(10)").IsRequired();
            entity.Property(x => x.Cidade).HasColumnType("nvarchar(50)").IsRequired();
            entity.Property(x => x.Cep).HasColumnType("bigint").IsRequired();

            #region RELAÇÕES

            entity.HasMany(d => d.Telefones).WithOne(f => f.Adotante);

            #endregion
        });

    }
}
