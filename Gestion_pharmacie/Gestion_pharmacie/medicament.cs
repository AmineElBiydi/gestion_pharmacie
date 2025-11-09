using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_pharmacie
{
    internal class medicament
    {
        int id_pharmacie, quantite_stock, seuil_alerte;
        DateTime date_peremption;
        String reference_medicament, numero_lot;
        Medicament_Standard medicament_standard;

        public medicament(Medicament_Standard medicament_standard, int id_pharmacie, int quantite_stock,
            int seuil_alerte, DateTime date_peremption, String reference_medicament, String numero_lot)
        {
            this.medicament_standard = medicament_standard;
            this.id_pharmacie = id_pharmacie;
            this.quantite_stock = quantite_stock;
            this.seuil_alerte = seuil_alerte;
            this.date_peremption = date_peremption;
            this.reference_medicament = reference_medicament;
            this.numero_lot = numero_lot;
        }

        public int modifie_medicament(int quantite_stock, int seuil_alerte, DateTime date_peremption,
            String reference_medicament)
        {
            SqlConnection conn = DB_Connexion.getInstance();
            string query = "UPDATE contenir SET quantite_stock=@quantite_stock, seuil_alerte=@seuil_alerte, " +
                "date_peremption=@date_peremption, reference_medicament=@reference_medicament " +
                "WHERE id_medicament=@id_medicament AND id_pharmacie=@id_pharmacie AND numero_lot=@numero_lot";
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@id_pharmacie", id_pharmacie);
            cmd.Parameters.AddWithValue("@numero_lot", numero_lot.ToLower());
            cmd.Parameters.AddWithValue("@id_medicament", medicament_standard.get_id_medicament());
            cmd.Parameters.AddWithValue("@reference_medicament", reference_medicament.ToLower());
            cmd.Parameters.AddWithValue("@date_peremption", date_peremption.Date);
            cmd.Parameters.AddWithValue("@seuil_alerte", seuil_alerte);
            cmd.Parameters.AddWithValue("@quantite_stock", quantite_stock);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                this.quantite_stock = quantite_stock;
                this.seuil_alerte = seuil_alerte;
                this.date_peremption = date_peremption;
                this.reference_medicament = reference_medicament;
                return 1;
            }
            return 0;
        }

        // Getters
        public Medicament_Standard get_medicament_standard() { return medicament_standard; }
        public int get_id_pharmacie() { return id_pharmacie; }
        public int get_quantite_stock() { return quantite_stock; }
        public int get_seuil_alerte() { return seuil_alerte; }
        public DateTime get_date_peremption() { return date_peremption; }
        public String get_reference_medicament() { return reference_medicament; }
        public String get_numero_lot() { return numero_lot; }

        // Setters
        public void set_quantite_stock(int quantite) { this.quantite_stock = quantite; }
        public void set_seuil_alerte(int seuil) { this.seuil_alerte = seuil; }
    }

    
}