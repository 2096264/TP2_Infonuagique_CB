using Microsoft.Extensions.Caching.Memory;
using RapidAuto.Favoris.API.Interfaces;
using RapidAuto.Favoris.API.Models;

namespace RapidAuto.Favoris.API.ServicesProxy
{
    /*
     * Chaque adresse Ip peut avoir une liste de favoris lui appartenant
     * La clée unique de chaque liste de favori est l'adresse Ip de l'ordinateur faisant la demande
     * Pour retrouver un favori appartenant à une adresse Ip, on doit se fier à l'Id du véhicule qui sera unique dans sa liste personalisée
     */
    public class FavorisServicesProxy : IFavorisServices
    {
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _cacheEntryOptions;

        public FavorisServicesProxy(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;

            //Configure la durée de mise en cache
            _cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
            };
        }

        //Retourne vrai si on réussi à ajouter le favoris au cache
        public void Ajouter(EntFavoris favori)
        {
            bool existe = false;
            bool adresseIpContientUneListeDeFavoris = _memoryCache.TryGetValue(favori.AdresseIP, out List<EntFavoris> favoris);
          
            if(adresseIpContientUneListeDeFavoris)
            {
                //Est-ce que ce véhicule a déjà été enregistré dans les favoris de cette personne?
                existe = favoris.Exists(f => f.VehiculeID == favori.VehiculeID);
            }
            else
            {
                //On instancie une nouvelle liste de favoris si cette adresse Ip n'a pas déjà de liste de favoris
                favoris = new List<EntFavoris>();
            }

            //Ajouter le favori dans le cache s'il n'existe pas déjà
            if (!existe)
            {
                favoris.Add(favori);
                _memoryCache.Set(favori.AdresseIP, favoris, _cacheEntryOptions);
            }
        }

        public IEnumerable<EntFavoris> ObtenirTout(string adresseIp)
        {
            //Obtient tous les favoris appartenant à l'adresse Ip         
            bool adresseIpContientUneListeDeFavoris = _memoryCache.TryGetValue(adresseIp, out List<EntFavoris> favoris);

            if (adresseIpContientUneListeDeFavoris)
            {
                return favoris;
            }

            return new List<EntFavoris>();
        }

        public void Supprimer(int idVehicule, string adresseIp)
        {
            try
            {
                //Obtient tous les favoris appartenant à l'adresse Ip         
                _memoryCache.TryGetValue(adresseIp, out List<EntFavoris> favoris);

                //Retire le favori ayant l'Id de favori qu'on veut retirer
                favoris.Remove(favoris.First(f => f.VehiculeID == idVehicule));

                //On remet à jour les favoris de cette adresse Ip
                _memoryCache.Set(adresseIp, favoris, _cacheEntryOptions);
            }
            catch (ArgumentNullException)
            {
                throw;
            }

        }

    }
}
