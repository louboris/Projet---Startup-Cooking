using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database_project_2020
{
    public class User
    {
        public string username = "";
        public string nom = "";
        public int numero = 0;
        public bool createur = false;
        public bool gestionnaire = false;
        public int soldeCdr = 0;
        public User(string username, string nom, int numero, bool createur = false, int solde = 0)
        {
            this.username = username;
            this.nom = nom;
            this.numero = numero;
            this.createur = createur;
            this.soldeCdr = solde;
        }

        public string Nom
        {
            get
            {
                return this.nom;
            }
            set
            {
                this.nom = value;
            }
        }

        public bool Createur
        {
            get
            {
                return this.createur;
            }
            set
            {
                this.createur = value;
            }
        }



    }
}
