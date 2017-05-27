using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TopDownGridBasedEngine;

namespace CompetitionV2.Menu
{
    public delegate void EventHandler(int i);
    public class Button
    {
        private Rectangle m_ButtonRect;
        private string m_Nom;
        private Texture2D m_BackgroundTexture;
        public static event EventHandler m_Job;
        private int m_PartieDeJeu;

        public Button(string nom, Texture2D texture, Rectangle ButtonBox, EventHandler job, int PartieDeJeu)
        {
            Nom = nom;
            BackgroundTexture = texture;
            ButtonRect = ButtonBox;
            m_Job = job;
            m_PartieDeJeu = PartieDeJeu;
        }

        public Rectangle ButtonRect
        {
            get { return m_ButtonRect; }
            set { m_ButtonRect = value; }
        }

        public string Nom
        {
            get { return m_Nom; }
            set { m_Nom = value; }
        }

        public Texture2D BackgroundTexture
        {
            get { return m_BackgroundTexture; }
            set { m_BackgroundTexture = value; }
        }

        /**
         * @return true: If a player enters the button with mouse
         */
        public bool enterButton()
        {
            if (ButtonRect.Contains(Mouse.GetState().Position))
            {
                return true;
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            if (enterButton() && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                //todo do stuff

                m_Job.Invoke(m_PartieDeJeu);
            }
        }
        public void Draw(SpriteBatch sb)
        {
            if (enterButton())
            {
                sb.Draw(BackgroundTexture, ButtonRect, Color.Gray);
            }
            else
            {
                sb.Draw(BackgroundTexture, ButtonRect, Color.White);
            }
            Vector2 size = TextureManager.Font.MeasureString(Nom);
            
            sb.DrawString(TextureManager.Font, Nom, ButtonRect.Center.ToVector2(), Color.White, 0, size*0.5f, 2.0f, SpriteEffects.None, 0);
            
            
        }
    }
}
