using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torneo.Modelos
{
    public class Equipo
    {
        [Key] public int Id { get; set; }

        // Atributos
        public string Nombre { get; set; }
        public string Entrenador { get; set; }
        public string Ciudad { get; set; }
        public List<InscripcionJugador?> InscripcionesJugadores { get; set; } = new List<InscripcionJugador?>();
        public List<InscripcionEquipo?> InscripcionesTorneos { get; set; } = new List<InscripcionEquipo?>();

    }
}
