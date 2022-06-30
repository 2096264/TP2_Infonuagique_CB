using System.ComponentModel.DataAnnotations;

namespace RapidAuto.MVC.Models.Favoris
{
    public class FavorisIndex
    {
        public int FavorisID { get; set; }
        public int VehiculeID { get; set; }
        [Display(Name="Image principale")]
        public string NomImage1 { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
