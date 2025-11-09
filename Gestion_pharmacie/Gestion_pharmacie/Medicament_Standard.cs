using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Gestion_pharmacie
{
    internal class Medicament_Standard
    {
        int id_medicament;
        String nom_Medicament, description, dosage, statut, prescription_requise, forme_pharmaceutique;
        float prix_unitaire, prix_achat;
        Categorie categorie;
        DateTime date_creation, date_modification;

        public Medicament_Standard(int id_medicament, String nom_Medicament, String description, String dosage,
            String statut, String prescription_requise, String forme_pharmaceutique, float prix_unitaire,
            float prix_achat, Categorie categorie, DateTime date_creation, DateTime date_modification)
        {
            this.id_medicament = id_medicament;
            this.nom_Medicament = nom_Medicament;
            this.description = description;
            this.dosage = dosage;
            this.statut = statut;
            this.prescription_requise = prescription_requise;
            this.forme_pharmaceutique = forme_pharmaceutique;
            this.prix_unitaire = prix_unitaire;
            this.prix_achat = prix_achat;
            this.categorie = categorie;
            this.date_creation = date_creation;
            this.date_modification = date_modification;
        }

        public int Changer_Information(String nom_Medicament, String description, String dosage, String statut,
            String prescription_requise, String forme_pharmaceutique, float prix_unitaire, float prix_achat,
            Categorie categorie)
        {
            SqlConnection conn = DB_Connexion.getInstance();
            string query = "UPDATE medicament SET nom_Medicament=@nom_Medicament, description=@description, " +
                "dosage=@dosage, statut=@statut, prescription_requise=@prescription_requise, " +
                "forme_pharmaceutique=@forme_pharmaceutique, prix_unitaire=@prix_unitaire, prix_achat=@prix_achat, " +
                "id_categorie=@id_categorie, date_modification=@date_modification WHERE id_medicament=@id_medicament";
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
                return 1;
            }
            return 0;
        }

        // Getters
        public int get_id_medicament() { return id_medicament; }
        public String get_nom_Medicament() { return nom_Medicament; }
        public String get_description() { return description; }
        public String get_dosage() { return dosage; }
        public String get_statut() { return statut; }
        public String get_prescription_requise() { return prescription_requise; }
        public String get_forme_pharmaceutique() { return forme_pharmaceutique; }
        public float get_prix_unitaire() { return prix_unitaire; }
        public float get_prix_achat() { return prix_achat; }
        public Categorie get_categorie() { return categorie; }
        public DateTime get_date_creation() { return date_creation; }
        public DateTime get_date_modification() { return date_modification; }
    }
}