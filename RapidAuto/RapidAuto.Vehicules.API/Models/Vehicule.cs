using System.ComponentModel.DataAnnotations;

namespace RapidAuto.Vehicules.API.Models
{
    public class Vehicule
    {
        [Key]
        public int ID { get; set; }
        
        [Required, MaxLength(20)]
        public string Constructeur { get; set; }

        [Required, MaxLength(20)]
        public string Modele { get; set; }

        [Required]
        public int AnneeFabrication { get; set; }

        [Required, MaxLength(10)]
        public string Type { get; set; }

        [Required]
        public int NbrSieges { get; set; }

        [Required, MaxLength(20)]
        public string Couleur { get; set; }

        [Required, MaxLength(20)]
        public string Niv { get; set; }

        [Required, MaxLength(50)]
        public string NomImage1 { get; set; }

        [Required, MaxLength(50)]
        public string NomImage2 { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public bool EstVendu { get; set; }

        [Required]
        public decimal Prix { get; set; }

        [Required]
        public string CodeUnique { get; set; }
    }
}
