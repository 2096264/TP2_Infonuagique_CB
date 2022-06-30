using System.ComponentModel.DataAnnotations;

namespace RapidAuto.MVC.Models.Commandes
{
    public class Commande
    {
        [Key]
        public int ID { get; set; }

        [Required, Display(Name = "Numéro de compte")]
        public int UtilisateurID { get; set; }

        [Required]
        public int VehiculeID { get; set; }
    }
}
