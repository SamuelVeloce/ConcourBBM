using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace CompetitionV2
{
    public interface IPartieDeJeu
    {
        void Draw(SpriteBatch sb, GameTime gameTime);
        void Update(GameTime gameTime);
    }
}
