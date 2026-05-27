using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Web.Mvc;
using Examen_Application_Web.Models;

namespace Examen_Application_Web.Controllers
{
    public class EcrireController : Controller
    {
        private Connexion db = new Connexion();

        
        public ActionResult Index()
        {
            List<Ecrire> liste = new List<Ecrire>();
           
            string query = @"SELECT Ecrire.*, Auteur.nomAut, Auteur.postnomAut, Livre.titre 
                             FROM (Ecrire 
                             INNER JOIN Auteur ON Ecrire.idAuteur = Auteur.idAuteur) 
                             INNER JOIN Livre ON Ecrire.idLivre = Livre.idLivre";

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
            return View(liste);
        }

      
        public ActionResult Create()
        {
           
            DataTable dtAut = db.RecupererDonnees("SELECT idAuteur, nomAut + ' ' + postnomAut AS NomComplet FROM Auteur");
            ViewBag.Auteurs = new SelectList(dtAut.DefaultView, "idAuteur", "NomComplet");

            
            DataTable dtLiv = db.RecupererDonnees("SELECT idLivre, titre FROM Livre");
            ViewBag.Livres = new SelectList(dtLiv.DefaultView, "idLivre", "titre");

            return View(new Ecrire());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ecrire ec)
        {
            try
            {
                string query = "INSERT INTO Ecrire (idAuteur, idLivre, datPub, nbrePag) VALUES (?, ?, ?, ?)";

                OleDbParameter[] parameters = {
                    new OleDbParameter("@aut", ec.idAuteur),
                    new OleDbParameter("@liv", ec.idLivre),
                    new OleDbParameter("@date", (object)ec.datPub ?? DBNull.Value),
                    new OleDbParameter("@pag", ec.nbrePag)
                };

                db.ExecuterAction(query, parameters);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur : " + ex.Message;

               
                DataTable dtAut = db.RecupererDonnees("SELECT idAuteur, nomAut + ' ' + postnomAut AS NomComplet FROM Auteur");
                ViewBag.Auteurs = new SelectList(dtAut.DefaultView, "idAuteur", "NomComplet");

                DataTable dtLiv = db.RecupererDonnees("SELECT idLivre, titre FROM Livre");
                ViewBag.Livres = new SelectList(dtLiv.DefaultView, "idLivre", "titre");

                return View(ec);
            }
        }

        
        public ActionResult Edit(int idAut, string idLiv)
        {
            string query = @"SELECT Ecrire.*, Auteur.nomAut, Auteur.postnomAut, Livre.titre 
                             FROM (Ecrire 
                             INNER JOIN Auteur ON Ecrire.idAuteur = Auteur.idAuteur) 
                             INNER JOIN Livre ON Ecrire.idLivre = Livre.idLivre 
                             WHERE Ecrire.idAuteur = ? AND Ecrire.idLivre = ?";

            OleDbParameter[] parameters = {
                new OleDbParameter("@idAut", idAut),
                new OleDbParameter("@idLiv", idLiv)
            };

            DataTable dt = db.RecupererDonnees(query, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                Ecrire ec = new Ecrire
                {
                    idAuteur = Convert.ToInt32(row["idAuteur"]),
                    idLivre = row["idLivre"].ToString(),
                    NomAuteur = row["nomAut"].ToString() + " " + row["postnomAut"].ToString(),
                    TitreLivre = row["titre"].ToString(),
                    nbrePag = row["nbrePag"] != DBNull.Value ? Convert.ToInt32(row["nbrePag"]) : 0,
                    datPub = row["datPub"] != DBNull.Value ? Convert.ToDateTime(row["datPub"]) : (DateTime?)null
                };

              
                DataTable dtAut = db.RecupererDonnees("SELECT idAuteur, nomAut + ' ' + postnomAut AS NomComplet FROM Auteur");
                ViewBag.Auteurs = new SelectList(dtAut.DefaultView, "idAuteur", "NomComplet", ec.idAuteur);

                DataTable dtLiv = db.RecupererDonnees("SELECT idLivre, titre FROM Livre");
                ViewBag.Livres = new SelectList(dtLiv.DefaultView, "idLivre", "titre", ec.idLivre);

                return View(ec);
            }

            
            DataTable dtAuteurs = db.RecupererDonnees("SELECT idAuteur, nomAut + ' ' + postnomAut AS NomComplet FROM Auteur");
            ViewBag.Auteurs = new SelectList(dtAuteurs.DefaultView, "idAuteur", "NomComplet");

            DataTable dtLivres = db.RecupererDonnees("SELECT idLivre, titre FROM Livre");
            ViewBag.Livres = new SelectList(dtLivres.DefaultView, "idLivre", "titre");

            return RedirectToAction("Index");
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ecrire ec, int oldIdAuteur, string oldIdLivre)
        {
            try
            {
                string query = "UPDATE Ecrire SET idAuteur = ?, idLivre = ?, datPub = ?, nbrePag = ? WHERE idAuteur = ? AND idLivre = ?";

                OleDbParameter[] parameters = {
            new OleDbParameter("@newAut", ec.idAuteur),
            new OleDbParameter("@newLiv", ec.idLivre),
            new OleDbParameter("@date", (object)ec.datPub ?? DBNull.Value),
            new OleDbParameter("@pag", ec.nbrePag),
            new OleDbParameter("@oldAut", oldIdAuteur),   
            new OleDbParameter("@oldLiv", oldIdLivre)    
        };

                db.ExecuterAction(query, parameters);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur : " + ex.Message;

                
                DataTable dtAut = db.RecupererDonnees("SELECT idAuteur, nomAut + ' ' + postnomAut AS NomComplet FROM Auteur");
                ViewBag.Auteurs = new SelectList(dtAut.DefaultView, "idAuteur", "NomComplet", ec.idAuteur);

                DataTable dtLiv = db.RecupererDonnees("SELECT idLivre, titre FROM Livre");
                ViewBag.Livres = new SelectList(dtLiv.DefaultView, "idLivre", "titre", ec.idLivre);

                return View(ec);
            }
        }

      
        public ActionResult Delete(int idAut, string idLiv)
        {
            try
            {
                string query = "DELETE FROM Ecrire WHERE idAuteur = ? AND idLivre = ?";
                OleDbParameter[] parameters = {
                    new OleDbParameter("@idAut", idAut),
                    new OleDbParameter("@idLiv", idLiv)
                };
                db.ExecuterAction(query, parameters);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur : " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}