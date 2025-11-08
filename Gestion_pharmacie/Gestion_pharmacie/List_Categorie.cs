using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_pharmacie
{
    internal class List_Categorie
    {
        static Dictionary<int, Categorie> liste_categorie = new Dictionary<int, Categorie>();

        public static Categorie getCategorie(int id_categorie)
        {
            if(liste_categorie.Count == 0)
            {
                liste_categorie = get_all_categories(); 
            }
            return liste_categorie[id_categorie];
        }

        private static Dictionary<int, Categorie> get_all_categories()
        {
            SqlConnection conn = DB_Connexion.getInstance();
            String query = "SELECT * FROM categorie";
            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataReader rdr = cmd.ExecuteReader();
            Dictionary<int, Categorie> liste = new Dictionary<int, Categorie>();
            if (rdr.Read())
            {
                Categorie categorie;
                int id;
                string nom_categorie, description;
                foreach (var item in rdr)
                {
                    id = int.Parse(rdr["Id_Categorie"].ToString());
                    nom_categorie = rdr["nom_categorie"].ToString();
                    description = rdr["discription"].ToString();
                    categorie = new Categorie(id,nom_categorie,description);
                    liste.Add(id, categorie);
                }
                return liste;
            }
            return null;
        }
    }
}
