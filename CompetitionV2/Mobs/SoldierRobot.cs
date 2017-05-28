using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownGridBasedEngine
{
    public class SoldierRobot : Enemy
    {


        public SoldierRobot(int x, int y, Map m) : base(x, y, m, 0.24f) // Speedfactor changed there
        {
            
        }

        public override Texture2D Texture
        {
            get
            {
                return base.Texture;
            }
        }
    }
}
