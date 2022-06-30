using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RapidAuto.MVC.Helpers;
using RapidAuto.MVC.Interfaces;
using RapidAuto.MVC.Models;
using System.Diagnostics;
using System.Net;

namespace RapidAuto.MVC.Controllers
{
    public class GestionVehiculesController : Controller
    {
        private readonly IGestionVehiculesService _gestionVehiculesService;
        private readonly IGestionFichiersService _gestionFichiersService;
        private readonly VehiculesHelper _vehiculesHelper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GestionVehiculesController> _logger;

        public GestionVehiculesController(IGestionVehiculesService gestionVehiculesService, IGestionFichiersService gestionFichiersService, IConfiguration configuration, ILogger<GestionVehiculesController> logger)
        {
            _gestionVehiculesService = gestionVehiculesService;
            _gestionFichiersService = gestionFichiersService;
            _vehiculesHelper = new VehiculesHelper();
            _configuration = configuration;
            _logger = logger;
        }

        // GET: GestionVehiculesController
        public async Task<ActionResult> Index(string chaineDeRecherche, string filtreDeRecherche, string triConstructeur, bool prixAsc, bool prixDesc)
        {
            ViewBag.SelectListFiltreRecherche = _vehiculesHelper.GenererSelectListPourLaRecherche(filtreDeRecherche);
            ViewBag.SelectListConstructeurs = _vehiculesHelper.GenererSelectListDeConstructeurs(await _gestionVehiculesService.ObtenirTout(), triConstructeur);

            ViewBag.FiltreDeRecherche = filtreDeRecherche;
            ViewBag.ChaineDeRecherche = chaineDeRecherche;
            ViewBag.triConstructeur = triConstructeur;
            ViewBag.prixAsc = prixAsc;
            ViewBag.prixDesc = prixDesc;

            List<Vehicule> vehicules;

            if (string.IsNullOrEmpty(chaineDeRecherche) || string.IsNullOrEmpty(filtreDeRecherche))
            {
                vehicules = await _gestionVehiculesService.ObtenirTout();
            }

            else
            {
                vehicules = await _gestionVehiculesService.ObtenirTout(chaineDeRecherche, filtreDeRecherche);

                _logger.LogInformation($"Recherche effectuer selon {filtreDeRecherche} : {chaineDeRecherche}");
            }

            if (triConstructeur != null)
            {
                vehicules = vehicules.Where(v => v.Constructeur == triConstructeur).ToList();
            }

            if (prixAsc)
                vehicules = vehicules.OrderBy(v => v.Prix).ToList();

            if (prixDesc)
                vehicules = vehicules.OrderByDescending(v => v.Prix).ToList();


            return View(vehicules);
        }

        // GET: GestionVehiculesController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var vehicule = await _gestionVehiculesService.ObtenirParId(id);

            _logger.LogInformation($"Consultation de la fiche du vehicule :\n\t- ID: {vehicule.ID}\n\t- NIV: {vehicule.Niv}");

            return View(vehicule);
        }

        // GET: GestionVehiculesController/Create
        public ActionResult Create()
        {
            ViewBag.Types = RecupererType();

            return View();
        }

        // POST: GestionVehiculesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Vehicule vehicule, IFormFile image1, IFormFile image2)
        {
            ViewBag.Types = RecupererType();

            if(image1 == null || image2 == null)
            {
                ViewBag.ImageNulle = "Veuillez renseigner une image";
                return View(vehicule);
            }

            if (!_gestionVehiculesService.ObtenirTout().Result.Any(x => x.Niv == vehicule.Niv))
            {
                vehicule.CodeUnique = _vehiculesHelper.CreerCodeVehicule(vehicule.Modele, vehicule.Niv);

                //Sauvegarde les images du véhicule et modifie les noms d'images qui seront enregistrés au passage.
                vehicule = await SauvegarderImage(vehicule, image1, image2);

                //On effectue la requête HTTP
                var response = await _gestionVehiculesService.Ajouter(vehicule);

                //On valide si la création a été effectuée correctmement avant de rediriger l'utilisateur
                if (response.StatusCode == HttpStatusCode.Created)
                    return RedirectToAction(nameof(Index));

                //Sinon on affiche le message retourné par le service
                else
                {
                    ModelState.AddModelError("Erreur du service", await response.Content.ReadAsStringAsync());
                }
            }
            else
            {
                ModelState.AddModelError("Niv", "Le Niv que vous avez entré est déja associer à un véhicule");
            }

            _logger.LogInformation($"Création de la fiche du vehicule :\n\t- ID: {vehicule.ID}\n\t- NIV: {vehicule.Niv}");

            return View(vehicule);
        }

