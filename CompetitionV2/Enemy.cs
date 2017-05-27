﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Competition.Armes;
using TopDownGridBasedEngine.Projectile;

using Penumbra;

namespace TopDownGridBasedEngine
{
    public class Enemy : AbsMoveableEntity, ITexturable
    {
        protected int _textureVariant;
        protected Path _path;
        protected double NextPathfindTime;
        protected float _SpeedFactor;
        protected int _Hp { get; set; }
        

        public override EntityType Type => EntityType.GenericEntity;

        public Enemy(int x, int y, Map m, float speedFactor) : base(x, y, m)
        {
            VelX = 0;
            VelY = 0;
            Size = 15;
            _path = null;
            Died += Die;
            Collided += Enemy_Collided;
            NextPathfindTime = 0;
            _SpeedFactor = speedFactor;
            Arme = new Pistol(this);
        }

        private void Enemy_Collided(object sender, BlockCollisionEventArgs e)
        {
            /*Vector2 vec = new Vector2(VelX + (this.X / Map.EntityPixelPerCase) * Map.EntityPixelPerCase + Map.EntityPixelPerCase / 2 - (this.X + this.Size / 2),
                VelY + (this.Y / Map.EntityPixelPerCase) * Map.EntityPixelPerCase + Map.EntityPixelPerCase / 2 - (this.Y + this.Size / 2));
            vec.Normalize();
            vec *= _SpeedFactor;
            this.VelX = vec.X;
            this.VelY = vec.Y;*/

            this.X = ((this.X + this.Size / 2) / Map.EntityPixelPerCase) * Map.EntityPixelPerCase + Map.EntityPixelPerCase / 2;
            this.Y = ((this.Y + this.Size / 2) / Map.EntityPixelPerCase) * Map.EntityPixelPerCase + Map.EntityPixelPerCase / 2;
        }

        public Weapons Arme { get; set; }

        public void Die(object sender, CancellableEventArgs e)
        {
            
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
                    Arme.MouseDown(new Point(j.X, j.Y));
                    Arme.MouseUp();

                }
                _path = new Path(new Point((this.X + this.Size / 2) / Map.EntityPixelPerCase, (this.Y + this.Size / 2) / Map.EntityPixelPerCase),
                    new Point(EntityManager.Instance.Joueur.X / Map.EntityPixelPerCase, EntityManager.Instance.Joueur.Y / Map.EntityPixelPerCase),
                    Map);
                
            }
            base.Tick(deltaTime);
        }

        public override void Draw(SpriteBatch sb, float width)
        {
            Draw(sb, width, Color.White);
        }

        public override void Draw(SpriteBatch sb, float w, Color color)
        {
            Texture2D bit;
            if (Math.Abs(VelX) > Math.Abs(VelY)) // Left or right
            {
                if (VelX > 0)
                    bit = TextureManager.Instance.TexturePlayerRight[_textureVariant / 20];
                else
                    bit = TextureManager.Instance.TexturePlayerLeft[_textureVariant / 20];
            }
            else // Up or down
            {
                if (VelY > 0)
                    bit = TextureManager.Instance.TexturePlayerDown[_textureVariant / 20];
                else
                    bit = TextureManager.Instance.TexturePlayerUp[_textureVariant / 20];
            }
            if (VelX == 0 && VelY == 0)
                bit = TextureManager.Instance.TexturePlayerDown[0];
            sb.Draw(bit, new Rectangle((int)(X * w), (int)(Y * w - Size * w), (int)(Size * w), (int)(Size * w * 2)), color);
     }

        public void UpdateTexture(long deltaTime)
        {
            _textureVariant += (int)deltaTime / 5;
            if (_textureVariant > 79)
                _textureVariant %= 80;

        }
    }
}
