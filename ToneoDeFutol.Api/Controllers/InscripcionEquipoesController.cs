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
    public class InscripcionEquipoesController : ControllerBase
    {
        private readonly ToneoDeFutolApiContext _context;

        public InscripcionEquipoesController(ToneoDeFutolApiContext context)
        {
            _context = context;
        }

        // GET: api/InscripcionEquipoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InscripcionEquipo>>> GetInscripcionEquipo()
        {
            return await _context.InscripcionEquipo.ToListAsync();
        }

        // GET: api/InscripcionEquipoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InscripcionEquipo>> GetInscripcionEquipo(int id)
        {
            var inscripcionEquipo = await _context.InscripcionEquipo.FindAsync(id);

            if (inscripcionEquipo == null)
            {
                return NotFound();
            }

            return inscripcionEquipo;
        }

        // PUT: api/InscripcionEquipoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInscripcionEquipo(int id, InscripcionEquipo inscripcionEquipo)
        {
            if (id != inscripcionEquipo.Id)
            {
                return BadRequest();
            }

            _context.Entry(inscripcionEquipo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InscripcionEquipoExists(id))
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

        // POST: api/InscripcionEquipoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InscripcionEquipo>> PostInscripcionEquipo(InscripcionEquipo inscripcionEquipo)
        {
            _context.InscripcionEquipo.Add(inscripcionEquipo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInscripcionEquipo", new { id = inscripcionEquipo.Id }, inscripcionEquipo);
        }

        // DELETE: api/InscripcionEquipoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInscripcionEquipo(int id)
        {
            var inscripcionEquipo = await _context.InscripcionEquipo.FindAsync(id);
            if (inscripcionEquipo == null)
            {
                return NotFound();
            }

            _context.InscripcionEquipo.Remove(inscripcionEquipo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InscripcionEquipoExists(int id)
        {
            return _context.InscripcionEquipo.Any(e => e.Id == id);
        }
    }
}
