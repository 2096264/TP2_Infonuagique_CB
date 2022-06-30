using System.ComponentModel.DataAnnotations;

namespace RapidAuto.Commandes.API.Models
{
    public class Commande
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int UtilisateurID { get; set; }

        [Required]
        public int VehiculeID { get; set; }

    }
}
