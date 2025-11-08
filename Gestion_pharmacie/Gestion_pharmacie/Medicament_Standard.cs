using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_pharmacie
{
    internal class Medicament_Standard
    {
        String nom_Medicament, description, dosage, statut, prescription_requise, forme_pharmaceutique;
        float prix_unitaire, prix_achat;
        Categorie categorie;
        DateTime date_creation, date_modification;
        
        public Medicament_Standard(String nom_Medicament, String description, String dosage, String statut, String prescription_requise, String forme_pharmaceutique, float prix_unitaire, float prix_achat, Categorie id_categorie, DateTime date_creation, DateTime date_modification)
        {
            this.nom_Medicament = nom_Medicament;
            this.description = description;
            this.dosage = dosage;
            this.statut = statut;
            this.prescription_requise = prescription_requise;
            this.forme_pharmaceutique = forme_pharmaceutique;
            this.prix_unitaire = prix_unitaire;
            this.prix_achat = prix_achat;
            this.categorie = id_categorie;
            this.date_creation = date_creation;
            this.date_modification = date_modification;
        }

        public int Changer_Information(int id_medicament,String nom_Medicament, String description, String dosage, String statut, String prescription_requise, String forme_pharmaceutique, float prix_unitaire, float prix_achat, Categorie categorie)
        {
            SqlConnection conn = DB_Connexion.getInstance();
            string query = "UPDATE medicament SET nom_Medicament=@nom_Medicament, description=@description, dosage=@dosage, " +
                           "statut=@statut, prescription_requise=@prescription_requise, forme_pharmaceutique=@forme_pharmaceutique," +
                           "prix_unitaire=@prix_unitaire, prix_achat=@prix_achat, id_categorie=@id_categorie, " +
                           "date_modification=@date_modification WHERE id_medicament=@id_medicament";
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@id_medicament", id_medicament);
            cmd.Parameters.AddWithValue("@nom_Medicament", nom_Medicament);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@dosage", dosage);
            cmd.Parameters.AddWithValue("@statut", statut);
            cmd.Parameters.AddWithValue("@prescription_requise", prescription_requise);
            cmd.Parameters.AddWithValue("@forme_pharmaceutique", forme_pharmaceutique);
            cmd.Parameters.AddWithValue("@prix_unitaire", prix_unitaire);
            cmd.Parameters.AddWithValue("@prix_achat", prix_achat);
            cmd.Parameters.AddWithValue("@id_categorie", categorie.get_Id_Categorie());
            cmd.Parameters.AddWithValue("@date_modification", DateTime.Now);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                this.nom_Medicament = nom_Medicament;
                this.description = description;
                this.dosage = dosage;
                this.statut = statut;
                this.prescription_requise = prescription_requise;
                this.forme_pharmaceutique = forme_pharmaceutique;
                this.prix_unitaire = prix_unitaire;
                this.prix_achat = prix_achat;
                this.categorie = categorie;
                this.date_modification = DateTime.Now;
                // les donners sont changeed avec success
                return 1;
            }
            // echec de changement des donners
            return 0;

        }

        // GETERS AND SETTERS 
    }
}
