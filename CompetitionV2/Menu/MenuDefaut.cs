using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CompetitionV2.Menu
{
    class MenuDefaut:Menu
    {
        private static Button[] btn = new Button[]
            {
                new ButtonInfo("The TTT Squad, the game!", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/2+Game1.Screen.ClientBounds.X/2-300, 50, 600, 100),
                    Game1.SetPartieDeJeu, 99), 
                new Button("Jouer!", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/2+Game1.Screen.ClientBounds.X/2-150, 250, 300, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.Jeu),
                new Button("Armurerie!", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/2+Game1.Screen.ClientBounds.X/2-150, 400, 300, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.Armurerie),
                new Button("Quitter!", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/2+Game1.Screen.ClientBounds.X/2-150, 550, 300, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.FermerJeu)
            };
        public MenuDefaut():base(btn)
        {
            btn = new Button[]
            {
                new ButtonInfo("The TTT Squad, the game!", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/2+Game1.Screen.ClientBounds.X/2-300, 50, 600, 100),
                    Game1.SetPartieDeJeu, 99),
                new Button("Jouer!", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/2+Game1.Screen.ClientBounds.X/2-150, 250, 300, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.Jeu),
                new Button("Armurerie!", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/2+Game1.Screen.ClientBounds.X/2-150, 400, 300, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.Armurerie),
                new Button("Quitter!", TextureManager.TextureTerre[0], new Rectangle(Game1.Screen.ClientBounds.Width/2+Game1.Screen.ClientBounds.X/2-150, 550, 300, 100),
                    Game1.SetPartieDeJeu, (int) TypesDePartieDeJeu.FermerJeu)
            };
        }
    }
}
