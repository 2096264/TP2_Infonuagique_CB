using RapidAuto.Fichiers.API.Models;

namespace RapidAuto.Fichiers.API.Interfaces
{
    public interface IStorageServiceHelper
    {
            
        Task<IEnumerable<StorageAccountData>> ObtenirNomsFichiersDansBlob(string nomConteneur);


    }
}
