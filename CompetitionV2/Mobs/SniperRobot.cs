using CompetitionV2.Armes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownGridBasedEngine
{
    public class SniperRobot : Enemy
    {
        public SniperRobot(int x, int y, Map m) : base(x, y, m, 0.18f) // Speedfactor changed there
        {
            _Hp = 26;
            Arme = new SemiAutomaticSniperAI(this);
            DistanceFromPlayer = 20;
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
