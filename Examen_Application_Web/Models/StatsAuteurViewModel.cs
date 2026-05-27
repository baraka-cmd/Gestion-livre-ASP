
using System;
using System.Collections.Generic;

namespace Examen_Application_Web.Models
{
    public class StatsAuteurViewModel
    {
        public string NomAuteur { get; set; }
        public string PostnomAuteur { get; set; }
        public int NombreLivres { get; set; }
        public int TotalPages { get; set; }
        public double MoyennePages { get; set; }

        public string NomComplet
        {
            get { return NomAuteur + " " + PostnomAuteur; }
        }
    }
}