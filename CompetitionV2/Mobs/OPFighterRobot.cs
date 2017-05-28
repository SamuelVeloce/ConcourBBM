using CompetitionV2.Armes;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TopDownGridBasedEngine
{
    public class OPFighterRobot : Enemy
    {
        public OPFighterRobot(int x, int y, Map m) : base(x, y, m, 0.45f) // Speedfactor changed there
        {
            _Hp = 80;
            Arme = new ShotgunAI(this);
            DistanceFromPlayer = 3;
            Couleur = Color.Orange;
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
