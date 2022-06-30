using RapidAuto.Commandes.API.Interfaces;
using RapidAuto.Commandes.API.Models;
using RapidAuto.Commandes.API.Services;

namespace RapidAuto.Commandes.API.ServicesProxy
{
    public class CommandesServicesProxy : ICommandesService
    {
        private readonly IAsyncRepository<Commande> _asyncRepository;

        public CommandesServicesProxy(IAsyncRepository<Commande> asyncRepository)
        {
            _asyncRepository = asyncRepository;
        }

        public Task Ajouter(Commande commande)
        {
            return _asyncRepository.AddAsync(commande);
        }

        public Task Modifier(Commande commande)
        {
            return _asyncRepository.EditAsync(commande);
        }

        public Task<Commande> Obtenir(int id)
        {
            return _asyncRepository.GetByIdAsync(id);
        }

        public Task<IEnumerable<Commande>> ObtenirTout()
        {
            return _asyncRepository.ListAsync();
        }

        public Task Supprimer(Commande commande)
        {
            return _asyncRepository.DeleteAsync(commande);
        }
    }
}
