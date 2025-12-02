using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torneo.Modelos
{
    public class InscripcionEquipo
    {

        [Key] public int Id { get; set; }
        public int EquipoID { get; set; }


        public DateTime FechaInscripcion { get; set; }
        public string GrupoFaseGrupos { get; set; }


        public TorneoTipo? Torneo { get; set; }
        public Equipo? Equipo { get; set; }
    }
}
