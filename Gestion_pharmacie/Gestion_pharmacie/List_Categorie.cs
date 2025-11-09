using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Gestion_pharmacie
{
    internal class List_Categorie
    {
        static Dictionary<int, Categorie> liste_categorie = new Dictionary<int, Categorie>();

        public static Categorie getCategorie(int id_categorie)
        {
            if (liste_categorie.Count == 0)
            {
                liste_categorie = get_all_categories();
            }
            return liste_categorie.ContainsKey(id_categorie) ? liste_categorie[id_categorie] : null;
        }

        private static Dictionary<int, Categorie> get_all_categories()
        {
            SqlConnection conn = DB_Connexion.getInstance();
            String query = "SELECT * FROM categorie";
            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataReader rdr = cmd.ExecuteReader();
            Dictionary<int, Categorie> liste = new Dictionary<int, Categorie>();

            while (rdr.Read())
            {
                int id = int.Parse(rdr["Id_Categorie"].ToString());
                string nom_categorie = rdr["nom_categorie"].ToString();
                string description = rdr["discription"].ToString();
                Categorie categorie = new Categorie(id, nom_categorie, description);
                liste.Add(id, categorie);
            }
            rdr.Close();

            return liste;
        }

        public static int modifie_Categorie(int id, String detailles)
        {
            if (liste_categorie.Count == 0)
            {
                liste_categorie = get_all_categories();
            }
            if (!liste_categorie.ContainsKey(id))
            {
                return -1; // categorie n'existe pas
            }
            return liste_categorie[id].mdifie_Catgorie(detailles);
        }

        public static int ajouter_categorie(String name, String detailles)
        {
            if (categorie_existe(name))
            {
                return -1; // categorie existe deja
            }
            else
            {
                Categorie cat = new Categorie();
                cat = cat.ajouter_Categorie(name, detailles);
                if (cat == null)
                {
                    return -2; // erreur lors de l'ajout
                }
                else
                {
                    if (liste_categorie.Count == 0)
                    {
                        liste_categorie = get_all_categories();
                    }
                    liste_categorie[cat.get_Id_Categorie()] = cat;
                    return 1;
                }
            }
        }

        private static bool categorie_existe(String name)
        {
            SqlConnection conn = DB_Connexion.getInstance();
            String query = "SELECT COUNT(*) FROM categorie WHERE nom_categorie=@nom_categorie";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@nom_categorie", name.ToLower().Trim());

            SqlDataReader rdr = cmd.ExecuteReader();
            int count = 0;
            if (rdr.Read())
            {
                count = int.Parse(rdr[0].ToString());
            }
            rdr.Close();
            return count > 0;
        }

        public static Dictionary<int, Categorie> get_all()
        {
            if (liste_categorie.Count == 0)
            {
                liste_categorie = get_all_categories();
            }
            return liste_categorie;
        }
    }
}
