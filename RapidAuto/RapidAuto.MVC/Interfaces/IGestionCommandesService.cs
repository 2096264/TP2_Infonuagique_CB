using RapidAuto.MVC.Models.Commandes;

namespace RapidAuto.MVC.Interfaces
{
    public interface IGestionCommandesService
    {
        Task<List<Commande>> ObtenirTout();
        Task<Commande> ObtenirParId(int? id);
        Task<HttpResponseMessage> Ajouter(Commande commande);
        Task<HttpResponseMessage> Modifier(int id, Commande commande);
        Task<HttpResponseMessage> Supprimer(int id);
    }
}
