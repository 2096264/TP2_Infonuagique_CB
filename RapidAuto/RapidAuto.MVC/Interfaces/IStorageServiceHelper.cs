using RapidAuto.MVC.Models;

namespace RapidAuto.MVC.Interfaces
{
    public interface IStorageServiceHelper
    {
        Task<IEnumerable<StorageAccountData>> ObtenirNomsFichiersDansBlob(string nomConteneur);

    }
}
