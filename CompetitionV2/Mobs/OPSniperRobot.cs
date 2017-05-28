using Competition.Armes;
using CompetitionV2.Armes;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownGridBasedEngine
{
    public class OPSniperRobot : Enemy
    {
        public OPSniperRobot(int x, int y, Map m) : base(x, y, m, 0.3f) // Speedfactor changed there
        {
            _Hp = 26;
            DistanceFromPlayer = 20;
            Arme = new BoltActionSniperAI(this);
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
