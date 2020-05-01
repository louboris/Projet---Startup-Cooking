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
            DataTable recette = database.ExecuteCommand("select * from recette"); //Chargement de la liste de recette
            dtGrid.DataContext = recette;


            // Ajout des differentes recettes dans la listview
            foreach (DataRow row in recette.Rows)
            {

                items.Add(new Recette() { Id = Convert.ToInt32(row[0].ToString()), Nom = row[1].ToString(), TypeR = row[2].ToString(), Descriptif = row[3].ToString(), Prix = Convert.ToInt32(row[4].ToString()) });

            }

            dtGrid.ItemsSource = items;

            if (utilisateur.createur == true)
            {
                CreatorSpace.Visibility = Visibility.Visible;
            }

            ChargementIngredient();

        }
        
        private void DtGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        /// <summary>
        /// Permet de charger la liste des ingredients lors de la création de recette
        /// </summary>
        private void ChargementIngredient()
        {

            dtIngredient = databaseMain.ExecuteCommand("select * from ingredient");
            dtIngredient.Columns.Add("Quantite", typeof(int));

            dgIngredient.DataContext = dtIngredient;
        }
        /// <summary>
        /// Permet d'afficher le menu de la recette active, menu permettant de selectionner la quantite voulue
        /// </summary>
        /// <param name="rec"></param>
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
        /// <summary>
        /// Bouton permettant d'acceder au createur space
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatorSpace_Click(object sender, RoutedEventArgs e)
        {

            //initialiser solde CDR
            tbSoldeCdr.Text = "Votre solde est de : " + Convert.ToString(utilisateurActif.soldeCdr) + " Cookpoint.";
            Recette_Liste.Visibility = Visibility.Hidden;
            Espace_Cdr.Visibility = Visibility.Visible;
            BackNouvelleRecette.Visibility = Visibility.Visible;
            Panier.Visibility = Visibility.Hidden;

        }

        private void BackNouvelleRecette_Click(object sender, RoutedEventArgs e)
        {
            Visualiser_recette_cdr.Visibility = Visibility.Hidden;
            Espace_Cdr.Visibility = Visibility.Hidden;
            Recette_Liste.Visibility = Visibility.Visible;
            BackNouvelleRecette.Visibility = Visibility.Hidden;
            Panier.Visibility = Visibility.Visible;
            Recette_Liste.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Permet d'envoyer une nouvelle recette
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            databaseMain.AddIngredient_Recette(dtIngredient, databaseMain.GetLastRecipe());
        }
        /// <summary>
        /// Permet de finaliser la commande, de decompter du solde CDR et d'envoyer la commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Finaliser_Click(object sender, RoutedEventArgs e)
        {
            string message = "";
            int soldeCdrFinal = utilisateurActif.soldeCdr - Panier_Actif.prixTotal;
            int total = Panier_Actif.prixTotal - utilisateurActif.soldeCdr;

            if (soldeCdrFinal <= 0 && utilisateurActif.soldeCdr != 0)
            {
                soldeCdrFinal = 0;
                message = "Acceptez-vous le paiement ?\nAprès déduction de votre solde CDR veillez régler : ";
            }
            if (soldeCdrFinal > 0 && total <= 0)
            {
                total = 0;
            }
            if (utilisateurActif.soldeCdr == 0)
            {
                soldeCdrFinal = 0;
            }

            else
            {
                utilisateurActif.soldeCdr = soldeCdrFinal;
                message = "Acceptez - vous le paiement ? Total : ";
            }
            MessageBoxResult resultat = MessageBox.Show(message + Convert.ToString(total) + "CookPoint", "Paiment", MessageBoxButton.YesNo);
            switch (resultat)
            {
                case MessageBoxResult.Yes:
                    databaseMain.PasserCommande(Panier_Actif.caddy, utilisateurActif.numero);
                    Finaliser.Visibility = Visibility.Hidden;
                    Panier_Actif.caddy = new Dictionary<Recette, int>();
                    dtGrid_Panier.ItemsSource = Panier_Actif.PanierComplete();
                    utilisateurActif.soldeCdr = soldeCdrFinal;
                    databaseMain.SetCdrSolde(utilisateurActif.username, soldeCdrFinal);
                    break;
                case MessageBoxResult.No:
                    break;

            }

        }

 
        private void BtIngredient_Click(object sender, RoutedEventArgs e)
        {
            Ajout_ingredient.Visibility = Visibility.Hidden;
            Nouvelle_Recette.Visibility = Visibility.Visible;
        }

        private void AjoutIngredient_Click(object sender, RoutedEventArgs e)
        {
            Creation_ingredient.Visibility = Visibility.Visible;
            AjoutIngredient.Visibility = Visibility.Hidden;
        }

        private void BtCreerIngredient_Click(object sender, RoutedEventArgs e)
        {
            databaseMain.AddIngredient(Nom_Ingredient.Text, ListeCategorieIngr.Text, cbUnite.Text);
            Nom_Ingredient.Text = "";

            Creation_ingredient.Visibility = Visibility.Hidden;
            AjoutIngredient.Visibility = Visibility.Visible;
            ChargementIngredient();
        }
        /// <summary>
        /// Permet de charger une liste de categorie d'ingredient predefinie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListeCategorieIngr_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> listeCategorie = new List<string>();
            listeCategorie.Add("Salade");
            listeCategorie.Add("Viande");
            listeCategorie.Add("Légume");
            listeCategorie.Add("Fruit");
            var combo = sender as ComboBox;
            combo.ItemsSource = listeCategorie;
            combo.SelectedIndex = 0;
        }
        /// <summary>
        /// Permet de charger liste d'unité pour la création d'ingredient
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbUnite_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> Unite = new List<string>();
            Unite.Add("Kg");
            Unite.Add("g");
            Unite.Add("L");
            Unite.Add("mL");
            var combo = sender as ComboBox;
            combo.ItemsSource = Unite;
            combo.SelectedIndex = 0;
        }

        private void BtMenuIngredient_Click(object sender, RoutedEventArgs e)
        {
            Ajout_ingredient.Visibility = Visibility.Visible;
            Nouvelle_Recette.Visibility = Visibility.Hidden;
        }

        private void BtAnnulerCreerIngredient_Click(object sender, RoutedEventArgs e)
        {
            Creation_ingredient.Visibility = Visibility.Hidden;
            AjoutIngredient.Visibility = Visibility.Visible;
        }

        private void BackCreerRecetteCdr_Click(object sender, RoutedEventArgs e)
        {
            Nouvelle_Recette.Visibility = Visibility.Hidden;
            Espace_Cdr.Visibility = Visibility.Visible;
            BackCreerRecetteCdr.Visibility = Visibility.Hidden;
            BackNouvelleRecette.Visibility = Visibility.Visible;
        }

        private void Bt_Cdr_VisualiserRecette_Click(object sender, RoutedEventArgs e)
        {
            Visualiser_recette_cdr.Visibility = Visibility.Visible;
            Espace_Cdr.Visibility = Visibility.Hidden;
            dtvisualiserRecetteCdr.DataContext = databaseMain.ExecuteCommand("select recette.nom ,recette.type,recette.descriptif,recette.prix from recette join client on recette.createur = client.numero where client.numero = " + utilisateurActif.numero);
            // Rajouter ici visibilité pour 

        }

        private void Bt_Cdr_CreerRecette_Click(object sender, RoutedEventArgs e)
        {
            BackNouvelleRecette.Visibility = Visibility.Hidden;
            BackCreerRecetteCdr.Visibility = Visibility.Visible;
            Espace_Cdr.Visibility = Visibility.Hidden;
            Nouvelle_Recette.Visibility = Visibility.Visible;
        }
    }
}
