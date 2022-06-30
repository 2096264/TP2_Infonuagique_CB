using RapidAuto.Utilisateurs.API.Interfaces;
using RapidAuto.Utilisateurs.API.Models;
using RapidAuto.Utilisateurs.API.Services;

namespace RapidAuto.Utilisateurs.API.ServicesProxy
{
    public class UtilisateurServicesProxy : IUtilisateurService
    {
        private readonly IAsyncRepository<Utilisateur> _asyncRepository;

        public UtilisateurServicesProxy(IAsyncRepository<Utilisateur> asyncRepository)
        {
            _asyncRepository = asyncRepository;
        }

        public Task Ajouter(Utilisateur Utilisateur)
        {
            return _asyncRepository.AddAsync(Utilisateur);
        }

        public Task Modifier(Utilisateur Utilisateur)
        {
            return _asyncRepository.EditAsync(Utilisateur);
        }

        public Task<Utilisateur> Obtenir(int id)
        {
            return _asyncRepository.GetByIdAsync(id);
        }

        public Task<IEnumerable<Utilisateur>> ObtenirTout()
        {
            return _asyncRepository.ListAsync();
        }

        public Task Supprimer(Utilisateur utilisateur)
        {
            return _asyncRepository.DeleteAsync(utilisateur);
        }
    }
}
