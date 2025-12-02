using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Torneo.Modelos;

namespace ToneoDeFutol.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoPartidoesController : ControllerBase
    {
        private readonly ToneoDeFutolApiContext _context;

        public EventoPartidoesController(ToneoDeFutolApiContext context)
        {
            _context = context;
        }

        // GET: api/EventoPartidoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventoPartido>>> GetEventoPartido()
        {
            return await _context.EventoPartido.ToListAsync();
        }

        // GET: api/EventoPartidoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventoPartido>> GetEventoPartido(int id)
        {
            var eventoPartido = await _context.EventoPartido.FindAsync(id);

            if (eventoPartido == null)
            {
                return NotFound();
            }

            return eventoPartido;
        }

        // PUT: api/EventoPartidoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventoPartido(int id, EventoPartido eventoPartido)
        {
            if (id != eventoPartido.Id)
            {
                return BadRequest();
            }

            _context.Entry(eventoPartido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventoPartidoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EventoPartidoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EventoPartido>> PostEventoPartido(EventoPartido eventoPartido)
        {
            _context.EventoPartido.Add(eventoPartido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEventoPartido", new { id = eventoPartido.Id }, eventoPartido);
        }

        // DELETE: api/EventoPartidoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventoPartido(int id)
        {
            var eventoPartido = await _context.EventoPartido.FindAsync(id);
            if (eventoPartido == null)
            {
                return NotFound();
            }

            _context.EventoPartido.Remove(eventoPartido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventoPartidoExists(int id)
        {
            return _context.EventoPartido.Any(e => e.Id == id);
        }
    }
}
