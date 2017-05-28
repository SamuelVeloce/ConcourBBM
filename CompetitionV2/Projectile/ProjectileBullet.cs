using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompetitionV2.Projectile
{
    class ProjectileBullet:absProjectile
    {
        public ProjectileBullet(Texture2D[] EntityTextures, Vector2 StartPosition, Vector2 StartSize, Vector2 StartVelocity, int pDamage) : base(EntityTextures,StartPosition,StartSize,StartVelocity,pDamage)
        {
            
        }
    }
}
