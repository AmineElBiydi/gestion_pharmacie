using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Gestion_pharmacie
{
    internal class liste_Medicament_standard
    {
        Dictionary<int, Medicament_Standard> liste_medic_std;

        public liste_Medicament_standard()
        {
            liste_medic_std = get_all();
        }

        private Dictionary<int, Medicament_Standard> get_all()
        {
            SqlConnection conn = DB_Connexion.getInstance();
            String query = "SELECT * FROM medicament";
            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataReader rdr = cmd.ExecuteReader();
            Dictionary<int, Medicament_Standard> liste = new Dictionary<int, Medicament_Standard>();

            while (rdr.Read())
            {
                int id = int.Parse(rdr["id_medicament"].ToString());
                Medicament_Standard medic_std = new Medicament_Standard(
                    id,
                    rdr["nom_Medicament"].ToString().ToLower(),
                    rdr["description"].ToString(),
                    rdr["dosage"].ToString(),
                    rdr["statut"].ToString(),
                    rdr["prescription_requise"].ToString(),
                    rdr["forme_pharmaceutique"].ToString(),
                    float.Parse(rdr["prix_unitaire"].ToString()),
                    float.Parse(rdr["prix_achat"].ToString()),
                    List_Categorie.getCategorie(int.Parse(rdr["id_categorie"].ToString())),
                    DateTime.Parse(rdr["date_creation"].ToString()),
                    DateTime.Parse(rdr["date_modification"].ToString())
                );
                liste.Add(id, medic_std);
            }
            rdr.Close();

            return liste;
        }

        public int ajouter_medicament_standard(String nom_Medicament, String description, String dosage,
            String statut, String prescription_requise, String forme_pharmaceutique, float prix_unitaire,
            float prix_achat, Categorie categorie)
        {
            if (medicament_standard_existe(nom_Medicament, dosage, forme_pharmaceutique))
            {
                return -1;
            }

            SqlConnection conn = DB_Connexion.getInstance();
            String query = "INSERT INTO medicament (nom_Medicament, description, dosage, statut, prescription_requise, " +
                "forme_pharmaceutique, prix_unitaire, prix_achat, id_categorie, date_creation, date_modification) " +
                "VALUES (@nom_Medicament, @description, @dosage, @statut, @prescription_requise, @forme_pharmaceutique, " +
                "@prix_unitaire, @prix_achat, @id_categorie, @date_creation, @date_modification)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nom_Medicament", nom_Medicament.ToLower());
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@dosage", dosage);
            cmd.Parameters.AddWithValue("@statut", statut);
            cmd.Parameters.AddWithValue("@prescription_requise", prescription_requise);
            cmd.Parameters.AddWithValue("@forme_pharmaceutique", forme_pharmaceutique);
            cmd.Parameters.AddWithValue("@prix_unitaire", prix_unitaire);
            cmd.Parameters.AddWithValue("@prix_achat", prix_achat);
            cmd.Parameters.AddWithValue("@id_categorie", categorie.get_Id_Categorie());
            cmd.Parameters.AddWithValue("@date_creation", DateTime.Now);
            cmd.Parameters.AddWithValue("@date_modification", DateTime.Now);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                int newId = chercher_medicament_standard(nom_Medicament, dosage, forme_pharmaceutique);
                if (newId != -1)
                {
                    liste_medic_std.Add(newId, new Medicament_Standard(
                        newId,
                        nom_Medicament.ToLower(),
                        description,
                        dosage,
                        statut,
                        prescription_requise,
                        forme_pharmaceutique,
                        prix_unitaire,
                        prix_achat,
                        categorie,
                        DateTime.Now,
                        DateTime.Now
                    ));
                }
                return 1;
            }
            return -2;
        }

        public bool medicament_standard_existe(String nom_medicament, string dosage, string forme_pharmaceutique)
        {
            return chercher_medicament_standard(nom_medicament, dosage, forme_pharmaceutique) != -1;
        }

        public int chercher_medicament_standard(String nom_medicament, string dosage, string forme_pharmaceutique)
        {
            SqlConnection conn = DB_Connexion.getInstance();
            String query = "SELECT id_medicament FROM medicament WHERE nom_medicament=@nom_medicament AND dosage=@dosage " +
                "AND forme_pharmaceutique=@forme_pharmaceutique";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nom_medicament", nom_medicament.ToLower());
            cmd.Parameters.AddWithValue("@dosage", dosage);
            cmd.Parameters.AddWithValue("@forme_pharmaceutique", forme_pharmaceutique);

            SqlDataReader rdr = cmd.ExecuteReader();
            int result = -1;
            if (rdr.Read())
            {
                result = int.Parse(rdr["id_medicament"].ToString());
            }
            rdr.Close();
            return result;
        }

        public Dictionary<int, Medicament_Standard> chercher_medicament_nom(String nom_medicament)
        {
            SqlConnection conn = DB_Connexion.getInstance();
            String query = "SELECT * FROM medicament WHERE nom_medicament=@nom_medicament";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nom_medicament", nom_medicament.ToLower());

            SqlDataReader rdr = cmd.ExecuteReader();
            Dictionary<int, Medicament_Standard> liste = new Dictionary<int, Medicament_Standard>();

            while (rdr.Read())
            {
                int id = int.Parse(rdr["id_medicament"].ToString());
                Medicament_Standard medic_std = new Medicament_Standard(
                    id,
                    rdr["nom_Medicament"].ToString().ToLower(),
                    rdr["description"].ToString(),
                    rdr["dosage"].ToString(),
                    rdr["statut"].ToString(),
                    rdr["prescription_requise"].ToString(),
                    rdr["forme_pharmaceutique"].ToString(),
                    float.Parse(rdr["prix_unitaire"].ToString()),
                    float.Parse(rdr["prix_achat"].ToString()),
                    List_Categorie.getCategorie(int.Parse(rdr["id_categorie"].ToString())),
                    DateTime.Parse(rdr["date_creation"].ToString()),
                    DateTime.Parse(rdr["date_modification"].ToString())
                );
                liste.Add(id, medic_std);
            }
            rdr.Close();

            return liste.Count > 0 ? liste : null;
        }

        public Dictionary<int, Medicament_Standard> chercher_medicament_statut(String statut)
        {
            SqlConnection conn = DB_Connexion.getInstance();
            String query = "SELECT * FROM medicament WHERE statut=@statut";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@statut", statut);

            SqlDataReader rdr = cmd.ExecuteReader();
            Dictionary<int, Medicament_Standard> liste = new Dictionary<int, Medicament_Standard>();

            while (rdr.Read())
            {
                int id = int.Parse(rdr["id_medicament"].ToString());
                Medicament_Standard medic_std = new Medicament_Standard(
                    id,
                    rdr["nom_Medicament"].ToString().ToLower(),
                    rdr["description"].ToString(),
                    rdr["dosage"].ToString(),
                    rdr["statut"].ToString(),
                    rdr["prescription_requise"].ToString(),
                    rdr["forme_pharmaceutique"].ToString(),
                    float.Parse(rdr["prix_unitaire"].ToString()),
                    float.Parse(rdr["prix_achat"].ToString()),
                    List_Categorie.getCategorie(int.Parse(rdr["id_categorie"].ToString())),
                    DateTime.Parse(rdr["date_creation"].ToString()),
                    DateTime.Parse(rdr["date_modification"].ToString())
                );
                liste.Add(id, medic_std);
            }
            rdr.Close();

            return liste.Count > 0 ? liste : null;
        }

        public Dictionary<int, Medicament_Standard> chercher_medicament_forme_pharmaceutique(String forme_pharmaceutique)
        {
            SqlConnection conn = DB_Connexion.getInstance();
            String query = "SELECT * FROM medicament WHERE forme_pharmaceutique=@forme_pharmaceutique";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@forme_pharmaceutique", forme_pharmaceutique.ToLower());

            SqlDataReader rdr = cmd.ExecuteReader();
            Dictionary<int, Medicament_Standard> liste = new Dictionary<int, Medicament_Standard>();

            while (rdr.Read())
            {
                int id = int.Parse(rdr["id_medicament"].ToString());
                Medicament_Standard medic_std = new Medicament_Standard(
                    id,
                    rdr["nom_Medicament"].ToString().ToLower(),
                    rdr["description"].ToString(),
                    rdr["dosage"].ToString(),
                    rdr["statut"].ToString(),
                    rdr["prescription_requise"].ToString(),
                    rdr["forme_pharmaceutique"].ToString(),
                    float.Parse(rdr["prix_unitaire"].ToString()),
                    float.Parse(rdr["prix_achat"].ToString()),
                    List_Categorie.getCategorie(int.Parse(rdr["id_categorie"].ToString())),
                    DateTime.Parse(rdr["date_creation"].ToString()),
                    DateTime.Parse(rdr["date_modification"].ToString())
                );
                liste.Add(id, medic_std);
            }
            rdr.Close();

            return liste.Count > 0 ? liste : null;
        }

        public int modifier_medicament(int id_medicament, String nom_Medicament, String description, String dosage,
            String statut, String prescription_requise, String forme_pharmaceutique, float prix_unitaire,
            float prix_achat, Categorie categorie)
        {
            if (!liste_medic_std.ContainsKey(id_medicament))
            {
                return -1; // medicament n'existe pas
            }
            return liste_medic_std[id_medicament].Changer_Information(nom_Medicament, description, dosage,
                statut, prescription_requise, forme_pharmaceutique, prix_unitaire, prix_achat, categorie);
        }

        public Medicament_Standard get_medicament_by_id(int id)
        {
            return liste_medic_std.ContainsKey(id) ? liste_medic_std[id] : null;
        }

        public Dictionary<int, Medicament_Standard> get_all_medicaments()
        {
            return liste_medic_std;
        }
    }
}