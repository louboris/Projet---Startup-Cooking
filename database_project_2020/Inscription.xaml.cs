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
    /// Logique d'interaction pour Inscription.xaml
    /// </summary>
    public partial class Inscription : Window
    {
        DBClass database = new DBClass();
        public Inscription()
        {
            InitializeComponent();
        }

        private void BtnValidation_Click(object sender, RoutedEventArgs e)
        {
            if (database.CreateUser(txUsername.Text, txPasseword.Password, txNom.Text, txNumeroTel.Text))
            {
                MessageBox.Show("Vous êtes inscrit.");
                this.Close();
            }
            else { MessageBox.Show("Une erreur s'est produite,\nVeuillez réessayer."); }
        }
    }
}
