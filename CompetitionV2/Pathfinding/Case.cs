using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CompetitionV2.Pathfinding
{
    public class Case
    {
        [Flags]
        private enum Direction
        {
            Down,
            Up,
            Left,
            Right,
        }
        public AbsCase Wrapped { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }
        public CaseType Type { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }

        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get; set; }
        public Case ParentCase { get; set; }

        Grille m_Grille;
        public bool Checked { get; set; }

        public Case(AbsCase Wrapped, Grille g)
        {
            PosX = Wrapped.X;
            PosY = Wrapped.Y;
            Type = Wrapped.Type;
            m_Grille = g;
            this.Wrapped = Wrapped;
            GCost = 0;
            HCost = 0;
            FCost = 0;
        }

        public Point getPos()
        {
            return new Point(PosX, PosY);
        }

        public List<Case> getNeighbors()
        {
            List<Case> Neighbors = new List<Case>();
            bool bLeft, bRight, bUp, bDown;

            bLeft = PosX > 0;
            bRight = PosX < m_Grille.Width - 1;
            bUp = PosY > 0;
            bDown = PosY < m_Grille.Width - 1;

            Case cLeft, cRight, cUp, cDown, c;

            if (bLeft)
            {
                cLeft = m_Grille[PosX - 1, PosY];
                if (!(cLeft.Checked || cLeft.Wrapped.IsSolid))
                    Neighbors.Add(cLeft);
            }

            if (bRight)
            {
                cRight = m_Grille[PosX + 1, PosY];
                if (!(cRight.Checked || cRight.Wrapped.IsSolid))
                    Neighbors.Add(cRight);
            }

            if (bDown)
            {
                cDown = m_Grille[PosX, PosY + 1];

                if (!(cDown.Checked || cDown.Wrapped.IsSolid))
                    Neighbors.Add(cDown);

                if (bRight)
                {
                    cRight = m_Grille[PosX + 1, PosY];
                    c = m_Grille[PosX + 1, PosY + 1];
                    if (!(c.Checked || c.Wrapped.IsSolid) && !cRight.Wrapped.IsSolid && !cDown.Wrapped.IsSolid)
                        Neighbors.Add(c);
                }

                if (bLeft)
                {
                    cLeft = m_Grille[PosX - 1, PosY];
                    c = m_Grille[PosX - 1, PosY + 1];
                    if (!(c.Checked || c.Wrapped.IsSolid) && !cLeft.Wrapped.IsSolid && !cDown.Wrapped.IsSolid)
                        Neighbors.Add(c);
                }

            }

            if (bUp)
            {
                cUp = m_Grille[PosX, PosY - 1];
                if (!(cUp.Checked || cUp.Wrapped.IsSolid))
                    Neighbors.Add(cUp);

                if (bLeft)
                {
                    cLeft = m_Grille[PosX - 1, PosY];
                    c = m_Grille[PosX - 1, PosY - 1];
                    if (!(c.Checked || c.Wrapped.IsSolid) && !cLeft.Wrapped.IsSolid && !cUp.Wrapped.IsSolid)
                        Neighbors.Add(c);
                }

                if (bRight)
                {
                    cRight = m_Grille[PosX + 1, PosY];
                    c = m_Grille[PosX + 1, PosY - 1];
                    if (!(c.Checked || c.Wrapped.IsSolid) && !cRight.Wrapped.IsSolid && !cUp.Wrapped.IsSolid)
                        Neighbors.Add(c);
                }
            }

            return Neighbors;
        }

        public void CalculateCost(Case Parent, Case End)
        {
            int G, H, F;
            G = Parent.GCost + 10;
            if (Math.Abs(Parent.PosY - this.PosY) + Math.Abs(Parent.PosX - this.PosX) > 1)
                G += 4;
            H = (Math.Abs(this.PosY - End.PosY) + Math.Abs(this.PosX - End.PosX)) * 10;
            F = G + H;

            if (ParentCase == null)
            {
                GCost = G;
                HCost = H;
                FCost = F;
                ParentCase = Parent;
            }
            else
            {
                if (F < FCost)
                {
                    ParentCase = Parent;
                    GCost = G;
                    HCost = H;
                    FCost = F;
                }

            }
        }

        public void CalculateCost(int G, Case End)
        {
            GCost = G;

            HCost = (Math.Abs(this.PosY - End.PosY) + Math.Abs(this.PosX - End.PosX)) * 10;

            FCost = GCost + HCost;
        }
    }
}
