using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownGridBasedEngine
{
    public class OPGrenadierRobot : Enemy
    {
        public OPGrenadierRobot(int x, int y, Map m) : base(x, y, m, 0.18f) // Speedfactor changed there
        {
            _Hp = 38;
            // _Weapon = Molotov;
        }

    }
}
