using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Penumbra;

namespace TopDownGridBasedEngine
{
    public class Hud
    {
        private GameWindow _window;
        private Joueur _joueur;

        public Hud(GameWindow win)
        {
            _window = win;
        }

        public void Draw(SpriteBatch sb)
        {
            //Dessine les informations dans le coin supérieur droit
            sb.DrawString(TextureManager.Font, EntityManager.Instance.Joueur.CurrentWeapon().Nom, new Vector2(Game1.Screen.ClientBounds.Width - 150, 10), Color.Yellow);
            sb.DrawString(TextureManager.Font, EntityManager.Instance.Entities.Count.ToString() + " Enemis restant", new Vector2(Game1.Screen.ClientBounds.Width - 150, 30), Color.Yellow);
            sb.DrawString(TextureManager.Font,"Balles: " + EntityManager.Instance.Joueur.CurrentWeapon().NBulletInCharger.ToString() , new Vector2(Game1.Screen.ClientBounds.Width - 150, 50),Color.Yellow);
        }
    }
}
