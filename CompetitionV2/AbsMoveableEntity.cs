using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace CompetitionV2
{

    [Flags]
    public enum CollisionSide
    {
        None = 0,
        Up = 1,
        Left = 2,
        Down = 4,
        Right = 8
    };

    public struct CollisionInfo
    {
        public CollisionSide Side;
        public AbsCase Case;

        public CollisionInfo(CollisionSide side, AbsCase Case)
        {
            this.Side = side;
            this.Case = Case;
        }
    }

    public abstract class AbsMoveableEntity : AbsEntity
    {
        public event OnChangeCaseHandler ChangedCase;
        public event OnMoveHandler Moved;
        public event OnCollideWithBlockHandler Collided;

        protected AbsMoveableEntity(int x, int y, Map m) : base(x, y, m)
        {
            VelX = 0;
            VelY = 0;
        }

        public void FireMoved(object sender, CancellableEventArgs e)
        {
            Moved?.Invoke(sender, e);
        }

        public void FireChangedCase(object sender, MultiCaseEventArgs e)
        {
            ChangedCase?.Invoke(sender, e);
        }
        public void FireCollided(object sender, BlockCollisionEventArgs e)
        {
            Collided?.Invoke(sender, e);
        }


        public float VelX { get; set; }

        public float VelY { get; set; }

        /// <summary>
        /// Vérifie les collisions avec les cases solides
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="velx"></param>
        /// <param name="vely"></param>
        /// <returns></returns>
        protected virtual List<CollisionInfo> CheckCollision(int x, int y, float velx, float vely)
        {

            List<CollisionInfo> ret = new List<CollisionInfo>(2);
            int i = 0, n = 0;
            
            // Top Left
            int xm = x + (int)velx;
            int ym = y + (int)vely;
            
            int oldx = x;
            int oldy = y;
            
            AbsCase[] cases = GetCasesIn(x + (int)velx, y + (int)vely);


            if (xm < 0)
                ret.Add(new CollisionInfo(CollisionSide.Left, null));
            else if (xm + Size > Map.Width * Map.EntityPixelPerCase)
                ret.Add(new CollisionInfo(CollisionSide.Right, null));
            if (ym < 0)
                ret.Add(new CollisionInfo(CollisionSide.Up, null));
            else if (ym + Size > Map.Height * Map.EntityPixelPerCase)
                ret.Add(new CollisionInfo(CollisionSide.Down, null));

            for (; i < cases.Length; i++)
            {
                if (cases[i].IsSolid)
                    n++;
                if (cases[i].IsBreaking)
                    FireDied(this, new CancellableEventArgs(false));
            }
            
            if (n == 0)
            {
                if (ret.Count > 0)
                    //i = 0;
                return ret;
            }
            
            cases = cases.Where((ca) => ca.IsSolid == true).OrderBy((a) => Math.Abs(xm - a.X * Map.EntityPixelPerCase) + Math.Abs(ym - a.Y * Map.EntityPixelPerCase)).ToArray();
            if (cases.Any((a) => a.X == this.X / Map.EntityPixelPerCase || a.Y == this.Y / Map.EntityPixelPerCase))
            {
                cases = cases.Where((ca) => (ca.X == this.X / Map.EntityPixelPerCase || ca.Y == this.Y / Map.EntityPixelPerCase)
                    && !(ca.X == this.X / Map.EntityPixelPerCase && ca.Y == this.Y / Map.EntityPixelPerCase)).ToArray();
                n = cases.Count();
            }


            for (i = 0; i < n; i++)
            {
                //xm = x + (int)velx;
                //ym = y + (int)vely;
                //float dxm, dym, dx, dy;

                Vector2 TopLeftEntity = new Vector2(x, y);
                Vector2 BottomRightEntity = new Vector2(x + Size, y + Size);

                Vector2 TopLeftCase = new Vector2(cases[i].X * Map.EntityPixelPerCase, cases[i].Y * Map.EntityPixelPerCase);
                Vector2 BottomLeftCase = new Vector2(cases[i].X * Map.EntityPixelPerCase + Map.EntityPixelPerCase, cases[i].Y * Map.EntityPixelPerCase + Map.EntityPixelPerCase);

                float X_on_Case = TopLeftEntity.X - TopLeftCase.X;
                float Y_on_Case = TopLeftEntity.Y - TopLeftCase.Y;
                if (X_on_Case <= -Size) // Too far to the left 
                {
                    if (X_on_Case + velx > -Size) // Moved in to the right 
                    {
                        if (TopLeftEntity.Y < BottomLeftCase.Y || BottomRightEntity.Y > TopLeftCase.Y) // Collision to the left of the block
                        {
                            ret.Add(new CollisionInfo(CollisionSide.Left, cases[i]));
                            velx = 0;
                        }
                    }
                }
                else if (X_on_Case >= Map.EntityPixelPerCase) // Too far to the right
                {
                    if (X_on_Case + velx < Map.EntityPixelPerCase) // Moved in to the left
                    {
                        if (TopLeftEntity.Y < BottomLeftCase.Y || BottomRightEntity.Y > TopLeftCase.Y) // Collision to the right of the block
                        {
                            ret.Add(new CollisionInfo(CollisionSide.Right, cases[i]));
                            velx = 0;
                        }
                    }
                }
                else if (Y_on_Case <= -Size) // Too high up
                {
                    if (Y_on_Case + vely > -Size) // Moved down
                    {
                        if (TopLeftEntity.X < BottomLeftCase.X || BottomRightEntity.X > TopLeftCase.X) // Collision on the top
                        {
                            ret.Add(new CollisionInfo(CollisionSide.Up, cases[i]));
                            vely = 0;
                        }
                    }
                }
                else if (Y_on_Case >= Map.EntityPixelPerCase) // Too low down
                {
                    if (Y_on_Case + vely < Map.EntityPixelPerCase) // Moved up
                    {
                        if (TopLeftEntity.X < BottomLeftCase.X || BottomRightEntity.X > TopLeftCase.X) // Collision on the bottom
                        {
                            ret.Add(new CollisionInfo(CollisionSide.Down, cases[i]));
                            vely = 0;
                        }
                    }
                }
            }
            return ret;


            /*
                // dxm / dym -> Position du coin haut gauche de l'entité
                // par rapport au coin haut gauche de la case
                // APRÈS le déplacement
                dxm = xm - cases[i].X * Map.EntityPixelPerCase;
                dym = ym - cases[i].Y * Map.EntityPixelPerCase;

                // dx / dy -> Position du coin haut gauche de l'entité
                // par rapport au coin haut gauche de la case
                dx = x - cases[i].X * Map.EntityPixelPerCase;
                dy = y - cases[i].Y * Map.EntityPixelPerCase;

                
                if (dx <= -Size)
                {
                    // Évite la case par la gauche. Seule collision possible = gauche
                    if (-dxm <= Size && dym > -Size && dym < Map.EntityPixelPerCase)
                    {
                        ret.Add(new CollisionInfo(CollisionSide.Left, cases[i]));
                        xm = x - Size;
                        velx = 0;
                        dxm = xm - cases[i].X * Map.EntityPixelPerCase;
                    }
                }
                else if (dx >= Size)
                {
                    if (dxm <= Map.EntityPixelPerCase && dym > -Size && dym < Map.EntityPixelPerCase)
                    {
                        ret.Add(new CollisionInfo(CollisionSide.Right, cases[i]));
                        xm = x - Size;
                        velx = 0;
                        dxm = xm - cases[i].X * Map.EntityPixelPerCase;
                    }
                }
                if (dy <= -Size)
                {
                    if (-dym <= Size && dxm > -Size && dxm < Map.EntityPixelPerCase)
                    {
                        ret.Add(new CollisionInfo(CollisionSide.Up, cases[i]));
                        ym = y - Size;
                        vely = 0;
                        dym = ym - cases[i].Y * Map.EntityPixelPerCase;
                    }
                }
                else if (dy >= Size)
                {
                    if (dym <= Map.EntityPixelPerCase && dxm > -Size && dxm < Map.EntityPixelPerCase)
                    {
                        ret.Add(new CollisionInfo(CollisionSide.Down, cases[i]));
                        ym = y - Size;
                        vely = 0;
                        dym = ym - cases[i].Y * Map.EntityPixelPerCase;
                    }
                }
            }

            return ret;*/
        }

        /// <summary>
        /// Calcule le mouvement de l'entité
        /// </summary>
        /// <param name="DeltaTime"></param>
        public override void Tick(long deltaTime)
        {
            Enemy ent = this as Enemy;
            if (ent != null)
            {
                if (ent._path == null)
                {
                    int i = 0;
                }
            }

            float dt = deltaTime / 2;
            //VelY += 0.2f;
            int vx = (int)(VelX * dt);
            int vy = (int)(VelY * dt);
            
            List<CollisionInfo> res = CheckCollision(X, Y, vx, vy);
            CollisionSide rs = 0;
            
            foreach (CollisionInfo c in res)
            {
                rs |= c.Side;
                if (c.Case != null && c.Case.IsBreaking)
                    FireDied(this, new CancellableEventArgs(false));
            }
            
            bool Or = false;
            if (Math.Abs(VelX) < 0.01f)
            {
                VelX = 0;
                Or = true;
            }
            if (Math.Abs(VelY) < 0.01f)
            {
                VelY = 0;
                Or = true;
            }
            if (Or)
                FireMoved(this, new CancellableEventArgs(false));

            if (vx == 0 && vy == 0)
                return; 

            
            if (rs != CollisionSide.None)
            {
                BlockCollisionEventArgs e = new BlockCollisionEventArgs(X / Map.EntityPixelPerCase, Y / Map.EntityPixelPerCase, res, false);
                FireCollided(this, e);
                if ((rs & (CollisionSide.Left | CollisionSide.Right)) > 0)
                {
                    //_velx = 0;
                    vx = 0;
                }
                if ((rs & (CollisionSide.Up | CollisionSide.Down)) > 0)
                {
                    //_vely = 0;
                    vy = 0;
                }
            }

            
            if (X / Map.EntityPixelPerCase != (X + vx) / Map.EntityPixelPerCase) // Side only
            {
                FireChangedCase(this, new MultiCaseEventArgs(Map[X / Map.EntityPixelPerCase, Y / Map.EntityPixelPerCase], new AbsCase[] {
                    Map[(X + vx) / Map.EntityPixelPerCase, (Y + vy) / Map.EntityPixelPerCase]
                }, false));
                if (Y / Map.EntityPixelPerCase != (Y + vy) / Map.EntityPixelPerCase) // Both side and up/down
                {
                    FireChangedCase(this, new MultiCaseEventArgs(Map[X / Map.EntityPixelPerCase, Y / Map.EntityPixelPerCase], new AbsCase[] {
                        Map[(X + vx) / Map.EntityPixelPerCase, (Y + vy) / Map.EntityPixelPerCase],
                        Map[(X + vx) / Map.EntityPixelPerCase, Y / Map.EntityPixelPerCase],
                        Map[X / Map.EntityPixelPerCase, (Y + vy) / Map.EntityPixelPerCase]
                    }, false));
                }
            }
            else if (Y / Map.EntityPixelPerCase != (Y + vy) / Map.EntityPixelPerCase) // Up/Down only
            {
                FireChangedCase(this, new MultiCaseEventArgs(Map[X / Map.EntityPixelPerCase, Y / Map.EntityPixelPerCase], new AbsCase[] {
                    Map[(X + vx) / Map.EntityPixelPerCase, (Y + vy) / Map.EntityPixelPerCase]
                }, false));
            }
            X += vx;
            Y += vy;
            FireMoved(this, new CancellableEventArgs(false));
        }
    }
}
