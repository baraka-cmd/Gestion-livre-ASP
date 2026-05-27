
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using Examen_Application_Web.Models;
using System.Linq;

namespace Examen_Application_Web.Controllers
{
    public class RapportController : Controller
    {
        private Connexion db = new Connexion();
        
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult ListeAuteurs()
        {
            List<Auteur> liste = new List<Auteur>();
            string query = "SELECT idAuteur, nomAut, postnomAut, prenomAut, dateNais FROM Auteur ORDER BY nomAut ASC";

            try
            {
                DataTable dt = db.RecupererDonnees(query);
                foreach (DataRow row in dt.Rows)
                {
                    liste.Add(new Auteur
                    {
                        idAuteur = Convert.ToInt32(row["idAuteur"]),
                        nomAut = row["nomAut"].ToString(),
                        postnomAut = row["postnomAut"].ToString(),
                        prenomAut = row["prenomAut"].ToString(),
                        dateNais = row["dateNais"] != DBNull.Value ? Convert.ToDateTime(row["dateNais"]) : (DateTime?)null
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur : " + ex.Message;
            }

            return View(liste);
        }

        
        public ActionResult ListeLivres()
        {
            List<Livre> liste = new List<Livre>();
            string query = "SELECT idLivre, titre, nbrepage FROM Livre ORDER BY titre ASC";

            try
            {
                DataTable dt = db.RecupererDonnees(query);
                foreach (DataRow row in dt.Rows)
                {
                    liste.Add(new Livre
                    {
                        idLivre = row["idLivre"].ToString(),
                        titre = row["titre"].ToString(),
                        nbrepage = row["nbrepage"] != DBNull.Value ? Convert.ToInt32(row["nbrepage"]) : 0
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur : " + ex.Message;
            }

            return View(liste);
        }

        
        public ActionResult PublicationsParAuteur()
        {
            List<Ecrire> liste = new List<Ecrire>();

            string query = @"SELECT Auteur.nomAut, Auteur.postnomAut, Livre.titre, 
                             Ecrire.datPub, Ecrire.nbrePag, Ecrire.idAuteur, Ecrire.idLivre
                             FROM (Ecrire 
                             INNER JOIN Auteur ON Ecrire.idAuteur = Auteur.idAuteur) 
                             INNER JOIN Livre ON Ecrire.idLivre = Livre.idLivre
                             ORDER BY Auteur.nomAut ASC";

            try
            {
                DataTable dt = db.RecupererDonnees(query);
                foreach (DataRow row in dt.Rows)
                {
                    liste.Add(new Ecrire
                    {
                        idAuteur = Convert.ToInt32(row["idAuteur"]),
                        idLivre = row["idLivre"].ToString(),
                        NomAuteur = row["nomAut"].ToString() + " " + row["postnomAut"].ToString(),
                        TitreLivre = row["titre"].ToString(),
                        nbrePag = row["nbrePag"] != DBNull.Value ? Convert.ToInt32(row["nbrePag"]) : 0,
                        datPub = row["datPub"] != DBNull.Value ? Convert.ToDateTime(row["datPub"]) : (DateTime?)null
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur : " + ex.Message;
            }

            return View(liste);
        }

        
        public ActionResult LivresEtAuteurs()
        {
            List<Ecrire> liste = new List<Ecrire>();

            string query = @"SELECT Auteur.nomAut, Auteur.postnomAut, Livre.titre, 
                             Ecrire.datPub, Ecrire.nbrePag, Ecrire.idAuteur, Ecrire.idLivre
                             FROM (Ecrire 
                             INNER JOIN Auteur ON Ecrire.idAuteur = Auteur.idAuteur) 
                             INNER JOIN Livre ON Ecrire.idLivre = Livre.idLivre
                             ORDER BY Livre.titre ASC";

            try
            {
                DataTable dt = db.RecupererDonnees(query);
                foreach (DataRow row in dt.Rows)
                {
                    liste.Add(new Ecrire
                    {
                        idAuteur = Convert.ToInt32(row["idAuteur"]),
                        idLivre = row["idLivre"].ToString(),
                        NomAuteur = row["nomAut"].ToString() + " " + row["postnomAut"].ToString(),
                        TitreLivre = row["titre"].ToString(),
                        nbrePag = row["nbrePag"] != DBNull.Value ? Convert.ToInt32(row["nbrePag"]) : 0,
                        datPub = row["datPub"] != DBNull.Value ? Convert.ToDateTime(row["datPub"]) : (DateTime?)null
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur : " + ex.Message;
            }

            return View(liste);
        }

       
        public ActionResult AuteursSansPublication()
        {
            List<Auteur> liste = new List<Auteur>();

            string query = @"SELECT Auteur.idAuteur, Auteur.nomAut, Auteur.postnomAut, Auteur.prenomAut
                             FROM Auteur 
                             LEFT JOIN Ecrire ON Auteur.idAuteur = Ecrire.idAuteur
                             WHERE Ecrire.idAuteur IS NULL
                             ORDER BY Auteur.nomAut ASC";

            try
            {
                DataTable dt = db.RecupererDonnees(query);
                foreach (DataRow row in dt.Rows)
                {
                    liste.Add(new Auteur
                    {
                        idAuteur = Convert.ToInt32(row["idAuteur"]),
                        nomAut = row["nomAut"].ToString(),
                        postnomAut = row["postnomAut"].ToString(),
                        prenomAut = row["prenomAut"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur : " + ex.Message;
            }

            return View(liste);
        }

     
        public ActionResult LivresSansAuteur()
        {
            List<Livre> liste = new List<Livre>();

            string query = @"SELECT Livre.idLivre, Livre.titre, Livre.nbrepage
                             FROM Livre 
                             LEFT JOIN Ecrire ON Livre.idLivre = Ecrire.idLivre
                             WHERE Ecrire.idLivre IS NULL
                             ORDER BY Livre.titre ASC";

            try
            {
                DataTable dt = db.RecupererDonnees(query);
                foreach (DataRow row in dt.Rows)
                {
                    liste.Add(new Livre
                    {
                        idLivre = row["idLivre"].ToString(),
                        titre = row["titre"].ToString(),
                        nbrepage = row["nbrepage"] != DBNull.Value ? Convert.ToInt32(row["nbrepage"]) : 0
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur : " + ex.Message;
            }

            return View(liste);
        }

        
        public ActionResult InventaireLivres()
        {
            var viewModel = new RapportInventaireViewModel();

            string queryListe = "SELECT idLivre, titre, nbrepage FROM Livre ORDER BY titre ASC";
            string queryStats = "SELECT COUNT(*) AS Total, SUM(nbrepage) AS Somme FROM Livre";

            try
            {
                DataTable dtLivres = db.RecupererDonnees(queryListe);
                foreach (DataRow row in dtLivres.Rows)
                {
                    viewModel.ListeLivres.Add(new Livre
                    {
                        idLivre = row["idLivre"].ToString(),
                        titre = row["titre"].ToString(),
                        nbrepage = row["nbrepage"] != DBNull.Value ? Convert.ToInt32(row["nbrepage"]) : 0
                    });
                }

                DataTable dtStats = db.RecupererDonnees(queryStats);
                if (dtStats.Rows.Count > 0)
                {
                    viewModel.TotalLivres = Convert.ToInt32(dtStats.Rows[0]["Total"]);
                    viewModel.TotalPages = dtStats.Rows[0]["Somme"] != DBNull.Value ? Convert.ToInt32(dtStats.Rows[0]["Somme"]) : 0;
                    viewModel.MoyennePages = viewModel.TotalLivres > 0
                        ? Math.Round((double)viewModel.TotalPages / viewModel.TotalLivres, 2)
                        : 0;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur inventaire : " + ex.Message;
            }

            return View(viewModel);
        }

        
        public ActionResult StatistiquesParAuteur()
        {
            List<StatsAuteurViewModel> liste = new List<StatsAuteurViewModel>();

            string query = @"SELECT Auteur.nomAut, Auteur.postnomAut, 
                             COUNT(Ecrire.idLivre) AS NombreLivres,
                             SUM(Ecrire.nbrePag) AS TotalPages
                             FROM Auteur 
                             INNER JOIN Ecrire ON Auteur.idAuteur = Ecrire.idAuteur
                             GROUP BY Auteur.nomAut, Auteur.postnomAut
                             ORDER BY Auteur.nomAut ASC";

            try
            {
                DataTable dt = db.RecupererDonnees(query);
                foreach (DataRow row in dt.Rows)
                {
                    int totalPages = row["TotalPages"] != DBNull.Value ? Convert.ToInt32(row["TotalPages"]) : 0;
                    int nombreLivres = Convert.ToInt32(row["NombreLivres"]);

                    liste.Add(new StatsAuteurViewModel
                    {
                        NomAuteur = row["nomAut"].ToString(),
                        PostnomAuteur = row["postnomAut"].ToString(),
                        NombreLivres = nombreLivres,
                        TotalPages = totalPages,
                        MoyennePages = nombreLivres > 0 ? Math.Round((double)totalPages / nombreLivres, 2) : 0
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur : " + ex.Message;
            }

            return View(liste);
        }


        
    }
}