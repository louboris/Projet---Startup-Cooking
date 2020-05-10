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
    /// Logique d'interaction pour Evaluation.xaml
    /// </summary>
    public partial class Evaluation : Window
    {
        DBClass evalutationDB = new DBClass();
        public Evaluation()
        {
            InitializeComponent();

        }

        private void BtUtilisateurs_Click(object sender, RoutedEventArgs e)
        {
            dtGridEval.DataContext = evalutationDB.ExecuteCommand("select * from client");
        }

        private void BtRecettes_Click(object sender, RoutedEventArgs e)
        {
            dtGridEval.DataContext = evalutationDB.ExecuteCommand("select * from recette");
        }

        private void BtCommandes_Click(object sender, RoutedEventArgs e)
        {
            dtGridEval.DataContext = evalutationDB.ExecuteCommand("select * from commande");
        }

        private void BtIngredient_Click(object sender, RoutedEventArgs e)
        {
            dtGridEval.DataContext = evalutationDB.ExecuteCommand("select * from ingredient");
        }

        private void BtIngredient_recette_Click(object sender, RoutedEventArgs e)
        {
            dtGridEval.DataContext = evalutationDB.ExecuteCommand("select * from ingredient_recette");
        }
    }
}
