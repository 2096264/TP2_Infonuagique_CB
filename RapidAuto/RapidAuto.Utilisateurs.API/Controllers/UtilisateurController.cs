using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidAuto.Utilisateurs.API.Models;
using RapidAuto.Utilisateurs.API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RapidAuto.Utilisateurs.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateurController : ControllerBase
    {
        private readonly IUtilisateurService _utilisateurService;

        public UtilisateurController(IUtilisateurService utilisateurService)
        {
            _utilisateurService = utilisateurService;
        }

        /// <summary>
        /// Retourne tout les utilisateurs
        /// </summary>
        /// <response code="200">Liste d'utilisateurs retourné avec succès</response>
        /// <response code="404">Liste d'utilisateurs introuvable</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        //// GET: api/<UtilisateurController>
        [HttpGet]
        public async Task<IEnumerable<Utilisateur>> Get()
        {
            return await _utilisateurService.ObtenirTout();
        }

        /// <summary>
        /// Retourne un utilisateur selon son ID
        /// </summary>
        /// <param name="id">id de l'utilisateur à retourner</param>   
        /// <response code="200">Utilisateur trouvé et retourné avec succès</response>
        /// <response code="404">Utilisateur introuvable pour l'id spécifé</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // GET api/<UtilisateurController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilisateur>> Get(int id)
        {
            var utilisateur = await _utilisateurService.Obtenir(id);

            if (utilisateur == null)
            {
                return NotFound();
            }

            return utilisateur;
        }

        /// <summary>
        /// Ajoute un utilisateur à la base de données
        /// </summary>
        /// <param name="utilisateur">Informations de l'utilisateur à enregistrer</param>   
        /// <response code="200">L'utilisateurs à été ajouté avec succès</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // POST api/<UtilisateurController>
        [HttpPost]
        public async Task<ActionResult<Utilisateur>> Post(Utilisateur utilisateur)
        {
            await _utilisateurService.Ajouter(utilisateur);
            return CreatedAtAction(nameof(Post), utilisateur);
        }

        /// <summary>
        /// Modifier un utilisateur selon son ID
        /// </summary>
        /// <param name="id">id de l'utilisateur à modifier</param>
        /// <param name="utilisateur">Informations de l'utilisateur à enregistrer</param>
        /// <response code="200">Utilisateurs trouvé et retourné avec succès</response>
        /// <response code="400">Le ID ne correspond pas au Id de l'utilisateur à modifier</response>
        /// <response code="404">Utilisateurs introuvable pour l'id spécifé</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // PUT api/<UtilisateurController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.ID)
            {
                return BadRequest();
            }

            try
            {
                await _utilisateurService.Modifier(utilisateur);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilisateurExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(Put),utilisateur );
        }

        /// <summary>
        /// Supprimer un utilisateur selon son ID
        /// </summary>
        /// <param name="id">id de l'utilisateur à modifier</param>
        /// <response code="200">Utilisateurs trouvé et supprimé avec succès</response>
        /// <response code="404">Utilisateurs introuvable pour l'id spécifé</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        //// DELETE api/<UtilisateurController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!UtilisateurExist(id))
            {
                return NotFound();

            }
            var utilisateurASupprimer = await _utilisateurService.Obtenir(id);
            await _utilisateurService.Supprimer(utilisateurASupprimer);

            return NoContent();
        }

        private bool UtilisateurExist(int id)
        {
            return _utilisateurService.ObtenirTout().Result.Any(e => e.ID == id);
        }     
    }
}
