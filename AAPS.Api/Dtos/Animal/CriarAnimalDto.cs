﻿using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Animal
{
    public class CriarAnimalDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string Raca { get; set; } = string.Empty;
        public string Pelagem { get; set; } = string.Empty;
        public string Sexo { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public StatusEnum Status { get; set; } = StatusEnum.Ativo;
        public DisponibilidadeEnum Disponibilidade { get; set; } = DisponibilidadeEnum.Disponivel;
        public bool Resgatado { get; set; } = false;
        public int DoadorId { get; set; } = 0;
    }
}