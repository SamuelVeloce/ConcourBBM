using CompetitionV2.Armes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompetitionV2.Mobs
{
    public class SniperRobot : Enemy
    {
        public SniperRobot(int x, int y, Map m) : base(x, y, m, 0.18f) // Speedfactor changed there
        {
            _Hp = 26;
            Arme = new SemiAutomaticSniperAI(this);
            DistanceFromPlayer = 20;
            Couleur = Color.Purple;
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