        // GET: GestionVehiculesController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.Types = RecupererType();       
            
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                return View(await _gestionVehiculesService.ObtenirParId(id));
            }
            catch (Exception ex)
            {
                return NotFound();
            }

        }

        // POST: GestionVehiculesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Vehicule vehicule)
        {
            ViewBag.Types = RecupererType();
            if (id != vehicule.ID)
            {
                return NotFound();
            }

            try
            {
                var response = await _gestionVehiculesService.Modifier(id, vehicule);

                //On valide si la création a été effectuée correctmement avant de rediriger l'utilisateur
                if (response.StatusCode == HttpStatusCode.Created)
                    return RedirectToAction(nameof(Index));
                //Sinon on affiche le message retourné par le service
                else
                {
                    ModelState.AddModelError("Erreur du service", await response.Content.ReadAsStringAsync());
                }

                _logger.LogInformation($"Modification de la fiche du vehicule :\n\t- ID: {vehicule.ID}\n\t- NIV: {vehicule.Niv}");

                return View(vehicule);
            }
            catch
            {
                return View(vehicule);
            }
        }

        // GET: GestionVehiculesController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var vehicule = await _gestionVehiculesService.ObtenirParId(id);

            if (vehicule == null)
            {
                return NotFound();
            }
            
            return View(vehicule);
        }

        // POST: GestionVehiculesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Vehicule vehicule)
        {
            if (id != vehicule.ID)
            {
                return NotFound();
            }
            try
            {
                //Il faudrait d'abord vérifier si le véhicule est dans les favoris d'un utilisateur avant de le supprimer                
                 await _gestionVehiculesService.Supprimer(id);

                _logger.LogInformation($"Supression de la fiche du vehicule :\n\t- ID: {vehicule.ID}\n\t- NIV: {vehicule.Niv}");

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(vehicule);
            }
        }

        public async Task<FileResult> GetImage(string nom)
        {
            EntGestionFichiers entGestionFichier = await _gestionFichiersService.Obtenir(nom);
            byte[] fichierAfficher = Convert.FromBase64String(entGestionFichier.Fichier1);
            string extension = entGestionFichier.ExtensionFichier1;
            return File(fichierAfficher, "image/" + extension);
        }

        private async Task<Vehicule> SauvegarderImage(Vehicule vehicule, IFormFile image1, IFormFile image2)
        {
            EntGestionFichiers gestionFichiers = new EntGestionFichiers() { CodeVehicule = vehicule.CodeUnique, ExtensionFichier1 = Path.GetExtension(image1.FileName), ExtensionFichier2 = Path.GetExtension(image2.FileName), NomFichier1 = image1.FileName, NomFichier2 = image2.FileName };

            //Convertit en format string pour faciliter la requête
            _vehiculesHelper.ImagesToString(ref gestionFichiers, image1, image2);

            //Sauvegarder les images dans le répertoire de l'API et renommer les noms de fichiers
            var responseSauvegardeFichiers = await _gestionFichiersService.RenommerEtSauvegarder(gestionFichiers);

            var reponseEntFichiers = await responseSauvegardeFichiers.Content.ReadFromJsonAsync<EntGestionFichiers>();
            vehicule.NomImage1 = reponseEntFichiers.NomFichier1;
            vehicule.NomImage2 = reponseEntFichiers.NomFichier2;

            return vehicule;
        }

        private List<string> RecupererType()
        {
            return _configuration.GetSection("Vehicule:Type").Get<string[]>().ToList();
        }

    }
}
