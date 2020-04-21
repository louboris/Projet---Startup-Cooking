using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logique d'interaction pour ModeDemo.xaml
    /// </summary>
    public partial class ModeDemo : Window
    {
        DBClass database = new DBClass();
        public ModeDemo()
        {
            InitializeComponent();
            RefreshPage1();
        }
        private void RefreshPage1()
        {
            tbNbClient.Text = "Nombre de client dans la base de donnée : " + database.GetNbClient();
        }
        private void RefreshPage2()
        {
            tbNbCdr.Text = "Nombre de Cdr : " + database.GetNbCdr();
            DataTable nbcommandeCdr = database.ExecuteCommand("select client.username, sum(commande.quantite) as TotalCommande from commande join recette join client on commande.recetteID = recette.recetteID and recette.createur = client.numero group by client.numero;");
            dtNomCdrNbRecette.DataContext = nbcommandeCdr.DefaultView;
        }
        private void RefreshPage3()
        {
            tbNbRecette.Text = "Il y a " + database.GetNbRecette() + " recette.";
        }
        private void RefreshPage4()
        {

        }
        private void RefreshPage5()
        {
            dtlisteingredient.DataContext = database.ExecuteCommand("select * from ingredient;");
        }
        private void AffichageListeRecetteProduit(String produit)
        {
            dtListIngrRecetteQt.DataContext = database.ExecuteCommand("select * from recette join (select recetteID from ingredient_recette join ingredient on ingredient_recette.ingredientID = ingredient.ingredientID and ingredient.nom = '"+ produit + "') as t1 on recette.recetteID = t1.recetteID; ");
        }
        private void BtPage1_2_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage2();
            Page1.Visibility = Visibility.Hidden;
            btPage1_2.Visibility = Visibility.Hidden;
            btPage2_1.Visibility = Visibility.Visible;
            btPage2_3.Visibility = Visibility.Visible;
            Page2.Visibility = Visibility.Visible;
        }

        private void BtPage2_1_Click(object sender, RoutedEventArgs e)
        {
            Page1.Visibility = Visibility.Visible;
            btPage1_2.Visibility = Visibility.Visible;
            btPage2_1.Visibility = Visibility.Hidden;
            btPage2_3.Visibility = Visibility.Hidden;
            Page2.Visibility = Visibility.Hidden;
        }

        private void BtPage2_3_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage3();
            Page3.Visibility = Visibility.Visible;
            btPage3_2.Visibility = Visibility.Visible;
            btPage3_4.Visibility = Visibility.Visible;
            btPage2_1.Visibility = Visibility.Hidden;
            btPage2_3.Visibility = Visibility.Hidden;
            Page2.Visibility = Visibility.Hidden;
        }

        private void BtPage3_2_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage2();
            Page2.Visibility = Visibility.Visible;
            btPage2_1.Visibility = Visibility.Visible;
            btPage2_3.Visibility = Visibility.Visible;
            btPage3_2.Visibility = Visibility.Hidden;
            btPage3_4.Visibility = Visibility.Hidden;
            Page3.Visibility = Visibility.Hidden;
        }

        private void BtPage3_4_Click(object sender, RoutedEventArgs e)
        {
            Page4.Visibility = Visibility.Visible;
            btPage4_3.Visibility = Visibility.Visible;
            btPage4_5.Visibility = Visibility.Visible;
            btPage3_2.Visibility = Visibility.Hidden;
            btPage3_4.Visibility = Visibility.Hidden;
            Page3.Visibility = Visibility.Hidden;
        }

        private void BtPage4_3_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage3();
            Page3.Visibility = Visibility.Visible;
            btPage3_2.Visibility = Visibility.Visible;
            btPage3_4.Visibility = Visibility.Visible;
            btPage4_3.Visibility = Visibility.Hidden;
            btPage4_5.Visibility = Visibility.Hidden;
            Page4.Visibility = Visibility.Hidden;
        }

        private void BtPage4_5_Click(object sender, RoutedEventArgs e)
        {
            RefreshPage5();
            Page5.Visibility = Visibility.Visible;
            btPage5_4.Visibility = Visibility.Visible;
            
            btPage4_3.Visibility = Visibility.Hidden;
            btPage4_5.Visibility = Visibility.Hidden;
            Page4.Visibility = Visibility.Hidden;
        }

        private void BtPage5_4_Click(object sender, RoutedEventArgs e)
        {
            Page4.Visibility = Visibility.Visible;
            btPage4_3.Visibility = Visibility.Visible;
            btPage4_5.Visibility = Visibility.Visible;
            btPage5_4.Visibility = Visibility.Hidden;
           
            Page5.Visibility = Visibility.Hidden;
        }

        private void BtProduit_Click(object sender, RoutedEventArgs e)
        {
            AffichageListeRecetteProduit(tbxProduit.Text);
        }
    }
}
