using RapidAuto.MVC.Models.Utilisateurs;

namespace RapidAuto.MVC.Interfaces
{
    public interface IGestionUtilisateursService
    {
        Task<List<Utilisateur>> ObtenirTout();
        Task<Utilisateur> ObtenirParId(int? id);
        Task<HttpResponseMessage> Ajouter(Utilisateur utilisateur);
        Task<HttpResponseMessage> Modifier(int id, Utilisateur utilisateur);
        Task<HttpResponseMessage> Supprimer(int id);
    }
}
