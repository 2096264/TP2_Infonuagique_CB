using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RapidAuto.MVC.Interfaces;
using RapidAuto.MVC.Models.Commandes;
using System.Net;

namespace RapidAuto.MVC.Controllers
{
    public class GestionCommandesController : Controller
    {
        private readonly IGestionCommandesService _gestionCommandesService;
        private readonly IGestionUtilisateursService _gestionUtilisateursService;
        private readonly IGestionVehiculesService _gestionVehiculesService;
        private readonly ILogger<GestionCommandesController> _logger;

        public GestionCommandesController(IGestionCommandesService gestionCommandesService, IGestionUtilisateursService gestionUtilisateursService, IGestionVehiculesService gestionVehiculesService, ILogger<GestionCommandesController> logger)
        {
            _gestionCommandesService = gestionCommandesService;
            _gestionUtilisateursService = gestionUtilisateursService;
            _gestionVehiculesService = gestionVehiculesService;
            _logger = logger;
        }

        // GET: GestionCommandesController
        public async Task<ActionResult> Index()
        {
            return View(await _gestionCommandesService.ObtenirTout());
        }

        // GET: GestionCommandesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var utilisateur = await _gestionCommandesService.ObtenirParId(id);
                return View(utilisateur);

            }
            catch (HttpRequestException httpRequestException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, httpRequestException.Message);
            }
        }

        // GET: GestionCommandesController/Create
        public async Task<ActionResult> Create(int vehicule)
        {
            var vehiculeSelectionne = await _gestionVehiculesService.ObtenirParId(vehicule);
            ViewBag.vehicule = vehiculeSelectionne;
            return View();
        }

        // POST: GestionCommandesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Commande commande)
        {

            var utilisateur = await _gestionUtilisateursService.ObtenirParId(commande.UtilisateurID);

            if (utilisateur.ID == 0)
                ModelState.AddModelError("UtilisateurID", "L'utilisateur entré n'exite pas. Créez-vous un compte pour commander!");

            if (ModelState.IsValid)
            {

                //On effectue la requête HTTP
                var response = await _gestionCommandesService.Ajouter(commande);

                //On valide si la création a été effectuée correctmement avant de rediriger l'utilisateur
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var vehicule = await _gestionVehiculesService.ObtenirParId(commande.VehiculeID);
                    vehicule.EstVendu = true;

                    var responseVehicule = await _gestionVehiculesService.Modifier(vehicule.ID, vehicule);

                    if (responseVehicule.StatusCode == HttpStatusCode.Created)
                    {
                        _logger.LogInformation($"Création de la commande:\n\t- ID: {commande.ID}\n\t- utilisateur: {commande.UtilisateurID}\n\t- vehicule: {commande.VehiculeID}");

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return StatusCode((int)responseVehicule.StatusCode, responseVehicule.Content);
                    }
                }
                //Sinon on affiche le message retourné par le service
                else
                {
                    ModelState.AddModelError("Erreur du service", await response.Content.ReadAsStringAsync());
                }


            }

            var vehiculeSelectionne = await _gestionVehiculesService.ObtenirParId(commande.VehiculeID);
            ViewBag.vehicule = vehiculeSelectionne;
            return View(commande);
        }

        // GET: GestionCommandesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var commande = await _gestionCommandesService.ObtenirParId(id);
                return View(commande);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Erreur du service", ex.Message);
            }

            return View();
        }

        // POST: GestionCommandesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Commande commande)
        {
            if (ModelState.IsValid && id == commande.ID)
            {
                try
                {
                    await _gestionCommandesService.Modifier(id, commande);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }

            }

            return View(commande);

        }

        // GET: GestionCommandesController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var commande = await _gestionCommandesService.ObtenirParId(id);

                if (commande != null)
                {
                    return View(commande);
                }
                else
                {
                    return NotFound();
                }

            }
            catch (HttpRequestException httpRequestException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, httpRequestException.Message);
            }

        }

        // POST: GestionCommandesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Commande commande)
        {
            var commandeProxy = await _gestionCommandesService.ObtenirParId(id);

            if (id == commande.ID && commandeProxy != null)
            {
                try
                {
                    //On effectue la requête HTTP
                    var responseDelete = await _gestionCommandesService.Supprimer(id);

                    if (responseDelete.StatusCode == HttpStatusCode.NoContent)
                    {
                        var vehicule = await _gestionVehiculesService.ObtenirParId(commandeProxy.VehiculeID);
                        vehicule.EstVendu = false;

                        var responseVehicule = await _gestionVehiculesService.Modifier(vehicule.ID, vehicule);

                        if (responseVehicule.StatusCode == HttpStatusCode.Created)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return StatusCode((int)responseVehicule.StatusCode, responseVehicule.Content);
                        }
                    }
                    else
                    {
                        return StatusCode((int)responseDelete.StatusCode, responseDelete.Content);
                    }
                }
                catch (HttpRequestException httpRequestException)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, httpRequestException.Message);
                }

            }
            else
            {
                return View(id);
            }

        }
    }
}
