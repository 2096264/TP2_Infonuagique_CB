using Newtonsoft.Json;
using RapidAuto.MVC.Interfaces;
using RapidAuto.MVC.Models.Utilisateurs;
using System.Text;

namespace RapidAuto.MVC.ServicesProxy
{
    public class GestionUtilisateursServiceProxy : IGestionUtilisateursService
    {
        private readonly HttpClient _httpClient;
        private const string _apiUrl = "api/Utilisateur";
        private readonly ILogger<GestionUtilisateursServiceProxy> _logger;


        public GestionUtilisateursServiceProxy(HttpClient httpClient, ILogger<GestionUtilisateursServiceProxy> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> Ajouter(Utilisateur utilisateur)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(utilisateur), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_apiUrl, content);

            _ = LogReponseAPI(response);

            return response;
        }

        public async Task<HttpResponseMessage> Modifier(int id, Utilisateur utilisateur)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(utilisateur), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(_apiUrl + "/" + id, content);

            _ = LogReponseAPI(response);

            return response;
        }

        public async Task<Utilisateur> ObtenirParId(int? id)
        {
            var response = await _httpClient.GetAsync(_apiUrl + "/" + id);

            _ = LogReponseAPI(response);

            return await response.Content.ReadFromJsonAsync<Utilisateur>();
        }

        public async Task<List<Utilisateur>> ObtenirTout()
        {
            var response = await _httpClient.GetAsync(_apiUrl);

            _ = LogReponseAPI(response);

            return await response.Content.ReadFromJsonAsync<List<Utilisateur>>();
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
                _logger.LogError(102, $"Réponse de l'appel de l'API Utilisateur : {contenuReponse}");
            }
            if (code >= 500 && code <= 599)
            {
                _logger.LogCritical(103, $"Réponse de l'appel de l'API Utilisateur : {contenuReponse}");
            }
        }
    }
}
