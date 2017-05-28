using CompetitionV2.Armes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownGridBasedEngine
{
    class FighterRobot : Enemy
    {
        public FighterRobot(int x, int y, Map m) : base(x, y, m, 0.22f) // Speedfactor changed there
        {
            _Hp = 50;
            DistanceFromPlayer = 3;
            Arme = new ShotgunAI(this);

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
