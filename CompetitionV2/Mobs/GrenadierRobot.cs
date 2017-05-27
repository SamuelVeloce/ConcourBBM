using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownGridBasedEngine
{
    class GrenadierRobot : Enemy
    {

        public GrenadierRobot(int x, int y, Map m) : base(x, y, m, 0.20f) // Speedfactor changed there
        {
            _Hp = 34;
            // _Weapon = Grenade;
        }
    }
}
