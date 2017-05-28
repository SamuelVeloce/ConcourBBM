using CompetitionV2.Armes;
using Microsoft.Xna.Framework;

namespace CompetitionV2.Mobs
{
    public class OPSoldierRobot : Enemy
    {

        public OPSoldierRobot(int x, int y, Map m) : base(x, y, m, 0.3f) // Speedfactor changed there
        {
            _Hp = 26;
            Arme = new AssaultRifleAI(this);
            Couleur = Color.Green;

        }
    }
}
