﻿using AAPS.Api.Models.Enums;

namespace AAPS.Api.Dtos.Evento
{
    public class FiltroEventoDto
    {
        public string? Busca { get; set; }
        public StatusEnum? Status { get; set; }
    }
}