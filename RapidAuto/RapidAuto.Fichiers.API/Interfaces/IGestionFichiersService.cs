namespace RapidAuto.Fichiers.API.Interfaces
{
    public interface IGestionFichiersService
    {
        public Task Sauvegarder(IFormFile fichier, string nomConteneur);

        public byte[] Obtenir(string nom);

        public string Renommer(IFormFile fichier, string codeDuVehicule, string suffixe);

        public bool VerifierExtension(IFormFile fichier, IEnumerable<string> extensionsAcceptees);

        public string ObtenirExtension(string nom);
    }
}
