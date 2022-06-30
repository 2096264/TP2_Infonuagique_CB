#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidAuto.Vehicules.API.Data;
using RapidAuto.Vehicules.API.Models;
using RapidAuto.Vehicules.API.Interfaces;

namespace RapidAuto.Vehicules.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculesController : ControllerBase
    {
        private readonly IVehiculeService _vehiculeService;

        public VehiculesController(IVehiculeService vehiculeService)
        {
            _vehiculeService = vehiculeService;
        }

        /// <summary>
        /// Retourne tout les véhicules
        /// </summary>
        /// <response code="200">Liste de véhicule retourné avec succès</response>
        /// <response code="404">Liste de véhicule introuvable</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // GET: api/Vehicules
        [HttpGet]
        public async Task<IEnumerable<Vehicule>> Get()
        {
            return  await _vehiculeService.ObtenirTout();
        }

        /// <summary>
        /// Retourne tout les véhicules
        /// </summary>
        /// <response code="200">Liste de véhicule retourné avec succès</response>
        /// <response code="404">Liste de véhicule introuvable</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // GET: api/Vehicules/{chaineDeRecherche}/{filtreDeRecherche}
        [HttpGet("{chaineDeRecherche}/{filtreDeRecherche}")]
        public async Task<IEnumerable<Vehicule>> Get(string chaineDeRecherche, string filtreDeRecherche)
        {
            var vehicules = await _vehiculeService.ObtenirTout();

            switch (filtreDeRecherche)
            {
                case "constructeur":
                    return vehicules = vehicules.Where(v => v.Constructeur.ToLower().Contains(chaineDeRecherche.ToLower())).ToList();
                case "model":
                    return vehicules = vehicules.Where(v => v.Modele.ToLower().Contains(chaineDeRecherche.ToLower())).ToList();
                case "annee":
                    return vehicules = vehicules.Where(v => v.AnneeFabrication.ToString().ToLower().Contains(chaineDeRecherche.ToLower())).ToList();
                case "niv":
                    return vehicules = vehicules.Where(v => v.Niv.ToLower().Contains(chaineDeRecherche.ToLower())).ToList();
                default:
                    return vehicules;
            }
        }

        /// <summary>
        /// Retourne un véhicule selon son ID
        /// </summary>
        /// <param name="id">id du véhicule à retourner</param>   
        /// <response code="200">Véhicule trouvé et retourné avec succès</response>
        /// <response code="404">Véhicule introuvable pour l'id spécifé</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // GET: api/Vehicules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicule>> Get(int id)
        {
            var vehicule = await _vehiculeService.Obtenir(id);

            if (vehicule == null)
            {
                return NotFound();
            }

            return vehicule;
        }

        /// <summary>
        /// Modifier un véhicule selon son ID
        /// </summary>
        /// <param name="id">id du véhicule à modifier</param>
        /// <param name="vehicule">Informations du véhicule à enregistrer</param>
        /// <response code="200">Véhicule trouvé et retourné avec succès</response>
        /// <response code="400">Le ID ne correspond pas au Id du véhicule à modifier</response>
        /// <response code="404">Véhicule introuvable pour l'id spécifé</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // PUT: api/Vehicules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Vehicule vehicule)
        {
            if (id != vehicule.ID)
            {
                return BadRequest();
            }

            try
            {
                await _vehiculeService.Modifier(vehicule);
                //Retourner l'entité vehicule créée dams la réponse HTTP
                return CreatedAtAction(nameof(Post), vehicule);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehiculeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Ajoute un véhicule à la base de données
        /// </summary>
        /// <param name="vehicule">Informations du véhicule à enregistrer</param>   
        /// <response code="200">Le véhicule à été ajouté avec succès</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // POST: api/Vehicules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vehicule>> Post(Vehicule vehicule)
        {
            if (ModelState.IsValid)
            {
                await _vehiculeService.Ajouter(vehicule);
                //Retourner l'entité vehicule créée dams la réponse HTTP
                return CreatedAtAction(nameof(Post), vehicule);
            }
            //Retouner BadRequest en cas d'une erreur de validation
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Supprimer un utilisateur selon son ID
        /// </summary>
        /// <param name="id">id du véhicule à modifier</param>
        /// <response code="200">Véhicule trouvé et supprimé avec succès</response>
        /// <response code="404">Véhicule introuvable pour l'id spécifé</response>
        /// <response code="500">Oops! le service est indisponible pour le moment</response>
        // DELETE: api/Vehicules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicule(int id)
        {
            if (!VehiculeExists(id))
            {
                return NotFound();
            }

            var vehiculeASupprimer = await _vehiculeService.Obtenir(id);
            await _vehiculeService.Supprimer(vehiculeASupprimer);

            return NoContent();
        }

        private bool VehiculeExists(int id)
        {
            return _vehiculeService.ObtenirTout().Result.Any(x => x.ID == id);
        }
    }
}
