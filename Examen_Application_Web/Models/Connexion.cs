using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Configuration;


namespace Examen_Application_Web.Models
{
    public class Connexion
    {
        private OleDbConnection con;
        private OleDbCommand cmd;
        private OleDbDataAdapter da;
        private DataTable dt;


        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\BARAKA\Documents\BiblioUAGO.accdb";

        public Connexion()
        {
            con = new OleDbConnection(connectionString);
            cmd = new OleDbCommand();
            cmd.Connection = con;
        }


        public DataTable RecupererDonnees(string query, OleDbParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand(query, con))
                {
                    
                    if (parameters != null)
                    {
                        cmd.Parameters.Clear();
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }

                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public int ExecuterAction(string query, OleDbParameter[] parameters = null)
        {
            int result = 0;
            try
            {
                cmd.CommandText = query;
                cmd.Parameters.Clear();

                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                if (con.State == ConnectionState.Closed) con.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur d'exécution SQL : " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
            return result;
        }
    }
}