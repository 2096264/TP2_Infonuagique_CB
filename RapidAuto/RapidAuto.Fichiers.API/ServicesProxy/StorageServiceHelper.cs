using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using RapidAuto.Fichiers.API.Interfaces;
using RapidAuto.Fichiers.API.Models;
using Microsoft.Extensions.Azure;
using System.Linq;


namespace RapidAuto.Fichiers.API.ServicesProxy
{
    public class StorageServiceHelper : IStorageServiceHelper
    {

        private readonly BlobServiceClient _blobServiceClient;
        
        public StorageServiceHelper(BlobServiceClient blobClient)
        {
            _blobServiceClient = blobClient;
            
        }

       
        public async Task<string> ObtenirConteneur(string nomConteneur)
        {
            string conteneur = nomConteneur;

            //Obtention de toutes les conteneurs dans le compte de stockage
            await foreach (BlobContainerItem containerItem in _blobServiceClient.GetBlobContainersAsync())
            {
                if(containerItem.Name == nomConteneur)
                {
                    conteneur = containerItem.Name;
                    break;
                }              
            }

            return conteneur;

        }

        public async Task<IEnumerable<StorageAccountData>> ObtenirNomsFichiersDansBlob(string nomConteneur)
        {
            List<StorageAccountData> storageAccountDatas = new();

            //Obtention d'un conteneur blob
            var containerClient = _blobServiceClient.GetBlobContainerClient(nomConteneur);

            //Lecture des bloc dans le conteneur
            await foreach (BlobItem blob in containerClient.GetBlobsAsync())
            {
                storageAccountDatas.Add(new StorageAccountData
                {
                    Id = blob.VersionId,
                    DateAjout = blob.Properties.LastModified,
                    Value = blob.Name
                });
            }

            return storageAccountDatas;
        }
    }
}
