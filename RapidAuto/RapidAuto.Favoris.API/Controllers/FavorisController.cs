using Microsoft.AspNetCore.Mvc;
using RapidAuto.Favoris.API.Interfaces;
using RapidAuto.Favoris.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RapidAuto.Favoris.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavorisController : ControllerBase
    {
        private readonly IFavorisServices _favorisServices;

        public FavorisController(IFavorisServices favorisServices)
        {
            _favorisServices = favorisServices;
        }

        /// <summary>
        /// Retourne tous les favoris appartenant à une adresse Ip quelconque
        /// </summary>
        /// /// <param name="adresseIp">Adresse Ip de l'ordinateur faisant la demande de sa liste de favoris</param> 
        /// <response code="200">Liste des favoris retourné avec succès</response>
        /// <response code="404">Liste des favoris introuvable</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // GET: api/<FavorisController>/adresseIp
        [HttpGet("{adresseIp}")]
        public List<EntFavoris> Get(string adresseIp)
        {
            var favoris = _favorisServices.ObtenirTout(adresseIp);
            
            if (favoris != null)
            {
                return favoris.ToList();
            }

            return null;
        }

        /// <summary>
        /// Ajoute un favori au cache mémoire
        /// </summary>
        /// <param name="favori">Le favori qui sera a enregistrer contenant l'information du véhicule et l'adresse Ip du client</param>   
        /// <response code="200">Favori ajouté avec succès</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // POST api/<FavorisController>
        [HttpPost]
        public IActionResult Post([FromBody] EntFavoris favori)
        {
            
            _favorisServices.Ajouter(favori);
            return CreatedAtAction(nameof(Post), favori);
        }

        /// <summary>
        /// Supprimer un favori d'un client
        /// </summary>
        /// <param name="idVehicule">Id du véhicule qui sera supprimé des favoris du client</param>
        /// <param name="adresseIp">Adresse Ip du client effectuant la demande</param>
        /// <response code="200">Favori trouvé et supprimé avec succès</response>
        /// <response code="400">Ce favori ne semble pas exister</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // DELETE api/<FavorisController>/5
        [HttpDelete("{idVehicule}/{adresseIp}")]
        public ActionResult Delete(int idVehicule, string adresseIp)
        {
            try
            {
                _favorisServices.Supprimer(idVehicule, adresseIp);
                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            
        }
    }
}
