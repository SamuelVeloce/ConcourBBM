using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TopDownGridBasedEngine;

namespace CompetitionV2.Menu
{
    class Armurerie : Menu
    {
        private static Button[] btn = new Button[]
            {
                new Button("truc 1!", TextureManager.TextureTerre[0], new Rectangle(10, 10, 300, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.Jeu),
                new Button("truc 2!", TextureManager.TextureTerre[0], new Rectangle(10, 160, 300, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.Armurerie),
                new Button("truc 3!", TextureManager.TextureTerre[0], new Rectangle(10, 310, 300, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.FermerJeu)
            };
        public Armurerie():base(btn)
        {

        }
    }
}
