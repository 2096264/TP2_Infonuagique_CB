using RapidAuto.Utilisateurs.API.Models;

namespace RapidAuto.Utilisateurs.API.Services
{
    public interface IUtilisateurService
    {
        Task<IEnumerable<Utilisateur>> ObtenirTout();
        Task<Utilisateur> Obtenir(int id);
        Task Ajouter(Utilisateur Utilisateur);
        Task Modifier(Utilisateur utilisateur);
        Task Supprimer(Utilisateur Utilisateur);
    }
}
