﻿using Microsoft.EntityFrameworkCore;

namespace P1_practica.Models
{
    public class equiposContext : DbContext
    {
        public equiposContext(DbContextOptions<equiposContext> options) : base(options) {
        
        }

        public DbSet<equipos> equipos { get; set; }
        public DbSet<marcas> marcas { get; set; }
        public DbSet<estados_equipo> estados_equipo { get; set; }
        public DbSet<tipo_equipo> tipo_equipo { get; set; }
        public DbSet<facultades> facultades { get; set; }
        public DbSet<carreras> carreras { get; set; }
        public DbSet<usuarios> usuarios { get; set; }
        public DbSet<reservas> reservas { get; set; }
        public DbSet<estados_reserva> estados_reserva { get; set; }
    }
}
