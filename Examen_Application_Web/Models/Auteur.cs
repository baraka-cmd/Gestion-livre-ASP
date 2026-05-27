using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Examen_Application_Web.Models
{
    public class Auteur
    {
        [Key]
        public int idAuteur { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string nomAut { get; set; }

        public string postnomAut { get; set; }

        public string prenomAut { get; set; }

        [DataType(DataType.Date)]
        public DateTime? dateNais { get; set; }

  
        public string NomComplet
        {
            get
            {
                
                string n = !string.IsNullOrEmpty(this.nomAut) ? this.nomAut.Trim() : "";
                string pn = !string.IsNullOrEmpty(this.postnomAut) ? this.postnomAut.Trim() : "";
                string pr = !string.IsNullOrEmpty(this.prenomAut) ? this.prenomAut.Trim() : "";

                
                string resultat = n + " " + pn + " " + pr;

                return resultat.Trim();
            }
        }


        public virtual ICollection<Livre> Livres { get; set; }

        public Auteur()
        {
            this.Livres = new HashSet<Livre>();
            this.dateNais = DateTime.Now;
        }
    }
}
