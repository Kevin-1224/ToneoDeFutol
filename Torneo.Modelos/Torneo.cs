using System.ComponentModel.DataAnnotations;

namespace Torneo.Modelos
{
    public class Torneo
    {

        [Key] public int Id { get; set; }


        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TipoTorneo { get; set; }
        public string Estado { get; set; }
        public List<InscripcionEquipo?> InscripcionesEquipos { get; set; } = new List<InscripcionEquipo?>();
        public List<Partido?> Partidos { get; set; } = new List<Partido?>();




    }
}
