using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TopDownGridBasedEngine;

namespace CompetitionV2.Menu
{
    class MenuDefaut:Menu
    {
        private static Button[] btn = new Button[]
            {
                new Button("Jouer!", TextureManager.TextureTerre[0], new Rectangle(10, 10, 300, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.Jeu),
                new Button("Armurerie!", TextureManager.TextureTerre[0], new Rectangle(10, 160, 300, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.Armurerie),
                new Button("Quitter!", TextureManager.TextureTerre[0], new Rectangle(10, 310, 300, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.FermerJeu)
            };
        public MenuDefaut():base(btn)
        {
            
        }
    }
}
