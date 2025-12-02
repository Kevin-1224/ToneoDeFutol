using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torneo.Modelos
{
    public class TorneoTipo
    {

        public int Id { get; set; }

        // Atributos
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TipoTorneo { get; set; } // "Liga", "Copa", "Mixto"
        public string Estado { get; set; } // "Creado", "Iniciado", "Finalizado"

        // Propiedades de Navegación
        public List<InscripcionEquipo> InscripcionesEquipos { get; set; } = new List<InscripcionEquipo>();
        public List<Partido> Partidos { get; set; } = new List<Partido>();
    }
}
