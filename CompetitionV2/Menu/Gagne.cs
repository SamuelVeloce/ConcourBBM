using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CompetitionV2.Menu
{
    class Gagne : Menu
    {
        private static Button[] btn = new Button[]
            {
                new ButtonInfo("Vous Avez Gagne! Felicitation!", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/2-250, 100, 500, 80),
                    Game1.SetPartieDeJeu, 99),
                new ButtonInfo("Argent gagne: " + ProgressManager.ArgentDernierePartie.ToString()+"$", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/2-250, 150, 500, 80),
                    Game1.SetPartieDeJeu, 99),
                new Button("Continuer", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/2-250, 300, 500, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.MenuDefaut),



            };
        public Gagne() : base(btn)
        {
            btn = new Button[]
            {
                new ButtonInfo("Vous Avez Gagne! Felicitation!", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/2-250, 100, 500, 80),
                    Game1.SetPartieDeJeu, 99),
                new ButtonInfo("Argent gagne: " + ProgressManager.ArgentDernierePartie.ToString()+"$", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/2-250, 150, 500, 80),
                    Game1.SetPartieDeJeu, 99),
                new Button("Continuer", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/2-250, 300, 500, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.MenuDefaut),



            };
        }
    }
}
