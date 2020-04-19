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
            DataTable recette = database.ExecuteCommand("select * from recette");
            dtgTop5.DataContext = recette.DefaultView;
            tbCdrOr.Text = "Cdr OR : "; //RAJOUTER CDR OR ICI
        }

        private void BtMenu1_Click(object sender, RoutedEventArgs e)
        {
            MenuPrincipal.Visibility = Visibility.Hidden;
            TableauBordSemaine.Visibility = Visibility.Visible;

        }
    }
}
