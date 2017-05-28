using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownGridBasedEngine;
using Competition.Armes;

namespace CompetitionV2
{
    public class Bonus : AbsEntity
    {

        public Bonus(int x, int y, Map m, WeaponType Type) : base(x, y, m)
        {
            this.Size = 10;
            EntityManager.Instance.Bonus.Add(this);
        }

        public Texture2D Texture => TextureManager.TextureBullet[0];

        public override EntityType Type => EntityType.Bonus;

        public override void Draw(SpriteBatch sb, float width)
        {
            Draw(sb, width, Color.White);
        }

        public override void Draw(SpriteBatch sb, float width, Color color)
        {
            sb.Draw(this.Texture, new Rectangle((int)(X * width / Map.EntityPixelPerCase), (int)(Y * width / Map.EntityPixelPerCase), (int)(Size * width), (int)(Size * width)), color);
        }

        public override void Tick(long deltaTime)
        {
            Joueur j = EntityManager.Instance.Joueur;
            if (j.X < this.X && j.X > this.X + this.Size &&
                j.Y < this.Y && j.Y > this.Y + this.Size)
            {
                int n = 1;
            }

        }

    }
}
