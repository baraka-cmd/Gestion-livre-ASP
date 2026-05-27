using System;
using System.Collections.Generic;

namespace Examen_Application_Web.Models
{
    
    public class RapportInventaireViewModel
    {
        
        public List<Livre> ListeLivres { get; set; }

        
        public int TotalLivres { get; set; }
        public int TotalPages { get; set; }
        public double MoyennePages { get; set; }

        
        public RapportInventaireViewModel()
        {
            ListeLivres = new List<Livre>();
        }
    }
}