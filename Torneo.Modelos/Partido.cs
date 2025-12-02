using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torneo.Modelos
{
    public class Partido
    {
        // PK
        public int PartidoID { get; set; }

        // FKs
        public int TorneoID { get; set; }
        public int EquipoLocalID { get; set; }
        public int EquipoVisitanteID { get; set; }
        public int? GanadorPenalesID { get; set; } // FK Nullable

        // Atributos y Resultados
        public DateTime FechaHora { get; set; }
        public string Fase { get; set; }
        public int? ResultadoLocal { get; set; }
        public int? ResultadoVisitante { get; set; }
        public int PuntosLocal { get; set; }
        public int PuntosVisitante { get; set; }
        public string EstadoPartido { get; set; }


        public int VersionResultado { get; set; }

        // Propiedades de Navegación (Colecciones y Objetos de Referencia)
        public TorneoTipo Torneo { get; set; }
        public Equipo EquipoLocal { get; set; }
        public Equipo EquipoVisitante { get; set; }
        public Equipo GanadorPenales { get; set; }
    }
}
