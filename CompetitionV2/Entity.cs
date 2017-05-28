using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompetitionV2
{
    public class Entity
    {
        private Vector2 m_Position;
        private Vector2 m_Velocity;
        private Vector2 m_Size;
        private bool m_Visible;
        private Texture2D[] m_EntityTextures;
        private double m_AnimationTimerStart;
        private double m_AnimationTimerDuration;
        private double m_AnimationTimerCurrentTime;
        private bool m_AnimationLoop;


        public Entity(Texture2D[] EntityTextures, Vector2 StartPosition, Vector2 StartSize, Vector2 StartVelocity, double AnimationTimerStart = 0.0,
            double AnimationTimerDuration = 1000.0/*millisecondes*/, bool AnimationLoop = true)
        {
            m_Size = StartSize;
            m_Position = StartPosition;
            m_AnimationTimerStart = AnimationTimerStart;
            m_EntityTextures = EntityTextures;
            m_AnimationTimerDuration = AnimationTimerDuration;
            m_AnimationTimerCurrentTime = AnimationTimerStart;
            m_AnimationLoop = AnimationLoop;
            m_Velocity = StartVelocity;
        }

        public Texture2D CurrentTexture()
        {
            if (m_AnimationLoop)
            {
                return
                    m_EntityTextures[
                        (int)
                        (m_EntityTextures.Length * (m_AnimationTimerCurrentTime -
                         m_AnimationTimerStart) / m_AnimationTimerDuration) % m_EntityTextures.Length];
            }
            else
            {
                return
                    m_EntityTextures[
                        Math.Min(
                            (int)
                            (m_EntityTextures.Length * (m_AnimationTimerCurrentTime -
                             m_AnimationTimerStart) / m_AnimationTimerDuration), m_EntityTextures.Length - 1)];
            }
        }

        public virtual bool Update(GameTime gametime)
        {
            m_AnimationTimerCurrentTime = gametime.TotalGameTime.TotalMilliseconds;
            m_Position = m_Position + m_Velocity * gametime.ElapsedGameTime.Milliseconds / 1000;
            return false;
        }

        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }

        public Texture2D[] EntityTextures
        {
            get { return m_EntityTextures; }
            set { m_EntityTextures = value; }
        }


        public bool AnimationLoop
        {
            get { return m_AnimationLoop; }
            set { m_AnimationLoop = value; }
        }

        public Vector2 Size
        {
            get { return m_Size; }
            set { m_Size = value; }
        }

        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        public double AnimationTimerStart
        {
            get { return m_AnimationTimerStart; }
            set { m_AnimationTimerStart = value; }
        }

        public double AnimationTimerDuration
        {
            get { return m_AnimationTimerDuration; }
            set { m_AnimationTimerDuration = value; }
        }

        public virtual void Draw(SpriteBatch sb, float w)
        {
            sb.Draw(CurrentTexture(), new Rectangle((int)m_Position.X, (int)m_Position.Y, (int)m_Size.X, (int)m_Size.Y), Color.DeepPink);
            
        }
        public bool LinesCross(Vector2 Debut1, Vector2 Fin1, Vector2 Debut2, Vector2 Fin2)
        {

            float Denominator = ((Fin1.X - Debut1.X) * (Fin2.Y - Debut2.Y)) -
                                ((Fin1.Y - Debut1.Y) * (Fin2.X - Debut2.X));
            float Numerator1 = ((Debut1.Y - Debut2.Y) * (Fin2.X - Debut2.X)) -
                               ((Debut1.X - Debut2.X) * (Fin2.Y - Debut2.Y));
            float Numerator2 = ((Debut1.Y - Debut2.Y) * (Fin1.X - Debut1.X)) -
                               ((Debut1.X - Debut2.X) * (Fin1.Y - Debut1.Y));


            if (Denominator == 0) return Numerator1 == 0 && Numerator2 == 0;

            float R = Numerator1 / Denominator;
            float S = Numerator2 / Denominator;

            return (R >= 0 && R <= 1) && (S >= 0 && S <= 1);
        }
    }
}
