﻿using System.ComponentModel.DataAnnotations;

namespace P1_practica.Models
{
    public class estados_reserva
    {
        [Key]
        public int estado_res_id { get; set; } 
        public string? estado { get; set; }
    }
}
