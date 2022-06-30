using Newtonsoft.Json;
using RapidAuto.MVC.Interfaces;
using RapidAuto.MVC.Models;
using System.Text;

namespace RapidAuto.MVC.ServicesProxy
{
    public class GestionFichiersServiceProxy : IGestionFichiersService
    {
        private readonly HttpClient _httpClient;
        private const string _gestionFichiersUrl = "api/GestionFichiers";
        private readonly ILogger<GestionFichiersServiceProxy> _logger;

        public GestionFichiersServiceProxy(HttpClient httpClient, ILogger<GestionFichiersServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<EntGestionFichiers> Obtenir(string nom)
        {
            var response = await _httpClient.GetAsync(_gestionFichiersUrl + "/" + nom);

            _ = LogReponseAPI(response);

            return await response.Content.ReadFromJsonAsync<EntGestionFichiers>();
        }

        public async Task<HttpResponseMessage> RenommerEtSauvegarder(EntGestionFichiers entGestionFichiers)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(entGestionFichiers), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_gestionFichiersUrl, content);

            _ = LogReponseAPI(response);

            return response;
        }

        private async Task LogReponseAPI(HttpResponseMessage reponse)
        {
            var code = (int)reponse.StatusCode;
            var contenuReponse = await reponse.Content.ReadAsStringAsync();

            if (code >= 400 && code <= 499)
            {
                _logger.LogError(102, $"Réponse de l'appel de l'API Fichier : {contenuReponse}");
            }
            if (code >= 500 && code <= 599)
            {
                _logger.LogCritical(103, $"Réponse de l'appel de l'API Fichier : {contenuReponse}");

            }
        }
    }
}
