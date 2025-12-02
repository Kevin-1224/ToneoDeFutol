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
    public class TorneoTipoesController : ControllerBase
    {
        private readonly ToneoDeFutolApiContext _context;

        public TorneoTipoesController(ToneoDeFutolApiContext context)
        {
            _context = context;
        }

        // GET: api/TorneoTipoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TorneoTipo>>> GetTorneo()
        {
            return await _context.Torneo.ToListAsync();
        }

        // GET: api/TorneoTipoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TorneoTipo>> GetTorneoTipo(int id)
        {
            var torneoTipo = await _context.Torneo.FindAsync(id);

            if (torneoTipo == null)
            {
                return NotFound();
            }

            return torneoTipo;
        }

        // PUT: api/TorneoTipoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTorneoTipo(int id, TorneoTipo torneoTipo)
        {
            if (id != torneoTipo.Id)
            {
                return BadRequest();
            }

            _context.Entry(torneoTipo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TorneoTipoExists(id))
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

        // POST: api/TorneoTipoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TorneoTipo>> PostTorneoTipo(TorneoTipo torneoTipo)
        {
            _context.Torneo.Add(torneoTipo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTorneoTipo", new { id = torneoTipo.Id }, torneoTipo);
        }

        // DELETE: api/TorneoTipoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTorneoTipo(int id)
        {
            var torneoTipo = await _context.Torneo.FindAsync(id);
            if (torneoTipo == null)
            {
                return NotFound();
            }

            _context.Torneo.Remove(torneoTipo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TorneoTipoExists(int id)
        {
            return _context.Torneo.Any(e => e.Id == id);
        }
    }
}
