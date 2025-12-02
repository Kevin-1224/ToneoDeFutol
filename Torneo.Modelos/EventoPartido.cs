using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torneo.Modelos
{
    public class EventoPartido
    {
        // PK
        [Key] public int Id { get; set; }

        // FKs
        public int PartidoID { get; set; }
        public int JugadorID { get; set; }
        public int EquipoID { get; set; }

        // Atributos
        public string TipoEvento { get; set; } // "Gol", "Amarilla", "Roja"
        public int Minuto { get; set; }


        public Partido Partido { get; set; }
        public Jugador Jugador { get; set; }
        public Equipo Equipo { get; set; }
    }
}
