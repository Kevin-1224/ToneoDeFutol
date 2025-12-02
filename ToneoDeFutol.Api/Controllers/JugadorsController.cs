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
    public class JugadorsController : ControllerBase
    {
        private readonly ToneoDeFutolApiContext _context;

        public JugadorsController(ToneoDeFutolApiContext context)
        {
            _context = context;
        }

        // GET: api/Jugadors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jugador>>> GetJugador()
        {
            return await _context.Jugador.ToListAsync();
        }

        // GET: api/Jugadors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Jugador>> GetJugador(int id)
        {
            var jugador = await _context.Jugador.FindAsync(id);

            if (jugador == null)
            {
                return NotFound();
            }

            return jugador;
        }

        // PUT: api/Jugadors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJugador(int id, Jugador jugador)
        {
            if (id != jugador.Id)
            {
                return BadRequest();
            }

            _context.Entry(jugador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JugadorExists(id))
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

        // POST: api/Jugadors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Jugador>> PostJugador(Jugador jugador)
        {
            _context.Jugador.Add(jugador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJugador", new { id = jugador.Id }, jugador);
        }

        // DELETE: api/Jugadors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJugador(int id)
        {
            var jugador = await _context.Jugador.FindAsync(id);
            if (jugador == null)
            {
                return NotFound();
            }

            _context.Jugador.Remove(jugador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JugadorExists(int id)
        {
            return _context.Jugador.Any(e => e.Id == id);
        }
    }
}
