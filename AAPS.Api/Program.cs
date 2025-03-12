using AAPS.Api.Context;
using AAPS.Api.Services.Impl;
using AAPS.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AAPS.Api", Version = "v1" });

    // Habilitar autorização usando Swagger (JWT)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then in the text input below. \r\n\r\nExemple: \"Bearer 12345abcdef\"",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// REGISTRO DE SERVICES
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IDoadorService, DoadorService>();
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddScoped<IPerfilAcessoService, PerfilAcessoService>();
builder.Services.AddScoped<IVoluntarioService, VoluntarioService>();
builder.Services.AddSingleton<EmailService>();

// configurações para o banco DbAaps
var businessConnectionString = builder.Configuration.GetConnectionString("DbAaps");

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(businessConnectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// configuração do Toker JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });


builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// **** IMPORTANTE INDICAR ESTE SERVICE PARA LIDAR COM TODOS OS JOINS DA API
//builder.Services.AddControllers().AddJsonOptions(x =>
//   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

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

app.UseCors("Cors");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();