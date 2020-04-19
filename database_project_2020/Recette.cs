using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database_project_2020
{
    public class Recette
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string TypeR { get; set; }
        public string Descriptif { get; set; }
        public int Prix { get; set; }
        public int Remuneration { get; set; }
        public int Quantite { get; set; }
        public int Total { get; set; }
        

    }
}
