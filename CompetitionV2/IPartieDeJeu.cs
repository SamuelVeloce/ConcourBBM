using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TopDownGridBasedEngine
{
    public interface IPartieDeJeu
    {
        void DrawWithShadows(SpriteBatch sb, GameTime gameTime, GraphicsDevice gd);
        void DrawWithoutShadows(SpriteBatch sb, GameTime gameTime, GraphicsDevice gd);
        void Update(GameTime gameTime);
    }
}
