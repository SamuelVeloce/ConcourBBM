using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace TopDownGridBasedEngine
{
    public class Grille
    {
        public Case BeginPoint;
        public Case EndPoint;
        Case[,] m_tCase;
        private Map map;

        public int Width, Height;

        public Grille(Map m, Point PositionDepart, Point PositionObjectif)
        {
            Width = m.NoCase;
            Height = Width; // Maybe have to change this to allow for non-square maps
            map = m;
            m_tCase = new Case[Width, Height];
            for (int I = 0; I < Height; I++)
                for (int J = 0; J < Width; J++)
                {
                    m_tCase[J, I] = new Case(m[J, I], this);
                }


            BeginPoint = this[PositionDepart.X, PositionDepart.Y];
            EndPoint = this[PositionObjectif.X, PositionObjectif.Y];
        }

        public Case this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < Width && y >= 0 && y < Height)
                {
                    return m_tCase[x, y];
                }
                return null;
            }
            set
            {
                if (x >= 0 && x < Width && y >= 0 && y < Height)
                {
                    m_tCase[x, y] = value;
                }
            }
        }

        public Case getBeginPoint()
        {
            return BeginPoint;
        }

        public Case getEndPoint()
        {
            return EndPoint;
        }


    }
}
