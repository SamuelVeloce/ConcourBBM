using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;

namespace TopDownGridBasedEngine
{
    class CaseVerte : AbsCase
    {
        //private Bomb _bomb;
        public CaseVerte(int x, int y, Map parent) : base(x, y, parent, false, true, true)
        {
            //_bomb = null;
        }

        //public bool ContainsBomb => _bomb != null;

        /*public Bomb Bomb
        {
            get
            {
                return _bomb; // null if there's no bomb
            }
            set
            {
                _bomb = value;
                LetsFireThrough = value == null;
            }
        }*/

        //public override bool IsSolid => _bomb != null;

        public override CaseType Type => CaseType.Vide;

        public override Texture2D Texture => TextureManager.Instance.TextureCaseVerte;
    }
}
