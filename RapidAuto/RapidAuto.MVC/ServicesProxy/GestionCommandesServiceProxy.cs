using Newtonsoft.Json;
using RapidAuto.MVC.Models.Commandes;
using System.Text;
using RapidAuto.MVC.Interfaces;

namespace RapidAuto.MVC.ServicesProxy
{
    public class GestionCommandesServiceProxy : IGestionCommandesService
    {
        private readonly HttpClient _httpClient;
        private const string _apiUrl = "api/Commandes";
        private readonly ILogger<GestionCommandesServiceProxy> _logger;

        public GestionCommandesServiceProxy(HttpClient httpClient, ILogger<GestionCommandesServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> Ajouter(Commande commande)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(commande), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_apiUrl, content);

            _ = LogReponseAPI(response);

            return response;
        }

        public async Task<HttpResponseMessage> Modifier(int id, Commande commande)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(commande), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(_apiUrl + "/" + id, content);

            _ = LogReponseAPI(response);

            return response;
        }

        public async Task<Commande> ObtenirParId(int? id)
        {
            var response = await _httpClient.GetAsync(_apiUrl + "/" + id);

            _ = LogReponseAPI(response);

            return await response.Content.ReadFromJsonAsync<Commande>();
        }

        public async Task<List<Commande>> ObtenirTout()
        {
            var response = await _httpClient.GetAsync(_apiUrl);

            _ = LogReponseAPI(response);

            return await response.Content.ReadFromJsonAsync<List<Commande>>();
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
                _logger.LogError(102, $"Réponse de l'appel de l'API GenerateurCodes : {contenuReponse}");
            }
            if (code >= 500 && code <= 599)
            {
                _logger.LogCritical(103, $"Réponse de l'appel de l'API Commande : {contenuReponse}");
            }
        }
    }
}
