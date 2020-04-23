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
            dt.WriteXml("test.xml");
            return utilisateurC;
        }
        public bool CreateUser(string username,string password,string nom,string numero)
        {
            string query = "INSERT into client(username,password,nom,numero) values( @username , SHA1( @password ), @nom , @numero );";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@numero", numero);

            connection.Open();
            try
            {
                cmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                connection.Close();
                return false;
            }
            
        }
        public string GetCdrOr()
        {
            string cdrOR = "";
            String query = "select * from cdrOr";
            DataTable dt = ExecuteCommand(query);
            try
            {
                cdrOR = dt.Rows[0][0].ToString();
            }
            catch
            {

            }
            if (cdrOR == "") cdrOR = "Il n'y a pas de cdrOR";
            return cdrOR;
        }

        public User GetCdrWeek()
        {

            String query = "select * from bestWeekCreator";
            DataTable dt = ExecuteCommand(query);
            try
            {
                User cdrOR = new User(dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), Convert.ToInt32(dt.Rows[0][2].ToString()), true);
                return cdrOR;
            }
            catch
            {
                User cdrOR = new User("None","None",0);

                return cdrOR;
            }

            
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                connection.Close();
            }
            connection.Close();
        }
        public void EraseRecipe(int IDrecette)
        {
            String query = "CALL SupprimerRecette( @IDrecette );";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@IDrecette", IDrecette);

            connection.Open();
            try
            {
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                connection.Close();
                
            }
        }
        public void EraseCdrRecette(int IDCdr)
        {
            String query = "CALL SupprimerCdr( @IDCdr );";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@IDCdr", IDCdr);

            connection.Open();
            try
            {
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                connection.Close();

            }
        }
        public void DownCdrRecette(int IDCdr)
        {
            String query = "CALL DownGradeCdr( @IDCdr );";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@IDCdr", IDCdr);

            connection.Open();
            try
            {
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                connection.Close();

            }
        }

        public string GetNbClient()
        {
            string nbClient = "";
            String query = "select count(*) from client";
            DataTable dt = ExecuteCommand(query);
            try
            {
                nbClient = dt.Rows[0][0].ToString();
            }
            catch
            {

            }
            if (nbClient == "") nbClient = "None";
            return nbClient;
        }
        public string GetNbRecette()
        {
            string nbRecette = "";
            String query = "select count(*) from recette";
            DataTable dt = ExecuteCommand(query);
            try
            {
                nbRecette = dt.Rows[0][0].ToString();
            }
            catch
            {

            }
            if (nbRecette == "") nbRecette = "None";
            return nbRecette;
        }

        public string GetNbCdr()
        {
            string nbCdr = "";
            String query = "select count(*) from client where createur = 1";
            DataTable dt = ExecuteCommand(query);
            try
            {
                nbCdr = dt.Rows[0][0].ToString();
            }
            catch
            {

            }
            if (nbCdr == "") nbCdr = "None";
            return nbCdr;
        }

        public void MajHebdo()
        {
            String query = "CALL ReaHebdo();";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.CommandType = CommandType.Text;
        

            connection.Open();
            try
            {
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Mise à jour effectuée.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                connection.Close();

            }
        }

    }
}
