using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torneo.Modelos
{
    public class InscripcionJugador
    {
        [Key] public int Id { get; set; }
        public int EquipoID { get; set; }
        public int JugadorID { get; set; }
        public int TorneoID { get; set; }
        public int NumeroCamiseta { get; set; }

        // Propiedades de Navegación (Objetos de Referencia)
        public Equipo? Equipo { get; set; }
        public Jugador? Jugador { get; set; }
    }
}
