using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownGridBasedEngine;

namespace CompetitionV2.Projectile
{
    public abstract class absProjectile : Entity
    {
        private bool m_Friendly;
        private int m_Damage;

        public absProjectile(Texture2D[] EntityTextures, Vector2 StartPosition, Vector2 StartSize, Vector2 StartVelocity, int pDamage) : base(EntityTextures,StartPosition,StartSize,StartVelocity)
        {
            m_Damage = pDamage;
        }

        public bool Friendly { get; set; }

        public override bool Update(GameTime gametime)
        {
            Vector2 oldPosition = Position;
            base.Update(gametime);

            if (this.Position.X < 0 || Position.Y < 0 || Position.X > EntityManager.Instance.Map.Width * Map.EntityPixelPerCase || Position.Y > EntityManager.Instance.Map.Height * Map.EntityPixelPerCase)
            {
                return true;
            }


            int i;
            

            if (Friendly)
            {
                List<AbsEntity> enemies = EntityManager.Instance.Entities;
                i = enemies.Count - 1;

                if (i >= 0)
                {
                    while (i >= 0 &&
                           !(LinesCross(oldPosition, Position, new Vector2(enemies[i].X, enemies[i].Y - enemies[i].Size),
                                 new Vector2(enemies[i].X + enemies[i].Size, enemies[i].Y + enemies[i].Size)) ||
                             LinesCross(oldPosition, Position, new Vector2(enemies[i].X, enemies[i].Y + enemies[i].Size),
                                 new Vector2(enemies[i].X + enemies[i].Size, enemies[i].Y - enemies[i].Size))))
                    {
                        i--;
                    }
                    if (i >= 0)
                    {
                        EntityManager.Instance.Entities.RemoveAt(i);
                        return true;
                    }


                }

            }
            else // Hostile
            {
                Joueur j = EntityManager.Instance.Joueur;

                if (LinesCross(oldPosition, Position, new Vector2(j.X, j.Y - j.Size),
                                 new Vector2(j.X + j.Size, j.Y + j.Size)) ||
                             LinesCross(oldPosition, Position, new Vector2(j.X, j.Y + j.Size),
                                 new Vector2(j.X + j.Size, j.Y - j.Size)))
                {
                    j.DealDamage(m_Damage);
                    return true;
                }
            }

            List<AbsCase> Walls = EntityManager.Instance.Map.Walls;
            i = Walls.Count - 1;
            if (i >= 0)
            {
                while (i >= 0 &&
                       !(LinesCross(oldPosition, Position, new Vector2(Walls[i].Hitbox.X, Walls[i].Hitbox.Y),
                             new Vector2(Walls[i].Hitbox.X + Walls[i].Hitbox.Width, Walls[i].Hitbox.Y + Walls[i].Hitbox.Height)) ||
                         LinesCross(oldPosition, Position, new Vector2(Walls[i].Hitbox.X, Walls[i].Hitbox.Y + Walls[i].Hitbox.Height),
                             new Vector2(Walls[i].Hitbox.X + Walls[i].Hitbox.Width, Walls[i].Hitbox.Y))))
                {
                    i--;
                }
                if (i >= 0)
                {
                    return true;
                }


            }

            return false;

        }


        public override void Draw(SpriteBatch sb, float w)
        {
            Texture2D tempTexture = CurrentTexture();
            sb.Draw(tempTexture, Position / Map.EntityPixelPerCase * w, null, Color.Red, (float)Math.Atan2(Velocity.Y,Velocity.X)+(float)Math.PI/2, new Vector2(tempTexture.Width / 2.0f, tempTexture.Height / 2.0f), Size.X / tempTexture.Height , SpriteEffects.None, 0);
        }
    }
}
