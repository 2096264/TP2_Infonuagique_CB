using RapidAuto.Vehicules.API.Interfaces;
using RapidAuto.Vehicules.API.Models;

namespace RapidAuto.Vehicules.API.ServicesProxy
{
    public class VehiculeServicesProxy : IVehiculeService
    {
        private readonly IAsyncRepository<Vehicule> _asyncRepository;

        public VehiculeServicesProxy(IAsyncRepository<Vehicule> asyncRepository)
        {
            _asyncRepository = asyncRepository;
        }

        public Task Ajouter(Vehicule vehicule)
        {
            return _asyncRepository.AddAsync(vehicule);
        }

        public Task Modifier(Vehicule vehicule)
        {
            return _asyncRepository.EditAsync(vehicule);
        }

        public Task<Vehicule> Obtenir(int id)
        {
            return _asyncRepository.GetByIdAsync(id);
        }

        public Task<IEnumerable<Vehicule>> ObtenirTout()
        {
            return _asyncRepository.ListAsync();
        }

        public Task Supprimer(Vehicule vehicule)
        {
            return _asyncRepository.DeleteAsync(vehicule);
        }
    }
}
