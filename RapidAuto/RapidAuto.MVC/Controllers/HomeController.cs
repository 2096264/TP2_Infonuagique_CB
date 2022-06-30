using Microsoft.AspNetCore.Mvc;
using RapidAuto.MVC.Interfaces;
using RapidAuto.MVC.Models;
using RapidAuto.MVC.ServicesProxy;
using System.Diagnostics;
using Microsoft.Extensions.Azure;

namespace RapidAuto.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGestionFichiersService _gestionFichier;
        private readonly IStorageServiceHelper _storageServiceHelper;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IGestionFichiersService gestionFichier, IStorageServiceHelper storageServiceHelper, IConfiguration configuration)
        {
            _logger = logger;
            _gestionFichier = gestionFichier;
            _storageServiceHelper = storageServiceHelper;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Sauvegarder()
        {
            return View();
        }
        public async Task<IActionResult> ListerBlobs()
        {
            return View(await _storageServiceHelper.ObtenirNomsFichiersDansBlob(_configuration.GetValue<string>("StorageAccount:NomConteneur")));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public IActionResult CodeStatus(int code)
        {
            CodeStatusViewModel codeStatus = new CodeStatusViewModel();
            codeStatus.IdRequete = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            codeStatus.Code = code;
            
            if (code == 404)
                codeStatus.MessageErreur = "La page demandée introuvable";
            else if (code == 500)
                codeStatus.MessageErreur = "Une erreur interne est survenue";
            else
                codeStatus.MessageErreur = "Une erreur inconnue c'est produite lors de l'exécution de la requête";

            return View(codeStatus);

        }
    }
}