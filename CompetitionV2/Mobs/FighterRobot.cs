using CompetitionV2.Armes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompetitionV2.Mobs
{
    class FighterRobot : Enemy
    {
        public FighterRobot(int x, int y, Map m) : base(x, y, m, 0.22f) // Speedfactor changed there
        {
            _Hp = 50;
            DistanceFromPlayer = 3;
            Arme = new ShotgunAI(this);
            Couleur = Color.Red;
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
