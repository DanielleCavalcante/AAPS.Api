using AAPS.Api.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AAPS.Api.Models.Enums
{
    public enum TipoPessoaEnum
    {
        Adotante = 1,
        Doador = 2,
        PontoAdocao = 3,
        Voluntario = 4
    }
}