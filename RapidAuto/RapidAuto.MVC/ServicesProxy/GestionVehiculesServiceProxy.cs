using Newtonsoft.Json;
using RapidAuto.MVC.Interfaces;
using RapidAuto.MVC.Models;
using System.Net;
using System.Text;

namespace RapidAuto.MVC.ServicesProxy
{
    public class GestionVehiculesServiceProxy : IGestionVehiculesService
    {
        private readonly HttpClient _httpClient;
        private const string _apiUrl = "api/Vehicules";
        private readonly ILogger<GestionVehiculesServiceProxy> _logger;


        public GestionVehiculesServiceProxy(HttpClient httpClient, ILogger<GestionVehiculesServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> Ajouter(Vehicule vehicule)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(vehicule), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_apiUrl, content);

            _ = LogReponseAPI(response);

            return response;

        }

        public async Task<HttpResponseMessage> Modifier(int id, Vehicule vehicule)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(vehicule), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(_apiUrl + "/" + id, content);

            _ = LogReponseAPI(response);

            return response;
        }

        public async Task<Vehicule> ObtenirParId(int? id)
        {
            var response = await _httpClient.GetAsync(_apiUrl + "/" + id);

            _ = LogReponseAPI(response);

            return await response.Content.ReadFromJsonAsync<Vehicule>();
        }
        public async Task<List<Vehicule>> ObtenirTout()
        {
            var response = await _httpClient.GetAsync(_apiUrl);

            _ = LogReponseAPI(response);

            return await response.Content.ReadFromJsonAsync<List<Vehicule>>();
        }

        public async Task<List<Vehicule>> ObtenirTout(string chaineDeRecherche, string filtreDeRecherche)
        {
            var response = await _httpClient.GetAsync(_apiUrl + "/" + chaineDeRecherche + "/" + filtreDeRecherche);

            _ = LogReponseAPI(response);

            return await response.Content.ReadFromJsonAsync<List<Vehicule>>();
        }

        public async Task<HttpResponseMessage> Supprimer(int id)
        {
            var response = await _httpClient.DeleteAsync(_apiUrl + "/" + id);

            _ = LogReponseAPI(response);

            return response;
        }

        private async Task LogReponseAPI(HttpResponseMessage reponse)
        {
            var code = (int)reponse.StatusCode;
            var contenuReponse = await reponse.Content.ReadAsStringAsync();

            if (code >= 400 && code <= 499)
            {
                _logger.LogError(102, $"Réponse de l'appel de l'API Vehicule : {contenuReponse}");
            }
            if (code >= 500 && code <= 599)
            {
                _logger.LogCritical(103, $"Réponse de l'appel de l'API Vehicule : {contenuReponse}");
            }
        }
    }
}
