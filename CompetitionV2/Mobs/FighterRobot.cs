using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownGridBasedEngine
{
    class FighterRobot : Enemy
    {
        public FighterRobot(int x, int y, Map m) : base(x, y, m, 0.22f) // Speedfactor changed there
        {
            _Hp = 50;
            // _Weapon = RiotShield;
        }
    }
}
