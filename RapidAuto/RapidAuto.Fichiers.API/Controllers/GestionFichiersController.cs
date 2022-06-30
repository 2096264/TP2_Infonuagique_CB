using Microsoft.AspNetCore.Mvc;
using RapidAuto.Fichiers.API.Interfaces;
using RapidAuto.Fichiers.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RapidAuto.Fichiers.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GestionFichiersController : ControllerBase
    {
        private readonly IGestionFichiersService _fichierService;
        private readonly IConfiguration _config;

        public GestionFichiersController(IGestionFichiersService fichierService, IConfiguration config)
        {
            _fichierService = fichierService;
            _config = config;
        }

        /// <summary>
        /// Obtenir un fichier à partir du nom
        /// </summary>
        /// <param name="nom">Le nom du fichier en incluant l'extension. ex.: "nom_du_fichier.ext"</param>
        /// <response code="200" >Fichier obtenu avec succès</response>
        /// <response code="404" >Fichier non trouvé</response>
        /// <response code = "500" > Le service est indisponible pour le moment</response>
        /// <returns>Une entité permettant de reconstruire le fichier</returns>  
        // GET: api/GestionFichiers
        [HttpGet("{nom}")]
        public ActionResult<EntGestionFichiers> Get(string nom)
        {
            try
            {
                byte[] fichier = _fichierService.Obtenir(nom);
                string fichierStringAEnvoyer = Convert.ToBase64String(fichier);
                string extension = _fichierService.ObtenirExtension(nom);
                return new EntGestionFichiers() { Fichier1 = fichierStringAEnvoyer, ExtensionFichier1 = extension};
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// Renommer et sauvegarder les deux fichiers relatifs à un véhicule
        /// </summary>
        /// <param name="requete">La requete contenant les informations à stocker dans l'API</param>
        /// <response code="201">Le fichier a été renommé et sauvegardé avec succès</response>
        /// <response code = "400" >Informations de la demande invalides</response>
        /// <response code = "500" >Le service est indisponible pour le moment. Il se pourrait que le format de fichier ne soit pas accepté</response>
        // POST api/GestionFichiers
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] EntGestionFichiers requete)
        {
            IEnumerable<string> extensionsAcceptees = _config.GetSection("AllowedExtensions").Get<List<string>>();
            string nomConteneur = _config.GetValue<string>("StorageAccount:NomConteneur");
            //On Convertit les fichiers en format byte array
            byte[] Image1Byte = Convert.FromBase64String(requete.Fichier1);
            byte[] Image2Byte = Convert.FromBase64String(requete.Fichier2);
            EntGestionFichiers entiteReponse = new EntGestionFichiers() { CodeVehicule = requete.CodeVehicule };

            //On Convertit le premier fichier en FormFile
            using (var stream = new MemoryStream(Image1Byte))
            {
                var file = new FormFile(stream, 0, Image1Byte.Length, requete.NomFichier1, requete.NomFichier1)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = requete.ExtensionFichier1,
                };

                if(_fichierService.VerifierExtension(file, extensionsAcceptees))
                {
                    await _fichierService.Sauvegarder(file, nomConteneur);
                    entiteReponse.NomFichier1 = _fichierService.Renommer(file, requete.CodeVehicule, "I1");
                }          
            }

            //On Convertit le deuxième fichier en FormFile
            using (var stream = new MemoryStream(Image2Byte))
            {
                var file = new FormFile(stream, 0, Image2Byte.Length, requete.NomFichier2, requete.NomFichier2)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = requete.ExtensionFichier2,
                };
                if(_fichierService.VerifierExtension(file, extensionsAcceptees))
                {
                    await _fichierService.Sauvegarder(file, nomConteneur);
                    entiteReponse.NomFichier2 = _fichierService.Renommer(file, requete.CodeVehicule, "I2");
                }

            }
            //On retourne l'entité réponse afin d'y récupérer les nouveaux noms d'image
            return CreatedAtAction(nameof(Post), entiteReponse);
        }
    }
}
