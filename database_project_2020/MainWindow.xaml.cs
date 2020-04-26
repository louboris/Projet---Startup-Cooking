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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace database_project_2020
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Variable globale recette active
        public Recette Recette_Active { get; set; }
        public Panier Panier_Actif { get; set; }
        DBClass databaseMain = new DBClass();
        User utilisateurActif;
        DataTable dtIngredient = new DataTable();
        public MainWindow(User utilisateur)
        {   
            //ff
            utilisateurActif = utilisateur;
            InitializeComponent();
            Panier_Actif = new Panier();
            List<Recette> items = new List<Recette>();

            DBClass database = new DBClass();
            DataTable recette = database.ExecuteCommand("select * from recette");
            dtGrid.DataContext = recette;

            

            foreach (DataRow row in recette.Rows)
            {

                items.Add(new Recette() { Id = Convert.ToInt32(row[0].ToString()), Nom = row[1].ToString(), TypeR = row[2].ToString(), Descriptif = row[3].ToString(), Prix = Convert.ToInt32(row[4].ToString()) });

            }

            dtGrid.ItemsSource = items;

            if(utilisateur.createur == true)
            {
                CreatorSpace.Visibility = Visibility.Visible;
            }


            dtIngredient = database.ExecuteCommand("select * from ingredient");
            dtIngredient.Columns.Add("Quantite", typeof(int));
            dgIngredient.DataContext = dtIngredient;

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DtGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
                
        private void Recette(Recette rec)
        {
            Recette_Active = rec;

            Recette_Liste.Visibility = Visibility.Hidden;
            BackRecette.Visibility = Visibility.Visible;
            Desc.Text = rec.Descriptif;
            prixAchat.Text = Convert.ToString(Recette_Active.Prix);
            NomRecette.Text = Recette_Active.Nom;
            try
            {
                txUniteAchat.Text = Convert.ToString(Panier_Actif.caddy[Recette_Active]);
            }
            catch
            {
                txUniteAchat.Text = "0";
            }
            Recette_Descriptif.Visibility = Visibility.Visible;

        }

        private void BtnAchat_Click(object sender, RoutedEventArgs e)
        {
            Panier_Actif.AddPanier(Recette_Active, Convert.ToInt32(txUniteAchat.Text));
        }
        private void BtnPanier_Click(object sender, RoutedEventArgs e)
        {
            Recette_Liste.Visibility = Visibility.Hidden;
            BackPanier.Visibility = Visibility.Visible;
            Panier.Visibility = Visibility.Hidden;
            Panier_Liste.Visibility = Visibility.Visible;
            Finaliser.Visibility = Visibility.Visible;
            List<Panier> items = new List<Panier>();

            dtGrid_Panier.ItemsSource = Panier_Actif.PanierComplete();

        }
        private void BtnRecette_Click(object sender, RoutedEventArgs e)
        {
            Recette_Descriptif.Visibility = Visibility.Hidden;
            Recette_Liste.Visibility = Visibility.Visible;
            BackRecette.Visibility = Visibility.Hidden;
            Panier.Visibility = Visibility.Visible;
        }
        private void BtnBackPanier_Click(object sender, RoutedEventArgs e)
        {
            Panier_Liste.Visibility = Visibility.Hidden;
            Recette_Liste.Visibility = Visibility.Visible;
            BackPanier.Visibility = Visibility.Hidden;
            Panier.Visibility = Visibility.Visible;
            Finaliser.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Menu recette navigable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Recette actual = ((ListViewItem)sender).Content as Recette;


            //MessageBox.Show(actual.Nom);

            Recette(actual);
            Panier.Visibility = Visibility.Hidden;

        }
        private void DtGrid_ItemActivate(Object sender, EventArgs e)
        {

            MessageBox.Show("You are in the ListView.ItemActivate event.");
        }

        private void CreatorSpace_Click(object sender, RoutedEventArgs e)
        {
            Nouvelle_Recette.Visibility = Visibility.Visible;
            BackNouvelleRecette.Visibility = Visibility.Visible;

        }

        private void BackNouvelleRecette_Click(object sender, RoutedEventArgs e)
        {
            Nouvelle_Recette.Visibility = Visibility.Hidden;
            Recette_Liste.Visibility = Visibility.Visible;
            BackNouvelleRecette.Visibility = Visibility.Hidden;
        }


        private void BtEnvoiNvRecette_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                databaseMain.AddRecette(txNomRecette.Text, txTypeRecette.Text, txDescriptifNouvelle.Text, Convert.ToInt32(txPrixNouvelle.Text), utilisateurActif.numero);
                txNomRecette.Text = "";
                txTypeRecette.Text = "";
                txDescriptifNouvelle.Text = "";
                txPrixNouvelle.Text = "";

                MessageBox.Show("Recette envoyée avec succès");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Finaliser_Click(object sender, RoutedEventArgs e)
        {
            databaseMain.PasserCommande(Panier_Actif.caddy, utilisateurActif.numero);
            Finaliser.Visibility = Visibility.Hidden;
            Panier_Actif.caddy = new Dictionary<Recette, int>();
            dtGrid_Panier.ItemsSource = Panier_Actif.PanierComplete();
        }

        private void BackRecetteFinaliser_Click(object sender, RoutedEventArgs e)
        {

        }
        //        Test
        
        
        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {/*
            foreach (MyObject item in e.RemovedItems)
            {
                lstMyObject.Remove(item);
            }

            foreach (MyObject item in e.AddedItems)
            {
                lstMyObject.Add(item);
            }*/
        }

        private void Quantite_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void BtIngredient_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("test");
        }

        private void AjoutIngredient_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
