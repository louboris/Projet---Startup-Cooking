﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database_project_2020
{
    public class Panier
    {
        public Dictionary<Recette, int> caddy = new Dictionary<Recette, int>();


        public void AddPanier(Recette rec, int quantite)
        {
            try
            {
                if (quantite == 0)
                {
                    caddy.Remove(rec);
                }
                else
                {
                    caddy[rec] = quantite + caddy[rec];
                }
            }
            catch
            {
                caddy[rec] = quantite;
            }

        }

        public List<Recette> PanierComplete()
        {
            List<Recette> items = new List<Recette>();
            foreach (KeyValuePair<Recette, int> entry in caddy)
            {
                // do something with entry.Value or entry.Key

                entry.Key.Quantite = entry.Value;
                entry.Key.Total = entry.Value * entry.Key.Prix;
                items.Add(entry.Key);
            }
            return items;
        }


    }
}
