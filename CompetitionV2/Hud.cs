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
            Point bottomLeft = new Point(0, _window.ClientBounds.Height);
            Point bottomRight = new Point(_window.ClientBounds.Width, _window.ClientBounds.Height);
            Point topLeft = new Point(_window.ClientBounds.Width, 0);
            Point topRight = new Point(0, 0);

            //sb.Draw(TextureManager.Instance.TextureFire[0], new Rectangle(bottomLeft.X, bottomLeft.Y - 10, 10, 10), Color.White);


        }
    }
}
