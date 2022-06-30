using System.ComponentModel.DataAnnotations;

namespace RapidAuto.MVC.Models
{
    public class Vehicule
    {
        [Key]
        public int ID { get; set; }

        [Required, MaxLength(20), Display(Name = "Constructeur")]
        public string Constructeur { get; set; }

        [Required, MaxLength(20), Display(Name = "Modèle")]
        public string Modele { get; set; }

        [Required, Range(1900.0, 2050.0, ErrorMessage = "Les années acceptées sont de 1900 à 2050."), Display(Name = "Année de fabrication")]
        public int AnneeFabrication { get; set; }

        [Required, MaxLength(10), Display(Name = "Type de carburant")]
        public string Type { get; set; }

        [Required, Range(1.0, 100.0, ErrorMessage = "Le nombre de sièges doit être entre 0 et 100."), Display(Name = "Nombre de sièges")]
        public int NbrSieges { get; set; }

        [Required, MaxLength(20), Display(Name = "Couleur")]
        public string Couleur { get; set; }

        [   
            Required, 
            MaxLength(20), 
            RegularExpression("[A-HJ-NPR-Z0-9]{17}", ErrorMessage ="Entrer un numéro d'identification valide."), //Pour tester: https://vingenerator.org/
            Display(Name = "Numéro d'identification")
        ]
        public string Niv { get; set; }

        [Required, MaxLength(50), Display(Name = "Image principale")]
        public string NomImage1 { get; set; }

        [Required, MaxLength(50), Display(Name = "Image secondaire")]
        public string NomImage2 { get; set; }

        [Required, MaxLength(500), Display(Name = "Description")]
        public string Description { get; set; }

        [Required, Display(Name = "Est vendu")]
        public bool EstVendu { get; set; }

        [Required, Display(Name = "Prix"), Range(1.0, 999999999.0, ErrorMessage = "Entrer un prix valide.")]
        public decimal Prix { get; set; }

        [Required, Display(Name = "Code Unique")]
        public string CodeUnique { get; set; }
    }
}
