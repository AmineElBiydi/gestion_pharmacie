using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_pharmacie
{
    internal class liste_medicament
    {
        Dictionary<string, medicament> liste_medic; // key: "id_medicament-id_pharmacie-numero_lot"
        liste_Medicament_standard liste_std;

        public liste_medicament()
        {
            liste_std = new liste_Medicament_standard();
            liste_medic = get_all();
        }

        private Dictionary<string, medicament> get_all()
        {
            SqlConnection conn = DB_Connexion.getInstance();
            String query = "SELECT * FROM contenir";
            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataReader rdr = cmd.ExecuteReader();
            Dictionary<string, medicament> liste = new Dictionary<string, medicament>();

            while (rdr.Read())
            {
                int id_medicament = int.Parse(rdr["id_medicament"].ToString());
                int id_pharmacie = int.Parse(rdr["id_pharmacie"].ToString());
                String numero_lot = rdr["numero_lot"].ToString();

                Medicament_Standard medic_std = liste_std.get_medicament_by_id(id_medicament);

                medicament medic = new medicament(
                    medic_std,
                    id_pharmacie,
                    int.Parse(rdr["quantite_stock"].ToString()),
                    int.Parse(rdr["seuil_alerte"].ToString()),
                    DateTime.Parse(rdr["date_peremption"].ToString()),
                    rdr["reference_medicament"].ToString(),
                    numero_lot
                );

                string key = $"{id_medicament}-{id_pharmacie}-{numero_lot}";
                liste.Add(key, medic);
            }
            rdr.Close();

            return liste;
        }

        public int ajouter_medicament(int id_medicament, int id_pharmacie, int quantite_stock,
            int seuil_alerte, DateTime date_peremption, String reference_medicament, String numero_lot)
        {
            string key = $"{id_medicament}-{id_pharmacie}-{numero_lot}";

            if (liste_medic.ContainsKey(key))
            {
                return -1; // medicament existe deja
            }

            SqlConnection conn = DB_Connexion.getInstance();
            String query = "INSERT INTO contenir (id_medicament, id_pharmacie, quantite_stock, seuil_alerte, " +
                "date_peremption, reference_medicament, numero_lot) " +
                "VALUES (@id_medicament, @id_pharmacie, @quantite_stock, @seuil_alerte, @date_peremption, " +
                "@reference_medicament, @numero_lot)";
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@id_medicament", id_medicament);
            cmd.Parameters.AddWithValue("@id_pharmacie", id_pharmacie);
            cmd.Parameters.AddWithValue("@quantite_stock", quantite_stock);
            cmd.Parameters.AddWithValue("@seuil_alerte", seuil_alerte);
            cmd.Parameters.AddWithValue("@date_peremption", date_peremption.Date);
            cmd.Parameters.AddWithValue("@reference_medicament", reference_medicament.ToLower());
            cmd.Parameters.AddWithValue("@numero_lot", numero_lot.ToLower());

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Medicament_Standard medic_std = liste_std.get_medicament_by_id(id_medicament);
                medicament new_medic = new medicament(medic_std, id_pharmacie, quantite_stock,
                    seuil_alerte, date_peremption, reference_medicament, numero_lot);
                liste_medic.Add(key, new_medic);
                return 1;
            }
            return -2;
        }

        public List<medicament> chercher_par_nom(String nom_medicament)
        {
            Dictionary<int, Medicament_Standard> medicaments_std = liste_std.chercher_medicament_nom(nom_medicament);

            if (medicaments_std == null || medicaments_std.Count == 0)
            {
                return null;
            }

            List<medicament> resultat = new List<medicament>();
            foreach (var medic in liste_medic.Values)
            {
                if (medicaments_std.ContainsKey(medic.get_medicament_standard().get_id_medicament()))
                {
                    resultat.Add(medic);
                }
            }

            return resultat.Count > 0 ? resultat : null;
        }

        public List<medicament> chercher_par_statut(String statut)
        {
            Dictionary<int, Medicament_Standard> medicaments_std = liste_std.chercher_medicament_statut(statut);

            if (medicaments_std == null || medicaments_std.Count == 0)
            {
                return null;
            }

            List<medicament> resultat = new List<medicament>();
            foreach (var medic in liste_medic.Values)
            {
                if (medicaments_std.ContainsKey(medic.get_medicament_standard().get_id_medicament()))
                {
                    resultat.Add(medic);
                }
            }

            return resultat.Count > 0 ? resultat : null;
        }

        public List<medicament> chercher_par_forme_pharmaceutique(String forme_pharmaceutique)
        {
            Dictionary<int, Medicament_Standard> medicaments_std = liste_std.chercher_medicament_forme_pharmaceutique(forme_pharmaceutique);

            if (medicaments_std == null || medicaments_std.Count == 0)
            {
                return null;
            }

            List<medicament> resultat = new List<medicament>();
            foreach (var medic in liste_medic.Values)
            {
                if (medicaments_std.ContainsKey(medic.get_medicament_standard().get_id_medicament()))
                {
                    resultat.Add(medic);
                }
            }

            return resultat.Count > 0 ? resultat : null;
        }

        public List<medicament> chercher_par_pharmacie(int id_pharmacie)
        {
            List<medicament> resultat = new List<medicament>();
            foreach (var medic in liste_medic.Values)
            {
                if (medic.get_id_pharmacie() == id_pharmacie)
                {
                    resultat.Add(medic);
                }
            }
            return resultat.Count > 0 ? resultat : null;
        }

        public List<medicament> chercher_medicaments_expires()
        {
            List<medicament> resultat = new List<medicament>();
            DateTime today = DateTime.Now.Date;

            foreach (var medic in liste_medic.Values)
            {
                if (medic.get_date_peremption() <= today)
                {
                    resultat.Add(medic);
                }
            }
            return resultat.Count > 0 ? resultat : null;
        }

        public List<medicament> chercher_medicaments_alerte_stock()
        {
            List<medicament> resultat = new List<medicament>();

            foreach (var medic in liste_medic.Values)
            {
                if (medic.get_quantite_stock() <= medic.get_seuil_alerte())
                {
                    resultat.Add(medic);
                }
            }
            return resultat.Count > 0 ? resultat : null;
        }

        public medicament get_medicament(int id_medicament, int id_pharmacie, String numero_lot)
        {
            string key = $"{id_medicament}-{id_pharmacie}-{numero_lot}";
            return liste_medic.ContainsKey(key) ? liste_medic[key] : null;
        }

        public Dictionary<string, medicament> get_all_medicaments()
        {
            return liste_medic;
        }
    }

}
