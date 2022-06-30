#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidAuto.Commandes.API.Data;
using RapidAuto.Commandes.API.Models;
using RapidAuto.Commandes.API.Services;

namespace RapidAuto.Commandes.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandesController : ControllerBase
    {
        private readonly ICommandesService _commandeService;

        public CommandesController(ICommandesService commandeService)
        {
            _commandeService = commandeService;
        }

        /// <summary>
        /// Retourne toutes les commandes
        /// </summary>
        /// <response code="200">Liste des commandes retourné avec succès</response>
        /// <response code="404">Liste des commandes introuvable</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // GET: api/Commandes
        [HttpGet]
        public async Task<IEnumerable<Commande>> Get()
        {
            var commades = await _commandeService.ObtenirTout();
            return commades;
        }

        /// <summary>
        /// Retourne une commandes selon son ID
        /// </summary>
        /// <param name="id">id de la commandes à retourner</param>   
        /// <response code="200">Commande trouvé et retourné avec succès</response>
        /// <response code="404">Commande introuvable pour l'id spécifé</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // GET: api/Commandes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Commande>> Get(int id)
        {
            var commande = await _commandeService.Obtenir(id);

            if (commande == null)
            {
                return NotFound();
            }

            return commande;
        }

        /// <summary>
        /// Modifier une commande selon son ID
        /// </summary>
        /// <param name="id">id de la commande à modifier</param>
        /// <param name="commande">Informations de la commande à enregistrer</param>
        /// <response code="200">Commande trouvé et retourné avec succès</response>
        /// <response code="400">Le ID ne correspond pas au Id de la commande à modifier</response>
        /// <response code="404">Utilisateurs introuvable pour l'id spécifé</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // PUT: api/Commandes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Commande commande)
        {
            if (id != commande.ID)
            {
                return BadRequest();
            }

            try
            {
                await _commandeService.Modifier(commande);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommandeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(Put), commande);
        }

        /// <summary>
        /// Ajoute une commande à la base de données
        /// </summary>
        /// <param name="commande">Informations de la commande à enregistrer</param>   
        /// <response code="201">La commande à été ajouté avec succès</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // POST: api/Commandes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Commande>> Post(Commande commande)
        {
            await _commandeService.Ajouter(commande);
            return CreatedAtAction(nameof(Post), commande);
        }

        /// <summary>
        /// Supprimer une commande selon son ID
        /// </summary>
        /// <param name="id">id de la commande à modifier</param>
        /// <response code="200">Commande trouvé et supprimé avec succès</response>
        /// <response code="404">Commande introuvable pour l'id spécifé</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // DELETE: api/Commandes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!CommandeExists(id))
            {
                return NotFound();

            }
            var commandeASupprimer = await _commandeService.Obtenir(id);
            await _commandeService.Supprimer(commandeASupprimer);

            return NoContent();
        }

        private bool CommandeExists(int id)
        {
            return _commandeService.ObtenirTout().Result.Any(e => e.ID == id);
        }
    }
}
