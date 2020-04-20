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
        public GestionnaireCook()
        {

            InitializeComponent();
            DBClass database = new DBClass();
            DataTable recette = database.ExecuteCommand("select recette.nom,commande.recetteID , sum(commande.quantite) as qt from commande join recette order by qt limit 5;");
            dtgTop5.DataContext = recette.DefaultView;
            tbCdrOr.Text = "Cdr OR : " + database.GetCdrOr();
            User cdrSemaine = database.GetCdrWeek();
            tbCdrWeekly.Text = "Cdr de la semaine : " + cdrSemaine.nom + " (" + cdrSemaine.username + ")";

            //Init cdr OR
            DataTable recette5cdrOr = database.ExecuteCommand("select recette.recetteID,recette.nom,recette.prix,t1.qt from recette join (select commande.recetteID, sum(commande.quantite) as qt from commande join cdrOr) as t1 order by t1.qt desc limit 5");
            dtCdrOrRecette.DataContext = recette5cdrOr.DefaultView;
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
    }
}
