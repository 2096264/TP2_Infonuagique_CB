using RapidAuto.MVC.Models;

namespace RapidAuto.MVC.Interfaces
{
    public interface IGestionVehiculesService
    {
        Task<List<Vehicule>> ObtenirTout();
        Task<List<Vehicule>> ObtenirTout(string chaineDeRecherche, string filtreDeRecherche);
        Task<Vehicule> ObtenirParId(int? id);
        Task<HttpResponseMessage> Ajouter(Vehicule vehicule);
        Task<HttpResponseMessage> Modifier(int id, Vehicule vehicule);
        Task<HttpResponseMessage> Supprimer(int id);
    }
}
