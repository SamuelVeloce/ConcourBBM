using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Competition.Armes;
using TopDownGridBasedEngine.Projectile;
using CompetitionV2.Armes;

using Penumbra;
using CompetitionV2;

namespace TopDownGridBasedEngine
{
    public class Enemy : AbsMoveableEntity, ITexturable
    {
        protected int _textureVariant;
        public Path _path;
        protected double NextPathfindTime;
        protected float _SpeedFactor;
        protected int DistanceFromPlayer;
        protected Color Couleur;
        public int _Hp { get; set; }
        

        public override EntityType Type => EntityType.GenericEntity;

        public Enemy(int x, int y, Map m, float speedFactor) : base(x, y, m)
        {
            VelX = 0;
            VelY = 0;
            Size = 25;
            _path = null;
            Died += Die;
            Collided += Enemy_Collided;
            NextPathfindTime = 0;
            _SpeedFactor = speedFactor;
            DistanceFromPlayer = 7;
            Arme = new PistolAI(this);
        }

        private void Enemy_Collided(object sender, BlockCollisionEventArgs e)
        {
            Vector2 vec = new Vector2(VelX + ((this.X + Size / 2) / Map.EntityPixelPerCase) * Map.EntityPixelPerCase + Map.EntityPixelPerCase / 2 - (this.X + this.Size / 2),
                VelY + ((this.Y + Size / 2) / Map.EntityPixelPerCase) * Map.EntityPixelPerCase + Map.EntityPixelPerCase / 2 - (this.Y + this.Size / 2));
            vec.Normalize();
            //vec *= _SpeedFactor;
            this.VelX = vec.X;
            this.VelY = vec.Y;
            
            //this.X = ((this.X + this.Size / 2) / Map.EntityPixelPerCase) * Map.EntityPixelPerCase + Map.EntityPixelPerCase / 2;
            //this.Y = ((this.Y + this.Size / 2) / Map.EntityPixelPerCase) * Map.EntityPixelPerCase + Map.EntityPixelPerCase / 2;
        }

        public Weapons Arme { get; set; }

        public void Die(object sender, CancellableEventArgs e)
        {
            // Make a random bonus

            Random r = new Random();

            if (r.Next() % 6 == 0)
            {
                int nbTypeBonus;
                int i = 0;
                while(i < ProgressManager.ArmesDebloque.Length && ProgressManager.ArmesDebloque[i])
                {
                    i++;
                }
                if(i == ProgressManager.ArmesDebloque.Length)
                {
                    nbTypeBonus = 3;
                }
                else
                {
                    nbTypeBonus = 4;
                }

                Bonus b = new Bonus(this.X, this.Y, this.Map, (BonusType)r.Next(nbTypeBonus));
            }

            //Make a sound
            SoundManager.ImpactRobot.Play();
        }

        public override void Tick(long deltaTime)
        {
            NextPathfindTime -= deltaTime;
            Joueur j = EntityManager.Instance.Joueur;

            if (_path != null && NextPathfindTime > 0)
            {
                
                int x = (this.X + Size / 2) / Map.EntityPixelPerCase;
                int y = (this.Y + Size / 2) / Map.EntityPixelPerCase;
                Point? p = _path.NextTile(new Point(x, y));
                if (p.HasValue)
                {
                    Vector2 Velocity = new Vector2(p.Value.X * Map.EntityPixelPerCase + Map.EntityPixelPerCase / 2 - (this.X + this.Size / 2),
                        p.Value.Y * Map.EntityPixelPerCase + Map.EntityPixelPerCase / 2 - (this.Y + this.Size / 2));
                    Velocity.Normalize();
                    Velocity *= _SpeedFactor;
                    VelX = Velocity.X;
                    VelY = Velocity.Y;
                }
                else
                {
                    VelX = VelY = 0;
                }
            }
            else if (NextPathfindTime <= 0)
            {
                NextPathfindTime = 200;
                if (RaycastTo(new Point(j.X + j.Size / 2, j.Y + j.Size / 2)))
                {
                    Arme.MouseDown(new Point(j.X - j.Size / 2, j.Y - j.Size / 2));
                    Arme.MouseUp();

                    
                }
                _path?.Delete();
                _path = new Path(new Point((this.X + this.Size / 2) / Map.EntityPixelPerCase, (this.Y + this.Size / 2) / Map.EntityPixelPerCase),
                    new Point(EntityManager.Instance.Joueur.X / Map.EntityPixelPerCase, EntityManager.Instance.Joueur.Y / Map.EntityPixelPerCase),
                    Map, DistanceFromPlayer);

                
            }

            base.Tick(deltaTime);
        }

        public override void Draw(SpriteBatch sb, float width)
        {
            Draw(sb, width, Couleur);
        }

        public virtual Texture2D Texture
        {
            get
            {
                if (Math.Abs(VelX) > Math.Abs(VelY)) // Left or right
                {
                    if (VelX > 0)
                        return TextureManager.Instance.TextureDroneRight[_textureVariant / 20];
                    else
                        return TextureManager.Instance.TextureDroneLeft[_textureVariant / 20];
                }
                else // Up or down
                {
                    if (VelY > 0)
                        return TextureManager.Instance.TextureDroneDown[_textureVariant / 20];
                    else
                        return TextureManager.Instance.TextureDroneUp[_textureVariant / 20];
                }
            }
        }

        public override void Draw(SpriteBatch sb, float w, Color color)
        {
            //sb.Draw(TextureManager.TextureTerre[0], new Rectangle((int)(X * w), (int)(Y * w), (int)(Size * w), (int)(Size * w)), color);
            sb.Draw(Texture, new Rectangle((int)(X * w), (int)(Y * w), (int)(Size * w), (int)(Size * w)), color);

        }

        public virtual void UpdateTexture(long deltaTime)
        {
            //_textureVariant += (int)deltaTime / 1;
            if (_textureVariant > 79)
                _textureVariant %= 80;

        }
    }
}
