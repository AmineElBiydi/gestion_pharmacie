using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_pharmacie
{
    internal class DB_Connexion
    {
        static SqlConnection conn = null;
        public static SqlConnection getInstance()
        {
            if (conn == null)
            {
                conn = new SqlConnection("Data Source=DESKTOP-4KJH1A1\\SQLEXPRESS;Initial Catalog=Gestion_pharmacie;Integrated Security=True");
                conn.Open();
            }
            return conn;
        }
    }
}
