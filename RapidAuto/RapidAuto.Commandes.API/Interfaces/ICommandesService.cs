using RapidAuto.Commandes.API.Models;

namespace RapidAuto.Commandes.API.Services
{
    public interface ICommandesService
    {
        Task<IEnumerable<Commande>> ObtenirTout();
        Task<Commande> Obtenir(int id);
        Task Ajouter(Commande commande);
        Task Modifier(Commande commande);
        Task Supprimer(Commande commande);
    }
}
