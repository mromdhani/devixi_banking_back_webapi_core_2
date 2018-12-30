using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonProjetBanking_Back.DataAccess;
using MonProjetBanking_Back.Models;

namespace MonProjetBanking_Back.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ComptesController : ControllerBase
    {
        private readonly ComptesContext _context;

        public ComptesController(ComptesContext context)
        {
            _context = context;

            if (_context.Comptes.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.Comptes.AddRange(
                     new Compte { Numero = "C100", Proprietaire = "Hosni", Solde = 100M },
                     new Compte { Numero = "C200", Proprietaire = "Rached", Solde = 200M },
                     new Compte { Numero = "C300", Proprietaire = "Haifa", Solde = 300M },
                     new Compte { Numero = "C400", Proprietaire = "Ameni", Solde = 400M }
                    );
                _context.SaveChanges();
            }
        }
        [HttpGet]
       [Authorize(Policy = "UserAndAdmin")]
        public async Task<ActionResult<IEnumerable<Compte>>> GetComptes()
        {
            return await _context.Comptes.ToListAsync();
        }
        [HttpGet("{id}")]
         [Authorize(Policy = "UserAndAdmin")]
        public async Task<ActionResult<Compte>> GetCompte(string id)
        {
            var compte = await _context.Comptes.FindAsync(id);

            if (compte == null)
            {
                return NotFound();
            }

            return compte;
        }
        // POST: api/Todo
        [HttpPost]
         [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Compte>> PostCompte(Compte compte)
        {
            _context.Comptes.Add(compte);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompte",
                         new { id = compte.Numero }, compte);
        }
        // PUT: api/Todo/5
        [HttpPut("{id}")]
           [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> PutCompte(string id, Compte compte)
        {
            if (id != compte.Numero)
            {
                return BadRequest();
            }

            _context.Entry(compte).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Compte>> DeleteCompte(string id)
        {
            var compte = await _context.Comptes.FindAsync(id);
            if (compte == null)
            {
                return NotFound();
            }

            _context.Comptes.Remove(compte);
            await _context.SaveChangesAsync();

            return compte;
        }
    }
}