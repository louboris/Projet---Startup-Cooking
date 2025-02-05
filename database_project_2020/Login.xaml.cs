﻿using System;
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
                if(databaseLogin.Connection(txUsername.Text, txPasseword.Password) == true) //Fait une demande auprès de la base de donnée pour savoir si la connexion est autorisée, mot de passe + username valide
                {
                    User utilisateur = databaseLogin.GetUser(txUsername.Text, txPasseword.Password); //initialise l'objet utilisateur qui correspond à l'utilisateur actif après connexion
                    if(utilisateur.username == "admin") // Si l'utilisateur est admin alors la page correspondante s'ouvre
                    {
                        GestionnaireCook gestionnaire = new GestionnaireCook();
                        gestionnaire.Show();
                        this.Close();
                    }
                    else {
                        MainWindow dashBoard = new MainWindow(utilisateur); //Ouverture de la page permettant les commandes "MainWindow"
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

        private void BtEvaluation_Click(object sender, RoutedEventArgs e)
        {
            Evaluation eval = new Evaluation();
            eval.Show();
          
        }
    }
}
