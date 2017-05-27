using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownGridBasedEngine;

namespace CompetitionV2.Menu
{
    class Menu : IPartieDeJeu
    {
        private Button[] m_btns;
        public Menu(Button[] Buttons)
        {
            m_btns = Buttons;

        }

        

        public void Update(GameTime gameTime)
        {
           //todo loop every buttons in form

            foreach (Button btn in m_btns)
            {
                btn.Update(gameTime);
            }
        }

        public void DrawWithShadows(SpriteBatch sb, GameTime gameTime, GraphicsDevice gd)
        {
            //Game1.Penumbra.Visible = false;
            Game1.Penumbra.BeginDraw();
            sb.Begin();
            sb.End();
            //DrawWithoutShadows(sb,gameTime,gd);
        }
        public void DrawWithoutShadows(SpriteBatch sb, GameTime gameTime, GraphicsDevice gd)
        {
            //Game1.Penumbra.Visible = false;
            //Game1.Penumbra.BeginDraw();
            sb.Begin();
            sb.Draw(TextureManager.BackgroundImage,gd.Viewport.Bounds,Color.White);
            foreach (Button btn in m_btns)
            {
                btn.Draw(sb);
            }
            sb.End();
        }


    }
}
