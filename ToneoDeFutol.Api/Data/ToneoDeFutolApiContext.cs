using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Torneo.Modelos;

    public class ToneoDeFutolApiContext : DbContext
    {
        public ToneoDeFutolApiContext (DbContextOptions<ToneoDeFutolApiContext> options)
            : base(options)
        {
        }

        public DbSet<Torneo.Modelos.Equipo> Equipo { get; set; } = default!;

public DbSet<Torneo.Modelos.EventoPartido> EventoPartido { get; set; } = default!;

public DbSet<Torneo.Modelos.InscripcionEquipo> InscripcionEquipo { get; set; } = default!;

public DbSet<Torneo.Modelos.InscripcionJugador> InscripcionJugador { get; set; } = default!;

public DbSet<Torneo.Modelos.Jugador> Jugador { get; set; } = default!;

public DbSet<Torneo.Modelos.Partido> Partido { get; set; } = default!;

public DbSet<Torneo.Modelos.TorneoTipo> Torneo { get; set; } = default!;
    }
