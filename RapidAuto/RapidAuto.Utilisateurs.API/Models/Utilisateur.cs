using System.ComponentModel.DataAnnotations;

namespace RapidAuto.Utilisateurs.API.Models
{
    public class Utilisateur
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(20)]
        public string Nom { get; set; }

        [Required, MaxLength(20)]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "L'adresse est invalide"), MaxLength(128)]
        public string Adresse { get; set; }

        [Required(ErrorMessage = "Le numéro de téléphone est invalide"), MaxLength(10), Display(Name = "Numéro de téléphone"), RegularExpression(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$")]
        public string NumeroTelephone { get; set; }

    }
}
