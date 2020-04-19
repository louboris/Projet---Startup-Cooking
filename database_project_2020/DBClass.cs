using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace database_project_2020
{
    class DBClass
    {
        // Faire constructeur avec l'initialisation de la connection + Faire des fonctions qui renvoient des DataTable
        public MySqlConnection connection;
        public DBClass()
        {
            string connectionString = "SERVER=localhost;DATABASE=cooking;UID=root;PASSWORD=22011997bobolou;";

            this.connection = new MySqlConnection(connectionString);


        }

        public DataTable ExecuteCommand(string commande)
        {
            MySqlCommand cmd = new MySqlCommand(commande, connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            return dt;
        }
        public User GetUser(string login, string password)
        {
            String query = "select * from client where password = sha1(@password) and username = @username";

            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@username", login);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            connection.Close();
            DataTable utilisateurConnect = dt;
            bool createur = false;
            if (Convert.ToInt32(utilisateurConnect.Rows[0][4].ToString()) == 0)
            {
                createur = true;
            }
            User utilisateurC = new User(utilisateurConnect.Rows[0][0].ToString(), utilisateurConnect.Rows[0][2].ToString(), Convert.ToInt32(utilisateurConnect.Rows[0][3].ToString()), createur);

            return utilisateurC;
        }
        public void AddRecette(string nom, string type, string descriptif, int prix, int createur)
        {
            String query = "insert into recette(nom,type,descriptif,prix,createur) values ( @nom ,@type , @descriptif , @prix , @createur );";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@descriptif", descriptif);
            cmd.Parameters.AddWithValue("@prix", prix);
            cmd.Parameters.AddWithValue("@createur", createur);
            connection.Open();
            try
            {
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch
            {
                connection.Close();
            }
        }
        public bool Connection(string login, string password)
        {
            bool exist = false;
            String query = "select count(1) from client where password = sha1(@password) and username = @username";

            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@username", login);
            connection.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            if (count == 1) exist = true;
            connection.Close();
            return exist;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True si la commande à été effectue False sinon</returns>
        public void PasserCommande(Dictionary<Recette, int> commande, int numero)
        {
            String query = "call PasserCommande( @Numero , @IDRecette , @Quantite , @Prix )";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            connection.Open();
            try
            {
                cmd.Parameters.Add(new MySqlParameter("@Numero", 0));
                cmd.Parameters.Add(new MySqlParameter("@IDRecette", 0));
                cmd.Parameters.Add(new MySqlParameter("@Quantite", 0));
                cmd.Parameters.Add(new MySqlParameter("@prix", 0));
                foreach (KeyValuePair<Recette, int> entry in commande)
                {





                    cmd.Parameters["@Numero"].Value = numero;
                    cmd.Parameters["@IDRecette"].Value = entry.Key.Id;
                    cmd.Parameters["@Quantite"].Value = entry.Key.Quantite;
                    cmd.Parameters["@prix"].Value = entry.Key.Prix;
                    cmd.ExecuteNonQuery();


                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
            connection.Close();
        }

    }
}
