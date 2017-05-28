using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Penumbra;

namespace TopDownGridBasedEngine
{
    
    public class Path
    {
        Case m_Begin;
        Case m_End;

        int m_Distance;

        List<Case> m_ListPath;

        List<Case> m_ListToCheck;

        Grille m_Grille;

        object Lock;

        public Path(Point Begin, Point End, Map m, int Distance)
        {
            m_Grille = new Grille(m, Begin, End);
            m_Begin = m_Grille.BeginPoint;
            m_End = m_Grille.EndPoint;
            m_ListPath = new List<Case>();
            m_ListToCheck = new List<Case>();
            m_Distance = Distance;
            Lock = new object();
            Trace();
            
        }

        public Point? NextTile(Point CurrentEntityPosition)
        {
            Case c = m_ListPath.LastOrDefault();
            if (c != null)
            {
                Vector2 Diff = new Vector2(CurrentEntityPosition.X - c.PosX, CurrentEntityPosition.Y - c.PosY);
                if (Diff.LengthSquared() < 0.25f)
                {
                    m_ListPath.Remove(c);
                }
                return new Point(c.PosX, c.PosY);
            }
            return null;
        }

        // Based on A* Algorithm
        public void Trace()
        {
            int Iteration = 0;
            m_ListToCheck.Add(m_Begin);
            List<Case> Neighbors = new List<Case>();
            Case c = m_Begin;

            c.CalculateCost(0, m_End);


            while (c != m_End && Iteration < 256 && m_ListToCheck.Any() && c != null)
            {
                // Find lowest F-Value in ListToCheck
                // Check all cases around and calculate their G-Value
                //   If their G-Value is lower when using this new route, change it and change the parent
                // Add them all in ListToCheck
                // Add current case to ListChecked
                // Repeat

                c = FindLowestFCost();
                if (c != null)
                {
                    Neighbors = c.getNeighbors();
                    foreach (Case cn in Neighbors)
                    {
                        cn.CalculateCost(c, m_End);
                        m_ListToCheck.Add(cn);
                    }
                    m_ListToCheck.Remove(c);
                    c.Checked = true;
                    Iteration++;
                }
            }
            if (c != null && m_End.ParentCase != null)
            {
                c = m_End.ParentCase;
                while (c != m_Begin)
                {
                    m_ListPath.Add(c);
                    c = c.ParentCase;
                }
            }
            if (m_ListPath.Count > m_Distance)
                m_ListPath.RemoveRange(0, m_Distance);
            else
                m_ListPath.Clear();

            //foreach (Case ca in m_ListPath)
            //    ca.Wrapped.color = Color.HotPink;
        }

        private Case FindLowestFCost()
        {
            m_ListToCheck.RemoveAll(cs => cs.Checked);
            if (m_ListToCheck.Count < 1)
            {
                return null;
            }
            else if (m_ListToCheck.Count == 1)
            {
                return m_ListToCheck[0];
            }
            int FCost = Int32.MaxValue;
            int I = 0;
            Case LowestCase = null;
            while (I < m_ListToCheck.Count)
            {
                if (m_ListToCheck[I].FCost < FCost)
                {
                    LowestCase = m_ListToCheck[I];
                    FCost = LowestCase.FCost;
                }
                I++;
            }
            return LowestCase;

        }

    }
    
}