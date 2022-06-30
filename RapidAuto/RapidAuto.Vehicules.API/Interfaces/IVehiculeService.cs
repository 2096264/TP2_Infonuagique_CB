using RapidAuto.Vehicules.API.Models;

namespace RapidAuto.Vehicules.API.Interfaces
{
    public interface IVehiculeService
    {
        Task<IEnumerable<Vehicule>> ObtenirTout();
        Task<Vehicule> Obtenir(int id);
        Task Ajouter(Vehicule vehicule);
        Task Modifier(Vehicule vehicule);
        Task Supprimer(Vehicule vehicule);
    }
}
