using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RapidAuto.MVC.Interfaces;
using RapidAuto.MVC.Models.Favoris;
using System.Net;

namespace RapidAuto.MVC.Controllers
{
    public class GestionFavorisController : Controller
    {
        private readonly IGestionFavorisService _gestionFavorisService;
        private readonly IGestionVehiculesService _gestionVehiculesService;

        public GestionFavorisController(IGestionFavorisService gestionFavorisService, IGestionVehiculesService gestionVehiculesService)
        {
            _gestionFavorisService = gestionFavorisService;
            _gestionVehiculesService = gestionVehiculesService;
        }

        // GET: GestionFavorisController
        public async Task<ActionResult> Index()
        {
            string adresseIp = GetIpAddress();

            //Liste des favoris appartenant à cette adresse ip
            var listeDesFavoris = await _gestionFavorisService.ObtenirTout(adresseIp);

            if(listeDesFavoris.Count == 0)
            {
                ViewBag.MessageFavorisVide = "Votre liste de favoris est vide. Vous pouvez en ajouter à partir de notre catalogue de véhicules";
            }

            //Liste de tous véhicules
            var listeDesVehicules = await _gestionVehiculesService.ObtenirTout();

            //Jointure entre les favoris et les véhicules pour permettre l'affichage
            var listeDesFavorisPourAfficher = from favoris in listeDesFavoris
                                              join vehicules in listeDesVehicules
                                              on favoris.VehiculeID equals vehicules.ID
                                              select new FavorisIndex()
                                              {
                                                  VehiculeID = vehicules.ID,
                                                  NomImage1 = vehicules.NomImage1,
                                                  Description = vehicules.Description
                                              };

            return View(listeDesFavorisPourAfficher);
        }

        public async Task<ActionResult> AjouterAuxFavoris(int idVehicule)
        {
            //Le favori qui sera enregistré
            EntFavoris favori = new EntFavoris() { AdresseIP = GetIpAddress(), VehiculeID = idVehicule };

            //On effectue la requête HTTP
            var response = await _gestionFavorisService.Ajouter(favori);

            //On valide si la création a été effectuée correctmement avant de rediriger vers l'index
            if (response.StatusCode == HttpStatusCode.Created)
                return RedirectToAction(nameof(Index));

            //Sinon on affiche le message retourné par le service
            else
            {
                ModelState.AddModelError("Erreur du service", await response.Content.ReadAsStringAsync());
                return NotFound();
            }
        }

        public async Task<ActionResult> RetirerDeLaListe(int idVehicule)
        {
            try
            {
                //On effectue la requête HTTP
                await _gestionFavorisService.Supprimer(idVehicule, GetIpAddress()); ;

                return RedirectToAction(nameof(Index));
            }
            catch (HttpRequestException httpRequestException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, httpRequestException.Message);
            }
        }

        private string GetIpAddress()
        {

            return Request.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
