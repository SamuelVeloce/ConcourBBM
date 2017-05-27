using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompetitionV2.Projectile
{
    abstract class absProjectile : Entity
    {
        
        private int m_Damage;

        public absProjectile(Texture2D[] EntityTextures, Vector2 StartPosition, Vector2 StartSize, Vector2 StartVelocity, int pDamage) : base(EntityTextures,StartPosition,StartSize,StartVelocity)
        {
            m_Damage = pDamage;
        }

        

        public override void Draw(SpriteBatch sb)
        {
            Texture2D tempTexture = CurrentTexture();
            sb.Draw(tempTexture, Position, null, Color.DeepPink, 0.5f, new Vector2(tempTexture.Width / 2.0f, tempTexture.Height / 2.0f), 1f, SpriteEffects.None, 0);
        }
    }
}
