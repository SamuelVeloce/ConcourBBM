using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompetitionV2.Mobs
{
    public class SoldierRobot : Enemy
    {


        public SoldierRobot(int x, int y, Map m) : base(x, y, m, 0.24f) // Speedfactor changed there
        {
            Couleur = Color.White;
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
