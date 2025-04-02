using AAPS.Api.Context;
using AAPS.Api.Middlewares;
using AAPS.Api.Models;
using AAPS.Api.Services;
using AAPS.Api.Services.Acompanhamentos;
using AAPS.Api.Services.Adotantes;
using AAPS.Api.Services.Animais;
using AAPS.Api.Services.Autenticacao;
using AAPS.Api.Services.Doadores;
using AAPS.Api.Services.Eventos;
using AAPS.Api.Services.PontosAdocao;
using AAPS.Api.Services.Voluntarios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// REGISTRO DE SERVICES
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IDoadorService, DoadorService>();
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddScoped<IVoluntarioService, VoluntarioService>();
builder.Services.AddScoped<IAdotanteService, AdotanteService>();
builder.Services.AddScoped<IPontoAdocaoService, PontoAdocaoService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IAcompanhamentoService, AcompanhamentoService>();

builder.Services.AddScoped<EmailService>();

//builder.Services.AddHttpClient<WhatsAppService>();

// configurações para o banco DbAaps
var businessConnectionString = builder.Configuration.GetConnectionString("DbAaps");

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(businessConnectionString));

builder.Services.AddIdentity<Voluntario, IdentityRole<int>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AAPS.Api", Version = "v1" });

    // Habilitar autorização usando Swagger (JWT)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then in the text input below. \r\n\r\nExemple: \"Bearer 12345abcdef\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer", //
        BearerFormat = "JWT", //
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = ClaimTypes.Role,
            //ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// **** IMPORTANTE INDICAR ESTE SERVICE PARA LIDAR COM TODOS OS JOINS DA API
//builder.Services.AddControllers().AddJsonOptions(x =>
//   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

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


app.UseMiddleware<ExcecoesMiddleware>(); // classe para lidar com erros de autenticacao
app.UseMiddleware<AutenticacaoMiddleware>(); // classe para lidar com autenticacao

app.UseCors("Cors");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();