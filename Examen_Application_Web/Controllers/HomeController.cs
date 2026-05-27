using System;
using System.Data;
using System.Web.Mvc;
using Examen_Application_Web.Models;

namespace Examen_Application_Web.Controllers
{
    public class HomeController : Controller
    {
        private Connexion db = new Connexion();

        public ActionResult Index()
        {
            try
            {
              
                DataTable dtLivres = db.RecupererDonnees("SELECT COUNT(*) FROM Livre");
                ViewBag.TotalLivres = dtLivres.Rows[0][0].ToString();

             
                DataTable dtAuteurs = db.RecupererDonnees("SELECT COUNT(*) FROM Auteur");
                ViewBag.TotalAuteurs = dtAuteurs.Rows[0][0].ToString();

                
                DataTable dtEcrire = db.RecupererDonnees("SELECT COUNT(*) FROM Ecrire");
                ViewBag.TotalEcrire = dtEcrire.Rows[0][0].ToString();

              
                ViewBag.TotalRapports = "8";
            }
            catch
            {
                
                ViewBag.TotalLivres = "0";
                ViewBag.TotalAuteurs = "0";
                ViewBag.TotalEcrire = "0";
                ViewBag.TotalRapports = "8";
            }

            return View();
        }

        public ActionResult Messages(string msg, string type, string url)
        {
            ViewBag.Message = msg;
            ViewBag.Type = type; 
            ViewBag.Redirection = url;

            return View();
        }
    }
}