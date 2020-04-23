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
    /// Logique d'interaction pour GestionnaireCook.xaml
    /// </summary>
    public partial class GestionnaireCook : Window
    {
        DBClass database = new DBClass();
        public GestionnaireCook()
        {

            InitializeComponent();
            //DBClass database = new DBClass();
            DataTable recette = database.ExecuteCommand("select recette.nom,commande.recetteID , sum(commande.quantite) as qt from commande join recette order by qt limit 5;");
            dtgTop5.DataContext = recette.DefaultView;
            tbCdrOr.Text = "Cdr OR : " + database.GetCdrOr();
            User cdrSemaine = database.GetCdrWeek();
            tbCdrWeekly.Text = "Cdr de la semaine : " + cdrSemaine.nom + " (" + cdrSemaine.username + ")";

            //Init cdr OR
            DataTable recette5cdrOr = database.ExecuteCommand("select recette.recetteID,recette.nom,recette.prix,t1.qt from recette join (select commande.recetteID, sum(commande.quantite) as qt from commande join cdrOr) as t1 order by t1.qt desc limit 5");
            dtCdrOrRecette.DataContext = recette5cdrOr.DefaultView;

            //suite init

            tbLastMaj.Text = "Dernière Maj : " + database.ExecuteCommand("select DATE_FORMAT(majdate, 'Le %d %M %Y') from ingredient order by majdate desc limit 1;").Rows[0][0].ToString();
        }

        private void BtMenu1_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal.Visibility = Visibility.Hidden;
            TableauBordSemaine.Visibility = Visibility.Visible;
            btBackTdBsMP.Visibility = Visibility.Visible;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            btBackTdBsMP.Visibility = Visibility.Hidden;
            MenuPrincipal.Visibility = Visibility.Visible;
            TableauBordSemaine.Visibility = Visibility.Hidden;
        }

        private void BtValidationSuppR_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Etes-vous sûr ? Action irreversible.", "Validation", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    database.EraseRecipe(Convert.ToInt32(tbRecetteIDSupp.Text));
                    break;
                case MessageBoxResult.No:

                    break;

            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BtValidationSuppC_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Etes vous sur de vouloir supprimer le client\nAinsi que les recettes qu'il a crée ?", "Validation", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    database.EraseCdrRecette(Convert.ToInt32(tbSupprimerCuisinier.Text));
                    break;
                case MessageBoxResult.No:

                    break;

            }
        }

        private void BtValidationDownC_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Etes vous sur de vouloir downgrader le CDR ?\nCette action entraine la SUPPRESSION de ses recettes.", "Validation", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    database.DownCdrRecette(Convert.ToInt32(tbSupprimerCuisinier.Text));
                    break;
                case MessageBoxResult.No:

                    break;

            }
        }

        private void MajQt_Click(object sender, RoutedEventArgs e)
        {
            database.MajHebdo();
            tbLastMaj.Text = "Dernière Maj : " + database.ExecuteCommand("select DATE_FORMAT(majdate, 'Le %d %M %Y') from ingredient order by majdate desc limit 1;").Rows[0][0].ToString();
        }

        private void BtListeCommande_Click(object sender, RoutedEventArgs e)
        {
            DataTable commande = database.ExecuteCommand("select * from ListeCommande");
            commande.WriteXml("commande.xml");
        }
    }
}
