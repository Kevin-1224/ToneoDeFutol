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
    public class InscripcionJugadorsController : ControllerBase
    {
        private readonly ToneoDeFutolApiContext _context;

        public InscripcionJugadorsController(ToneoDeFutolApiContext context)
        {
            _context = context;
        }

        // GET: api/InscripcionJugadors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InscripcionJugador>>> GetInscripcionJugador()
        {
            return await _context.InscripcionJugador.ToListAsync();
        }

        // GET: api/InscripcionJugadors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InscripcionJugador>> GetInscripcionJugador(int id)
        {
            var inscripcionJugador = await _context.InscripcionJugador.FindAsync(id);

            if (inscripcionJugador == null)
            {
                return NotFound();
            }

            return inscripcionJugador;
        }

        // PUT: api/InscripcionJugadors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInscripcionJugador(int id, InscripcionJugador inscripcionJugador)
        {
            if (id != inscripcionJugador.Id)
            {
                return BadRequest();
            }

            _context.Entry(inscripcionJugador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InscripcionJugadorExists(id))
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

        // POST: api/InscripcionJugadors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InscripcionJugador>> PostInscripcionJugador(InscripcionJugador inscripcionJugador)
        {
            _context.InscripcionJugador.Add(inscripcionJugador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInscripcionJugador", new { id = inscripcionJugador.Id }, inscripcionJugador);
        }

        // DELETE: api/InscripcionJugadors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInscripcionJugador(int id)
        {
            var inscripcionJugador = await _context.InscripcionJugador.FindAsync(id);
            if (inscripcionJugador == null)
            {
                return NotFound();
            }

            _context.InscripcionJugador.Remove(inscripcionJugador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InscripcionJugadorExists(int id)
        {
            return _context.InscripcionJugador.Any(e => e.Id == id);
        }
    }
}
