using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_pharmacie
{
    internal class Categorie
    {
        int Id_Categorie ;
        string nom_categorie , discription;

        public Categorie( int Id_Categorie , string nom_categorie , string discription)
        {
            this.Id_Categorie = Id_Categorie;
            this.nom_categorie = nom_categorie;
            this.discription = discription;
        }
        public int get_Id_Categorie()
        {
            return Id_Categorie;
        }
    }
}
