using RapidAuto.Fichiers.API.Interfaces;
using Microsoft.Extensions.Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RapidAuto.Fichiers.API.ServicesProxy
{
    public class GestionFichierServiceProxy : IGestionFichiersService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _chemin;
        private readonly BlobServiceClient _blobServiceClient;

        public GestionFichierServiceProxy(IWebHostEnvironment env, BlobServiceClient nomBlob)
        {
            _env = env;
            _chemin = _env.WebRootPath + @"\images\";
            _blobServiceClient = nomBlob;
        }

        public byte[] Obtenir(string nom)
        {
            var path = _chemin + nom;
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            else
            {
                throw new FileNotFoundException();
            }        
        }

        public string Renommer(IFormFile fichier, string codeDuVehicule, string suffixe)
        {
            string nouveauNom = codeDuVehicule + Path.GetExtension(fichier.FileName) + suffixe;
            File.Move(_chemin + fichier.FileName, _chemin + nouveauNom);
            return nouveauNom;
        }

        public async Task Sauvegarder(IFormFile fichier, string nomConteneur)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(nomConteneur);
            var path = _chemin + fichier.FileName;

            using (var stream = new FileStream(path, FileMode.Create))
            {

                await containerClient.UploadBlobAsync(path, stream);
                stream.Flush();
            }
        }

        public bool VerifierExtension(IFormFile fichier, IEnumerable<string> extensionsAcceptees)
        {
            string path = fichier.FileName;
            string extensionAVerifier = Path.GetExtension(path);

            foreach (string extensionAcceptee in extensionsAcceptees)
            {
                if (extensionAVerifier == extensionAcceptee)
                    return true;
            }
            return false;
        }

        public string ObtenirExtension(string nom)
        {
            return Path.GetExtension(_chemin + nom);
        }
    }
}
