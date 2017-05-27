using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownGridBasedEngine.Projectile
{
    class ProjectileBullet:absProjectile
    {
        public ProjectileBullet(Texture2D[] EntityTextures, Vector2 StartPosition, Vector2 StartSize, Vector2 StartVelocity, int pDamage) : base(EntityTextures,StartPosition,StartSize,StartVelocity,pDamage)
        {
            
        }
    }
}
