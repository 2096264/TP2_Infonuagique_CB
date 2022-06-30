using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RapidAuto.MVC.Interfaces;
using RapidAuto.MVC.Models.Utilisateurs;
using System.Net;

namespace RapidAuto.MVC.Controllers
{
    public class GestionUtilisateursController : Controller
    {
        private readonly IGestionUtilisateursService _gestionUtilisateursService;
        private readonly ILogger _logger;

        public GestionUtilisateursController(IGestionUtilisateursService gestionUtilisateursService, ILogger<GestionUtilisateursController> logger)
        {
            _gestionUtilisateursService = gestionUtilisateursService;
            _logger = logger;
        }

        // GET: GestionUtilisateursController
        public async Task<ActionResult> Index()
        {
            return View(await _gestionUtilisateursService.ObtenirTout());
        }

        // GET: GestionUtilisateursController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var utilisateur = await _gestionUtilisateursService.ObtenirParId(id);
            return View(utilisateur);
        }

        // GET: GestionUtilisateursController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GestionUtilisateursController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Utilisateur utilisateur)
        {            

            if (ModelState.IsValid)
            {
                //On effectue la requête HTTP
                var response = await _gestionUtilisateursService.Ajouter(utilisateur);

                //On valide si la création a été effectuée correctmement avant de rediriger l'utilisateur
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    _logger.LogInformation($"La création d'un utilisateur à réusis :\n\t- ID: {utilisateur.ID}");
                    return RedirectToAction(nameof(Index));

                }

                //Sinon on affiche le message retourné par le service
                else
                {
                    ModelState.AddModelError("Erreur du service", await response.Content.ReadAsStringAsync());
                }
            }

            return View(utilisateur);

        }

        // GET: GestionUtilisateursController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            _logger.LogInformation($"Accès à la page de modification d'un utilisateur :\n\t- ID: {id}");

            try
            {
                var utilisateur = await _gestionUtilisateursService.ObtenirParId(id);
                return View(utilisateur);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Erreur du service", ex.Message);
            }

            return View();
        }

        // POST: GestionUtilisateursController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _gestionUtilisateursService.Modifier(id, utilisateur);

                //On valide si la création a été effectuée correctmement avant de rediriger l'utilisateur
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    _logger.LogInformation($"La modification de la fiche d'un utilisateur à réusis :\n\t- ID: {utilisateur.ID}");
                    return RedirectToAction(nameof(Index));
                }
                //Sinon on affiche le message retourné par le service
                else
                {
                    _logger.LogInformation($"La modification de la fiche d'un utilisateur à échoué :\n\t- ID: {utilisateur.ID}");
                    ModelState.AddModelError("Erreur du service", await response.Content.ReadAsStringAsync());
                }
            }

            return View(utilisateur);

        }

        // GET: GestionUtilisateursController/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            var utilisateur = await _gestionUtilisateursService.ObtenirParId(id);

            _logger.LogInformation($"Accès à la page de supression d'un utilisateur :\n\t- ID: {id}");

            if (utilisateur == null)
            {
                return NotFound();
            }

            return View(utilisateur);

        }

        // POST: GestionUtilisateursController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.ID)
            {
                return NotFound();
            }
            try
            {
                await _gestionUtilisateursService.Supprimer(id);

                _logger.LogInformation($"Supression d'un utilisateur :\n\t- ID: {utilisateur.ID}");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Erreur du service", ex.Message);
            }

            return View(utilisateur);
            
        }
    }
}
