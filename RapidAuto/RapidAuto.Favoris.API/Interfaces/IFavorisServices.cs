using Microsoft.AspNetCore.Mvc;
using RapidAuto.Favoris.API.Models;

namespace RapidAuto.Favoris.API.Interfaces
{
    public interface IFavorisServices
    {
        IEnumerable<EntFavoris> ObtenirTout(string adresseIp);
        void Ajouter(EntFavoris favoris);
        void Supprimer(int id, string adresseIp);
    }
}
