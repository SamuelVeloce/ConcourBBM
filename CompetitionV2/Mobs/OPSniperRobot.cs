using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownGridBasedEngine
{
    public class OPSniperRobot : Enemy
    {
        public OPSniperRobot(int x, int y, Map m, bool registered) : base(x, y, m, registered, 0.24f) // Speedfactor changed there
        {
            _Hp = 26;
            // _Weapon = BoltActionSniper;
        }
    }
}
