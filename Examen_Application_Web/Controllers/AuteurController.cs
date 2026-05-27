using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Web.Mvc;
using Examen_Application_Web.Models;

namespace Examen_Application_Web.Controllers
{
    public class AuteurController : Controller
    {
        private Connexion db = new Connexion();

        
        public ActionResult Index()
        {
            List<Auteur> liste = new List<Auteur>();
           
            string query = "SELECT * FROM Auteur ORDER BY idAuteur DESC";
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
            return View(liste);
        }

 
        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Auteur aut)
        {
            try
            {
                
                string nom = !string.IsNullOrWhiteSpace(aut.nomAut) ? aut.nomAut.Trim().ToUpper() : "NOM INCONNU";
                
                string pnom = (aut.postnomAut != null) ? aut.postnomAut.Trim().ToUpper() : "";
                string prenom = (aut.prenomAut != null) ? aut.prenomAut.Trim() : "";


               
                string query = "INSERT INTO Auteur (nomAut, postnomAut, prenomAut, dateNais) VALUES (?, ?, ?, ?)";

                OleDbParameter[] parameters = {
                    new OleDbParameter("@nom", nom),
                    new OleDbParameter("@postnom", (object)pnom ?? DBNull.Value),
                    new OleDbParameter("@prenom", (object)prenom ?? DBNull.Value),
                    new OleDbParameter("@date", (object)aut.dateNais ?? DBNull.Value)
                };

                db.ExecuterAction(query, parameters);

                return RedirectToAction("Messages", "Home", new
                {
                    msg = "L'auteur " + nom + " " + prenom + " a été enregistré avec succès !",
                    type = "success",
                    url = Url.Action("Index", "Auteur")
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Messages", "Home", new
                {
                    msg = "Erreur lors de l'ajout : " + ex.Message,
                    type = "error",
                    url = Url.Action("Create", "Auteur")
                });
            }
        }

        public ActionResult Edit(int id)
        {
            string query = "SELECT * FROM Auteur WHERE idAuteur = " + id;
            DataTable dt = db.RecupererDonnees(query);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return View(new Auteur
                {
                    idAuteur = Convert.ToInt32(row["idAuteur"]),
                    nomAut = row["nomAut"].ToString(),
                    postnomAut = row["postnomAut"].ToString(),
                    prenomAut = row["prenomAut"].ToString(),
                    dateNais = row["dateNais"] != DBNull.Value ? Convert.ToDateTime(row["dateNais"]) : (DateTime?)null
                });
            }
            return RedirectToAction("Index");
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Auteur aut)
        {
            try
            {
                
                string query = "UPDATE Auteur SET nomAut = ?, postnomAut = ?, prenomAut = ?, dateNais = ? WHERE idAuteur = ?";

                OleDbParameter[] parameters = {
                    new OleDbParameter("@nom", (object)aut.nomAut.ToUpper() ?? DBNull.Value),
                    new OleDbParameter("@postnom", (object)aut.postnomAut.ToUpper() ?? DBNull.Value),
                    new OleDbParameter("@prenom", (object)aut.prenomAut ?? DBNull.Value),
                    new OleDbParameter("@date", (object)aut.dateNais ?? DBNull.Value),
                    new OleDbParameter("@id", aut.idAuteur)
                };

                db.ExecuterAction(query, parameters);

                return RedirectToAction("Messages", "Home", new
                {
                    msg = "Informations de l'auteur mises à jour !",
                    type = "success",
                    url = Url.Action("Index", "Auteur")
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Messages", "Home", new
                {
                    msg = "Échec de la mise à jour : " + ex.Message,
                    type = "error",
                    url = Url.Action("Edit", "Auteur", new { id = aut.idAuteur })
                });
            }
        }

     
        public ActionResult Delete(int id)
        {
            try
            {
                string query = "DELETE FROM Auteur WHERE idAuteur = ?";
                OleDbParameter[] parameters = { new OleDbParameter("@id", id) };
                db.ExecuterAction(query, parameters);

                return RedirectToAction("Messages", "Home", new
                {
                    msg = "L'auteur a été retiré du système.",
                    type = "warning",
                    url = Url.Action("Index", "Auteur")
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Messages", "Home", new
                {
                    msg = "Action impossible : " + ex.Message,
                    type = "error",
                    url = Url.Action("Index", "Auteur")
                });
            }
        }
    }
}
