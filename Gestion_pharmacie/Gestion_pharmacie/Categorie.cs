using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Gestion_pharmacie
{
    internal class Categorie
    {
        int Id_Categorie;
        string nom_categorie, discription;

        public Categorie() { }

        public Categorie(int Id_Categorie, string nom_categorie, string discription)
        {
            this.Id_Categorie = Id_Categorie;
            this.nom_categorie = nom_categorie;
            this.discription = discription;
        }

        public Categorie(string nom_categorie, string discription)
        {
            this.Id_Categorie = -1;
            this.nom_categorie = nom_categorie;
            this.discription = discription;
        }

        public int get_Id_Categorie() { return Id_Categorie; }
        public string get_nom_categorie() { return nom_categorie; }
        public string get_discription() { return discription; }

        private void get_id_categorie_db()
        {
            SqlConnection conn = DB_Connexion.getInstance();
            string query = "SELECT Id_Categorie FROM categorie WHERE nom_categorie=@nom_categorie";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nom_categorie", nom_categorie.ToLower().Trim());

            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                Id_Categorie = int.Parse(rdr["Id_Categorie"].ToString());
            }
            rdr.Close();
        }

        public int mdifie_Catgorie(String discription)
        {
            SqlConnection conn = DB_Connexion.getInstance();
            string query = "UPDATE categorie SET discription=@description WHERE Id_Categorie=@Id_Categorie";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id_Categorie", Id_Categorie);
            cmd.Parameters.AddWithValue("@description", discription);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                this.discription = discription;
                return 1;
            }
            return 0;
        }

        public Categorie ajouter_Categorie(string nom_categorie, String discription)
        {
            SqlConnection conn = DB_Connexion.getInstance();
            string query = "INSERT INTO categorie(nom_categorie, discription) VALUES (@nom_categorie, @discription)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nom_categorie", nom_categorie.ToLower().Trim());
            cmd.Parameters.AddWithValue("@discription", discription);

            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                Categorie cat = new Categorie(nom_categorie.ToLower().Trim(), discription);
                cat.get_id_categorie_db();
                return cat;
            }
            return null;
        }
    }
}

