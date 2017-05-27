using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownGridBasedEngine
{
    public class OPGrenadierRobot : Enemy
    {
        public OPGrenadierRobot(int x, int y, Map m, bool registered) : base(x, y, m, registered, 0.18f) // Speedfactor changed there
        {
            _Hp = 38;
            // _Weapon = Molotov;
        }

    }
}
