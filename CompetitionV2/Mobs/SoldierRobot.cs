using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownGridBasedEngine
{
    public class SoldierRobot : Enemy
    {


        public SoldierRobot(int x, int y, Map m) : base(x, y, m, 0.24f) // Speedfactor changed there
        {
            _Hp = 18;
           // _Weapon = HandGunFlyWeigth;
        }
    }
}
