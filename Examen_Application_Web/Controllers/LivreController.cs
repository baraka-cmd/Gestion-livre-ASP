using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Web.Mvc;
using Examen_Application_Web.Models;

namespace Examen_Application_Web.Controllers
{
    public class LivreController : Controller
    {
        private Connexion db = new Connexion();

        
        public ActionResult Index()
        {
            List<Livre> liste = new List<Livre>();
            string query = "SELECT idLivre, titre, nbrepage FROM Livre ORDER BY idLivre DESC";
            DataTable dt = db.RecupererDonnees(query);

            foreach (DataRow row in dt.Rows)
            {
                liste.Add(new Livre
                {
                    idLivre = row["idLivre"].ToString(),
                    titre = row["titre"].ToString(),
                    nbrepage = Convert.ToInt32(row["nbrepage"])
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
        public ActionResult Create(Livre liv)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string query = "INSERT INTO Livre (idLivre, titre, nbrepage) VALUES (?, ?, ?)";

                    OleDbParameter[] parameters = {
                        new OleDbParameter("@id", liv.idLivre.Trim().ToUpper()),
                        new OleDbParameter("@titre", liv.titre.Trim()),
                        new OleDbParameter("@pages", liv.nbrepage)
                    };

                    db.ExecuterAction(query, parameters);

                    return RedirectToAction("Messages", "Home", new
                    {
                        msg = "Le livre '" + liv.titre + "' a été enregistré avec succès !",
                        type = "success",
                        url = Url.Action("Index", "Livre")
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur d'insertion : " + ex.Message;
            }
            return View(liv);
        }

    
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            string query = "SELECT * FROM Livre WHERE idLivre = ?";
            OleDbParameter[] parameters = { new OleDbParameter("@id", id) };
            DataTable dt = db.RecupererDonnees(query, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                Livre liv = new Livre
                {
                    idLivre = row["idLivre"].ToString(),
                    titre = row["titre"].ToString(),
                    nbrepage = Convert.ToInt32(row["nbrepage"])
                };
                return View(liv);
            }
            return RedirectToAction("Index");
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Livre liv)
        {
            try
            {
                string query = "UPDATE Livre SET titre = ?, nbrepage = ? WHERE idLivre = ?";

                OleDbParameter[] parameters = {
                    new OleDbParameter("@titre", liv.titre.Trim()),
                    new OleDbParameter("@pages", liv.nbrepage),
                    new OleDbParameter("@id", liv.idLivre)
                };

                db.ExecuterAction(query, parameters);

                return RedirectToAction("Messages", "Home", new
                {
                    msg = "Mise à jour effectuée !",
                    type = "success",
                    url = Url.Action("Index", "Livre")
                });
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Erreur : " + ex.Message;
                return View(liv);
            }
        }

     
        public ActionResult Delete(string id)
        {
            try
            {
                string query = "DELETE FROM Livre WHERE idLivre = ?";
                OleDbParameter[] parameters = { new OleDbParameter("@id", id) };
                db.ExecuterAction(query, parameters);

                return RedirectToAction("Messages", "Home", new
                {
                    msg = "Livre supprimé.",
                    type = "warning",
                    url = Url.Action("Index", "Livre")
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Messages", "Home", new { msg = ex.Message, type = "error" });
            }
        }

        
        [HttpGet]
        public ActionResult GetNombrePages(string id)
        {
            string query = "SELECT nbrepage FROM Livre WHERE idLivre = ?";
            OleDbParameter[] parameters = {
                new OleDbParameter("@id", id)
            };

            DataTable dt = db.RecupererDonnees(query, parameters);

            if (dt.Rows.Count > 0)
            {
                int nbPages = Convert.ToInt32(dt.Rows[0]["nbrepage"]);
                return Content(nbPages.ToString());
            }

            return Content("0");
        }
    }
}