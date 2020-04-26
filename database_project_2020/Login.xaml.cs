using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace database_project_2020
{
    /// <summary>
    /// Logique d'interaction pour Login.xaml
    /// </summary>

    
    

    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            DBClass databaseLogin = new DBClass();

            try
            {
                if(databaseLogin.Connection(txUsername.Text, txPasseword.Password) == true)
                {
                    User utilisateur = databaseLogin.GetUser(txUsername.Text, txPasseword.Password);
                    if(utilisateur.username == "admin")
                    {
                        GestionnaireCook gestionnaire = new GestionnaireCook();
                        gestionnaire.Show();
                        this.Close();
                    }
                    else {
                        MainWindow dashBoard = new MainWindow(utilisateur);
                        dashBoard.Show();
                        this.Close();
                    }
                    
                }
                else
                {
                    MessageBox.Show("Incorrect");
                }


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void BtnNewUser_Click(object sender, RoutedEventArgs e)
        {
            Inscription nouvelUtilisateur = new Inscription();
            nouvelUtilisateur.Show();
        }

        private void BtDEMO_Click(object sender, RoutedEventArgs e)
        {
            ModeDemo demo = new ModeDemo();
            demo.Show();
            this.Close();
        }
    }
}
