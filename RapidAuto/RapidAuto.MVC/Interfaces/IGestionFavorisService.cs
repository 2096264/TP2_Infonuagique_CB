using RapidAuto.MVC.Models.Favoris;
namespace RapidAuto.MVC.Interfaces
{
    public interface IGestionFavorisService
    {
        Task<List<EntFavoris>> ObtenirTout(string adresseIp);
        Task<EntFavoris> ObtenirParId(int? id);
        Task<HttpResponseMessage> Ajouter(EntFavoris favoris);
        Task<HttpResponseMessage> Supprimer(int idVehicule, string adresseIp);
    }
}
