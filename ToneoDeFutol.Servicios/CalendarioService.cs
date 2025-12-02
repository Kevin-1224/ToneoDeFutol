using Torneo.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToneoDeFutol.Servicios
{
    public class CalendarioService
    {
        private readonly ToneoDeFutolApiContext _context;
        private const int DIAS_ENTRE_RONDAS = 7; // Una semana entre jornadas/rondas

        public CalendarioService(ToneoDeFutolApiContext context)
        {
            _context = context;
        }

        // Método principal llamado por TorneoService.IniciarTorneo()
        public async Task GenerarYGuardarCalendario(int torneoId)
        {
            var torneo = await _context.Torneo
                                       .FirstOrDefaultAsync(t => t.Id == torneoId);

            var equipos = await _context.InscripcionEquipo
                                        .Where(ie => ie.Id == torneoId)
                                        .Select(ie => ie.EquipoID)
                                        .ToListAsync();

            if (torneo == null || equipos.Count < 8)
                throw new InvalidOperationException("No se puede generar calendario: torneo o equipos insuficientes.");

            List<Partido> partidosGenerados = torneo.TipoTorneo switch
            {
                "Liga" => GenerarLiga(torneoId, equipos, torneo.FechaInicio),
                "Copa" => GenerarCopa(torneoId, equipos, torneo.FechaInicio),
                "Mixto" => GenerarMixto(torneoId, equipos, torneo.FechaInicio),
                _ => throw new ArgumentException($"Tipo de torneo '{torneo.TipoTorneo}' no válido.")
            };

            // Guardar todos los partidos en una única transacción
            _context.Partido.AddRange(partidosGenerados);
            await _context.SaveChangesAsync();
        }

        // -------------------------------------------------------------------
        // 1. GENERACIÓN TIPO "LIGA" (Round-Robin Ida y Vuelta)
        // -------------------------------------------------------------------
        private List<Partido> GenerarLiga(int torneoId, List<int> equipos, DateTime fechaInicio)
        {
            var partidos = new List<Partido>();
            var listaEquipos = new List<int>(equipos);
            DateTime fechaPartido = fechaInicio;

            // Ajuste a número par (añadir BYE si es impar)
            int N = listaEquipos.Count;
            bool esImpar = N % 2 != 0;
            if (esImpar)
            {
                listaEquipos.Add(-1); // -1 simula el descanso (BYE)
                N++;
            }

            int totalRondas = N - 1;

            // Generar Rondas de Ida y Vuelta
            for (int ronda = 1; ronda <= totalRondas; ronda++)
            {
                for (int i = 0; i < N / 2; i++)
                {
                    int equipoLocal = listaEquipos[i];
                    int equipoVisitante = listaEquipos[N - 1 - i];

                    // Solo crear partido si no es un enfrentamiento con el descanso (-1)
                    if (equipoLocal != -1 && equipoVisitante != -1)
                    {
                        // IDA
                        partidos.Add(CrearPartido(torneoId, equipoLocal, equipoVisitante, fechaPartido, $"Jornada {ronda} (Ida)"));

                        // VUELTA (Programada semanas después)
                        partidos.Add(CrearPartido(torneoId, equipoVisitante, equipoLocal, fechaPartido.AddDays(totalRondas * DIAS_ENTRE_RONDAS), $"Jornada {ronda} (Vuelta)"));
                    }
                }

                // Rotar equipos (Algoritmo del Círculo)
                int ultimoEquipo = listaEquipos[N - 1];
                for (int i = N - 1; i > 1; i--)
                {
                    listaEquipos[i] = listaEquipos[i - 1];
                }
                listaEquipos[1] = ultimoEquipo;

                fechaPartido = fechaPartido.AddDays(DIAS_ENTRE_RONDAS); // Avanzar a la siguiente jornada
            }
            return partidos;
        }

        // -------------------------------------------------------------------
        // 2. GENERACIÓN TIPO "COPA" (Eliminación Directa - Solo Primera Ronda)
        // -------------------------------------------------------------------
        private List<Partido> GenerarCopa(int torneoId, List<int> equipos, DateTime fechaInicio)
        {
            // Requerimos que sean 8, 16 o 32. Si no, necesitaríamos lógica de "bye" o ronda previa.
            int numEquipos = equipos.Count;
            if (numEquipos != 8 && numEquipos != 16 && numEquipos != 32)
                throw new InvalidOperationException("El formato Copa requiere 8, 16 o 32 equipos para el fixture simple.");

            // Ordenar aleatoriamente para el sorteo
            var listaEquipos = equipos.OrderBy(e => Guid.NewGuid()).ToList();
            DateTime fechaPartido = fechaInicio;
            string fase = GetFaseEliminacion(numEquipos);
            var partidos = new List<Partido>();

            // Emparejar secuencialmente para la primera ronda
            for (int i = 0; i < numEquipos; i += 2)
            {
                partidos.Add(CrearPartido(torneoId, listaEquipos[i], listaEquipos[i + 1], fechaPartido, fase));
            }

            // Las siguientes rondas (Semifinal, Final) se generan dinámicamente
            // después de que los resultados de esta fase se registran.
            return partidos;
        }

        // -------------------------------------------------------------------
        // 3. GENERACIÓN TIPO "MIXTO" (Fase de Grupos)
        // -------------------------------------------------------------------
        private List<Partido> GenerarMixto(int torneoId, List<int> equipos, DateTime fechaInicio)
        {
            var partidos = new List<Partido>();

            // 1. Dividir en 4 grupos (A, B, C, D)
            var grupos = DividirEnGrupos(equipos, 4);

            DateTime fechaGrupo = fechaInicio;

            // 2. Aplicar Round-Robin (solo IDA, por eficiencia) a cada grupo
            foreach (var grupoEntry in grupos)
            {
                char nombreGrupo = grupoEntry.Key;
                List<int> equiposGrupo = grupoEntry.Value;

                // Generar los partidos dentro del grupo
                var partidosGrupo = GenerarLigaSimple(torneoId, equiposGrupo, fechaGrupo, $"Fase Grupos {nombreGrupo}");
                partidos.AddRange(partidosGrupo);

                // Avanzar la fecha para el siguiente grupo/ronda de grupos
                fechaGrupo = partidosGrupo.Max(p => p.FechaHora).AddDays(DIAS_ENTRE_RONDAS);
            }

            // La fase de Eliminación (Cuartos) se genera dinámicamente al finalizar los grupos.
            return partidos;
        }

        // -------------------------------------------------------------------
        // MÉTODOS AUXILIARES
        // -------------------------------------------------------------------

        private Partido CrearPartido(int torneoId, int localId, int visitanteId, DateTime fecha, string fase)
        {
            return new Partido
            {
                TorneoID = torneoId,
                EquipoLocalID = localId,
                EquipoVisitanteID = visitanteId,
                FechaHora = fecha,
                Fase = fase,
                EstadoPartido = "Programado",
                VersionResultado = 0, // Control de Concurrencia
            };
        }

        private string GetFaseEliminacion(int equiposRestantes)
        {
            return equiposRestantes switch
            {
                32 => "Dieciseisavos de Final",
                16 => "Octavos de Final",
                8 => "Cuartos de Final",
                4 => "Semifinal",
                2 => "Final",
                _ => "Ronda Desconocida"
            };
        }

        private Dictionary<char, List<int>> DividirEnGrupos(List<int> equipos, int numGrupos)
        {
            var grupos = new Dictionary<char, List<int>>();
            var equiposAleatorios = equipos.OrderBy(e => Guid.NewGuid()).ToList();

            for (int i = 0; i < numGrupos; i++)
            {
                grupos[(char)('A' + i)] = new List<int>();
            }

            for (int i = 0; i < equiposAleatorios.Count; i++)
            {
                char grupoKey = (char)('A' + (i % numGrupos));
                grupos[grupoKey].Add(equiposAleatorios[i]);

                // Aquí deberías actualizar el campo GrupoFaseGrupos en InscripcionEquipo en la BD
                // (Opcional, se puede hacer en el BLL principal)
            }
            return grupos;
        }

        // Variante simple de la liga (solo IDA, usada para Fase de Grupos)
        private List<Partido> GenerarLigaSimple(int torneoId, List<int> equipos, DateTime fechaInicio, string faseBase)
        {
            var partidos = new List<Partido>();
            var listaEquipos = new List<int>(equipos);
            DateTime fechaPartido = fechaInicio;

            int N = listaEquipos.Count;
            if (N % 2 != 0)
            {
                listaEquipos.Add(-1);
                N++;
            }

            int totalRondas = N - 1;

            for (int ronda = 1; ronda <= totalRondas; ronda++)
            {
                for (int i = 0; i < N / 2; i++)
                {
                    int equipoLocal = listaEquipos[i];
                    int equipoVisitante = listaEquipos[N - 1 - i];

                    if (equipoLocal != -1 && equipoVisitante != -1)
                    {
                        partidos.Add(CrearPartido(torneoId, equipoLocal, equipoVisitante, fechaPartido, $"{faseBase} - Jornada {ronda}"));
                    }
                }

                // Rotar
                int ultimoEquipo = listaEquipos[N - 1];
                for (int i = N - 1; i > 1; i--)
                {
                    listaEquipos[i] = listaEquipos[i - 1];
                }
                listaEquipos[1] = ultimoEquipo;

                fechaPartido = fechaPartido.AddDays(DIAS_ENTRE_RONDAS);
            }
            return partidos;
        }
    }
}
