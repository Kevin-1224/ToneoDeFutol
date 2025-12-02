using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torneo.Modelos
{
    public class Jugador
    {
        [Key] public int Id { get; set; }
        public string Nombre { get; set; }
        public string Posicion { get; set; }
        public LinkedList<InscripcionJugador?> InscripcionesEquipos { get; set; } = new LinkedList<InscripcionJugador?>();
        public List<EventoPartido> EventosRegistrados { get; set; } = new List<EventoPartido>();
    }
}
