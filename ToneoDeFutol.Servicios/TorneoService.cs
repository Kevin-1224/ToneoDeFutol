using Torneo.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToneoDeFutol.Servicios
{
    public class TorneoService
    {
        private readonly ToneoDeFutolApiContext _context;
        // Asumimos que tienes un CalendarioService para generar los partidos
        private readonly CalendarioService _calendarioService;

        // Constructor: Usa inyección de dependencias para el DbContext
        public TorneoService(ToneoDeFutolApiContext context)
        {
            _context = context;
            // Debes inicializar o inyectar CalendarioService aquí
            _calendarioService = new CalendarioService(context);
        }

        // -------------------------------------------------------------------
        // RESTRICCIÓN 1: INSCRIPCIÓN DE EQUIPOS (Máximo 32 y Estado del Torneo)
        // -------------------------------------------------------------------
        public async Task<bool> InscribirEquipo(int torneoId, int equipoId)
        {
            var torneo = await _context.TorneoTipo
                                       .Include(t => t.InscripcionesEquipos)
                                       .FirstOrDefaultAsync(t => t.TorneoID == torneoId);

            if (torneo == null)
                throw new ArgumentException("Torneo no encontrado.");

            // REGLA: NO se pueden inscribir más equipos una vez iniciado.
            if (torneo.Estado != "Creado")
                throw new InvalidOperationException("El torneo ya ha comenzado, no se permiten nuevas inscripciones.");

            // REGLA: Máximo 32 equipos por torneo.
            if (torneo.InscripcionesEquipos.Count >= 32)
                throw new InvalidOperationException("Se ha alcanzado el límite de 32 equipos inscritos.");

            // Opcional: Validar si el equipo ya está inscrito (para evitar duplicados lógicos)
            if (torneo.InscripcionesEquipos.Any(ie => ie.EquipoID == equipoId))
                throw new InvalidOperationException("El equipo ya está inscrito en este torneo.");

            // Opcional: Implementar validación de Jugador en múltiples equipos (se haría al inscribir al jugador)
            // Lógica de inscripción (se asume que el Equipo ya existe)
            var inscripcion = new InscripcionEquipo
            {
                TorneoID = torneoId,
                EquipoID = equipoId,
                FechaInscripcion = DateTime.Now
            };
            _context.InscripcionEquipo.Add(inscripcion);

            await _context.SaveChangesAsync();
            return true;
        }

        // -------------------------------------------------------------------
        // RESTRICCIÓN 2: INICIO DEL TORNEO (Mínimo de 8 Equipos y Calendario)
        // -------------------------------------------------------------------
        public async Task<bool> IniciarTorneo(int torneoId)
        {
            var torneo = await _context.TorneTipo
                                       .Include(t => t.InscripcionesEquipos)
                                       .FirstOrDefaultAsync(t => t.TorneoID == torneoId);

            if (torneo == null) throw new ArgumentException("Torneo no encontrado.");

            if (torneo.Estado != "Creado")
                throw new InvalidOperationException("El torneo ya está iniciado o finalizado.");

            // REGLA: Un torneo NO puede iniciarse si tiene menos de 8 equipos.
            if (torneo.InscripcionesEquipos.Count < 8)
                throw new InvalidOperationException($"El torneo no puede iniciar, requiere un mínimo de 8 equipos. Actualmente tiene {torneo.InscripcionesEquipos.Count}.");

            // Requisito: Generar Calendario Automático antes de iniciar
            bool calendarioExiste = await _context.Partido.AnyAsync(p => p.TorneoID == torneoId);

            if (!calendarioExiste)
            {
                // Llama al servicio que contiene la lógica Round-Robin/Eliminación
                await _calendarioService.GenerarYGuardarCalendario(torneoId);
            }

            // Cambiar estado y guardar
            torneo.Estado = "Iniciado";
            await _context.SaveChangesAsync();
            return true;
        }

        // -------------------------------------------------------------------
        // RESTRICCIÓN 3: REGISTRO DE RESULTADOS (Validación de Partidos y Concurrencia)
        // -------------------------------------------------------------------
        public
