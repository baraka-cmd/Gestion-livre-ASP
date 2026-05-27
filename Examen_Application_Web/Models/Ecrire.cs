using System;
using System.ComponentModel.DataAnnotations;

namespace Examen_Application_Web.Models
{
    public class Ecrire
    {
        [Required]
        public int idAuteur { get; set; }

        [Required]
        public string idLivre { get; set; } 

        [DataType(DataType.Date)]
        public DateTime? datPub { get; set; }

        public int nbrePag { get; set; }

        
        public string NomAuteur { get; set; }
        public string TitreLivre { get; set; }

        public Ecrire()
        {
            this.datPub = DateTime.Now;
        }
    }
}