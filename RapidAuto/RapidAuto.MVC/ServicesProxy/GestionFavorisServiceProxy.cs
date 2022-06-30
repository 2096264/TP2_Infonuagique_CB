using Newtonsoft.Json;
using RapidAuto.MVC.Interfaces;
using RapidAuto.MVC.Models.Favoris;
using System.Text;

namespace RapidAuto.MVC.ServicesProxy
{
    public class GestionFavorisServiceProxy : IGestionFavorisService
    {
        private readonly HttpClient _httpClient;
        private const string _apiUrl = "api/Favoris";
        private readonly ILogger<GestionFavorisServiceProxy> _logger;

        public GestionFavorisServiceProxy(HttpClient httpClient, ILogger<GestionFavorisServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> Ajouter(EntFavoris favoris)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(favoris), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_apiUrl, content);

            _ = LogReponseAPI(response);

            return response;
        }

        public async Task<EntFavoris> ObtenirParId(int? id)
        {
            var response = await _httpClient.GetAsync(_apiUrl + "/" + id);

            _ = LogReponseAPI(response);

            return await response.Content.ReadFromJsonAsync<EntFavoris>();
        }

        public async Task<List<EntFavoris>> ObtenirTout(string adresseIp)
        {
            var response = await _httpClient.GetAsync(_apiUrl + "/" + adresseIp);

            _ = LogReponseAPI(response);

            return await response.Content.ReadFromJsonAsync<List<EntFavoris>>();
        }

        public async Task<HttpResponseMessage> Supprimer(int idVehicule, string adresseIp)
        {
            var response = await _httpClient.DeleteAsync(_apiUrl + "/" + idVehicule + "/" + adresseIp);

            _ = LogReponseAPI(response);

            return response;
        }

        private async Task LogReponseAPI(HttpResponseMessage reponse)
        {
            var code = (int)reponse.StatusCode;
            var contenuReponse = await reponse.Content.ReadAsStringAsync();

            if (code >= 400 && code <= 499)
            {
                _logger.LogError(102, $"Réponse de l'appel de l'API Utilisateur : {contenuReponse}");
            }
            if (code >= 500 && code <= 599)
            {
                _logger.LogCritical(103, $"Réponse de l'appel de l'API Utilisateur : {contenuReponse}");
            }
        }
    }
}
