using Microsoft.AspNetCore.Mvc.Rendering;
using RapidAuto.MVC.Models;

namespace RapidAuto.MVC.Helpers
{
    public class VehiculesHelper
    {
        public SelectList GenererSelectListPourLaRecherche(string? filtreDeRecherche)
        {
            SelectList selectListFiltreRecherche = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Selected = false, Text = "Modèle", Value = "model"},
                new SelectListItem { Selected = false, Text = "Année", Value = "annee"},
                new SelectListItem { Selected = false, Text = "Numéro d'identification", Value = "niv"},
            }, "Value", "Text");

            if (!String.IsNullOrEmpty(filtreDeRecherche))
            {
                selectListFiltreRecherche.First(s => s.Value == filtreDeRecherche).Selected = true;
            }

            return selectListFiltreRecherche;
        }

        public SelectList GenererSelectListDeConstructeurs(List<Vehicule> vehicules, string? constructeur)
        {
            vehicules = vehicules.GroupBy(v => v.Constructeur).Select(x => x.FirstOrDefault()).ToList();
            var selectListConstructeur = new SelectList(vehicules, "Constructeur", "Constructeur");

            if (!String.IsNullOrEmpty(constructeur))
            {
                selectListConstructeur.First(s => s.Value == constructeur).Selected = true;
            }

            return selectListConstructeur;
        }

        public string CreerCodeVehicule(string marque, string niv)
        {
            return marque + niv;
        }

        public void ImagesToString(ref EntGestionFichiers gestionFichiers, IFormFile image1, IFormFile image2)
        {
            gestionFichiers.Fichier1 = ImageToString(image1);
            gestionFichiers.Fichier2 = ImageToString(image2);
        }

        private string ImageToString(IFormFile image)
        {
            //Convertir les images en chaines de caractères pour faciliter le transfert en Json
            using (var ms = new MemoryStream())
            {
                image.CopyTo(ms);
                var fileBytes = ms.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }
    }
}
