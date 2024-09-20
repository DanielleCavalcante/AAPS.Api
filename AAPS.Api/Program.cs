using AAPS.Api.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// configurações para o banco DbAaps
var businessConnectionString =
builder.Configuration.GetConnectionString("DbAaps");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(businessConnectionString));

// configurações para o banco gerenciador de Acessos via Identity
//var identityConnectionString =
//builder.Configuration.GetConnectionString("AcessosIdentity");
//builder.Services.AddDbContext<AcessosDbContext>(options =>
//options.UseSqlServer(identityConnectionString));

//builder.Services.AddAuthorization();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//.AddRoles<IdentityRole>()
//.AddEntityFrameworkStores<AcessosDbContext>();

//builder.Services.AddIdentityApiEndpoints<>();    
//    .AddEntityFrameworkStores<AcessosDbContext>();


// **** IMPORTANTE INDICAR ESTE SERVICE PARA LIDAR COM TODOS OS JOINS DA API
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// ADICIONAR O SERVICE PARA INICIALIZAÇÃO DO CORS - TEM COMO OBJETIVO "HABILITAR" O CROSS-DOMAIN (CRUZAMENTO DE DOMÍNIO DE APLICAÇÕES)
// adicionar as políticas de aceitação de qualquer solicitação de aplicações client/front - a partir de qualquer outro domínio/ambiente

builder.Services.AddCors(
    options =>
    {
        options.AddPolicy("Cors", p =>
        {
            p.AllowAnyHeader()
           .AllowAnyMethod()
           .AllowAnyOrigin();
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();                           
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
