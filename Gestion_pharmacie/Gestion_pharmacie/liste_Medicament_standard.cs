using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            SqlCommand cmd = new SqlCommand(query,conn);

            SqlDataReader rdr = cmd.ExecuteReader();
            Dictionary<int, Medicament_Standard> liste = new Dictionary<int, Medicament_Standard>() ;
            if (rdr.Read())
            {
                Medicament_Standard medic_std ;
                foreach (var item in rdr)
                {
                    medic_std = new Medicament_Standard(
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
                }
                return liste;
            }
            else
            {
                return null;
            }
        }

        public int ajouter_medicament_standard(String nom_Medicament, String description, String dosage,
                    String statut,String prescription_requise, String forme_pharmaceutique, float prix_unitaire,
                    float prix_achat,Categorie categorie)
        {
            if(medicament_existe(nom_Medicament, dosage, forme_pharmaceutique))
            {
                // we can use exception to handle it better but for now we will just return -1
                return -1;
            }else
            {
                SqlConnection conn = DB_Connexion.getInstance();
                String query = "INSERT INTO medicament (nom_Medicament, description, dosage, statut, prescription_requise, forme_pharmaceutique, prix_unitaire, prix_achat, id_categorie, date_creation, date_modification) " +
                               "VALUES (@nom_Medicament, @description, @dosage, @statut, @prescription_requise, @forme_pharmaceutique, @prix_unitaire, @prix_achat, @id_categorie, @date_creation, @date_modification)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nom_Medicament", nom_Medicament);
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
                    liste_medic_std.Add(chercher_medicament_standard(nom_Medicament, dosage, forme_pharmaceutique), new Medicament_Standard(
                        nom_Medicament,
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
                    // medicament added successfully
                    return 1;
                }
                else
                {
                    // failed to add medicament
                    return -2;
                }
            }
        }

        public bool medicament_standard_existe(String nom_medicament,string dosage, string forme_pharmaceutique)
        {
            if(chercher_medicament_standard(nom_medicament, dosage, forme_pharmaceutique) != -1)
            {
                return true;
            }
            return false;
        }

        public int chercher_medicament_standard(String nom_medicament, string dosage,string forme_pharmaceutique)
        {
            SqlConnection conn = DB_Connexion.getInstance();
            String query = "SELECT id_medicament FROM medicament where nom_medicament = @nom_medicament AND dosage=@dosage AND forme_pharmaceutique=@forme_pharmaceutique";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nom_medicament", @nom_medicament);
            cmd.Parameters.AddWithValue("@dosage", dosage);
            cmd.Parameters.AddWithValue("@forme_pharmaceutique", forme_pharmaceutique);

            SqlDataReader rdr = cmd.ExecuteReader();
            if(rdr.Read())
            {
                return int.Parse(rdr["id_medicament"].ToString());
            }
            else
            {
                return -1; 
            }
        }

        
        public int modifier_medicament (int id_medicament)
        {

        }
    }
}
