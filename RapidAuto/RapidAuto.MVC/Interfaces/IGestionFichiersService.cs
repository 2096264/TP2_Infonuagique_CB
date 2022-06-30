using Microsoft.AspNetCore.Mvc;
using RapidAuto.MVC.Models;

namespace RapidAuto.MVC.Interfaces
{
    public interface IGestionFichiersService
    {
        Task<EntGestionFichiers> Obtenir(string nom);
        Task<HttpResponseMessage> RenommerEtSauvegarder(EntGestionFichiers entGestionFichiers);
    }
}
