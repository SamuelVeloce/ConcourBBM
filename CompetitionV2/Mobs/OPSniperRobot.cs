using CompetitionV2.Armes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompetitionV2.Mobs
{
    public class OPSniperRobot : Enemy
    {
        public OPSniperRobot(int x, int y, Map m) : base(x, y, m, 0.3f) // Speedfactor changed there
        {
            _Hp = 26;
            DistanceFromPlayer = 20;
            Arme = new BoltActionSniperAI(this);
            Couleur = Color.BlueViolet;
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
