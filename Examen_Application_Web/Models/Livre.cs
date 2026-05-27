using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Examen_Application_Web.Models
{
   
    public class Livre
    {
        [Key]
        [Required(ErrorMessage = "L'identifiant du livre (ex: L001) est obligatoire")]
        [Display(Name = "Code Livre")]
        public string idLivre { get; set; }

        [Required(ErrorMessage = "Le titre de l'ouvrage est requis")]
        [StringLength(150, ErrorMessage = "Le titre est trop long")]
        [Display(Name = "Titre du Livre")]
        public string titre { get; set; }

        [Required(ErrorMessage = "Le nombre de pages est obligatoire")]
        [Range(1, 10000, ErrorMessage = "Le nombre de pages doit être positif")]
        [Display(Name = "Nombre de Pages")]
        public int nbrepage { get; set; }

        
        public Livre()
        {
        }
    }
}
