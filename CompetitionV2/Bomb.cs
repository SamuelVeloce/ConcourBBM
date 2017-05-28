/// <summary>
        /// Appelée par l'EntityManager. Fait ce qui est nécessaire
        /// </summary>
        /// <param name="deltaTime"></param>
        /*public override void Tick(long deltaTime)
        {
            if (!Carried && !Flying)
            {
                base.Tick(deltaTime);
            }
            else if (Flying)
            {
                TickFlying(deltaTime);
            }
            if (Z == 0 && Flying == false)
            {
                // Décrémente sa durée de vie
                LifeTime -= (int)deltaTime;
                if (LifeTime < 0)
                    Update();
            }
        }

        /// <summary>
        /// Change la texture de la bombe
        /// </summary>
        /// <param name="deltaTime"></param>
        public void UpdateTexture(long deltaTime)
        {
            _textureVariant += (int)deltaTime / 5;
            if (_textureVariant > 79)
                _textureVariant %= 80;
        }

        /// <summary>
        /// Explosion! BOOM
        /// </summary>
        public void Update()
        {
            
            int i = 1, x, y, mx = X / Map.EntityPixelPerCase, my = Y / Map.EntityPixelPerCase;
            CaseEventArgs e = new CaseEventArgs(Map[mx, my], false);
            FireExplode(this, e);
            Game1.Penumbra.Lights.Remove(Light);
            if (e.Cancelled)
                return;
            
            Owner.BombsLeft++;
            bool bLeft = true, bRight = true, bUp = true, bDown = true;

            List<AbsCase> BrokenBlocks = new List<AbsCase>();
            while (i <= _power && (bRight || bUp || bLeft || bDown))
            {
                if (bRight)
                {
                    x = mx + i;
                    if (x < Map.NoCase && Map[x, my].IsBreakable)
                    {
                        if (!SetFire(x, my, BrokenBlocks))
                            bRight = false;
                    }
                    else
                        bRight = false;
                }
                if (bLeft)
                {
                    x = mx - i;
                    if (x >= 0 && Map[x, my].IsBreakable)
                    {
                        if (!SetFire(x, my, BrokenBlocks))
                            bLeft = false;
                    }
                    else
                        bLeft = false;
                }
                if (bUp)
                {
                    y = my + i;
                    if (y < Map.NoCase && Map[mx, y].IsBreakable)
                    {
                        if (!SetFire(mx, y, BrokenBlocks))
                            bUp = false;
                    }
                    else
                        bUp = false;
                }
                if (bDown)
                {
                    y = my - i;
                    if (y >= 0 && Map[mx, y].IsBreakable)
                    {
                        if (!SetFire(mx, y, BrokenBlocks))
                            bDown = false;
                    }
                    else
                        bDown = false;
                }
                i++;
            }

            SetFire(mx, my, BrokenBlocks);
            FireBreakBlocks(this, new MultiCaseEventArgs(Map[mx, my], BrokenBlocks.ToArray(), false));
            
            ((CaseVide)Map[mx, my]).Bomb = null;
            EntityManager.Instance.Remove(this);


        }

        /// <summary>
        /// Changes a block to fire
        /// </summary>
        /// <param name="x">X coord (in map coords) of the block</param>
        /// <param name="y">Y coord (in map coords) of the block</param>
        /// <returns>True if the block was changed and you can keep updating other blocks.
        /// False if you must stop updating blocks.</returns>
        private bool SetFire(int x, int y, List<AbsCase> BrokenBlocks)
        {
            if (x < 0 || y < 0 || x >= Map.NoCase || y >= Map.NoCase)
                return false;
            bool ret = false;
            AbsCase c = Map[x, y];
            if (!c.IsBreakable)
                return false;

            CaseVide cv = c as CaseVide;
            
            if (cv != null)
            {
                if (cv.ContainsBomb)
                    cv.Bomb.LifeTime = 150;
            }

            if (c is CaseWall)
            {
                Ignite(x, y, Map, 600, BrokenBlocks);
            }
            else
            {
                Ignite(x, y, Map, 600, BrokenBlocks);
                if (c.LetsFireThrough)
                {
                    ret = true;
                }
            }

            return ret;
        }

        /// <summary>
        /// Brise une case
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="m"></param>
        /// <param name="LifeTime"></param>
        /// <param name="BrokenBlocks"></param>
        /// <param name="Register"></param>
        /// <param name="EntityID"></param>
        public static void Ignite(int x, int y, Map m, int LifeTime, List<AbsCase> BrokenBlocks)
        {
            Fire f = new Fire(x * Map.EntityPixelPerCase + Map.EntityPixelPerCase / 2, y * Map.EntityPixelPerCase + Map.EntityPixelPerCase / 2, m, 600);
            m[x, y].Fire = f;
            BrokenBlocks?.Add(m[x, y]);
            EntityManager.Instance.Add(f);
        }








        // Collisions dans les airs
        #region Flying Collisions

        private void TickFlying(long DeltaTime)
        {
            float dt = DeltaTime / 2;
            int vx = (int)(VelX * dt);
            int vy = (int)(VelY * dt);
            List<CollisionInfo> CollisionInfoReturned = CheckFlyingCollision(X, Y, vx, vy);
            CollisionSide Side = 0;
            CollisionInfo Target = new CollisionInfo(CollisionSide.None, null);
            foreach (CollisionInfo c in CollisionInfoReturned)
            {
                Side |= c.Side;
                if (c.Case != null)
                {
                    if (c.Case.IsBreaking)
                    {
                        FireDied(this, new CancellableEventArgs(false));
                    }
                    Target = c;
                }
                else if (c.Case == null && c.Side != 0)
                {
                    switch (Side)
                    {
                        case CollisionSide.Up:
                            Y = Map.NoCase * Map.EntityPixelPerCase - Size;
                            break;
                        case CollisionSide.Down:
                            Y = Size;
                            break;
                        case CollisionSide.Left:
                            X = Map.NoCase * Map.EntityPixelPerCase - Size;
                            break;
                        case CollisionSide.Right:
                            X = Size;
                            break;
                    }
                }
            }

            if (Side == CollisionSide.None)
            {
                FireMoved(this, new CancellableEventArgs(false));
            }
            else
            {
                if (Target.Case != null && !Target.Case.IsSolid)
                {
                    BlockCollisionEventArgs e = new BlockCollisionEventArgs(Target.Case.X, Target.Case.Y, new List<CollisionInfo> { new CollisionInfo(Target.Side, Target.Case)}, false);
                    FireCollided(this, e);
                }
                if ((Side & (CollisionSide.Left | CollisionSide.Right)) > 0)
                {
                    //_velx = 0;
                    vx = 0;
                }
                if ((Side & (CollisionSide.Up | CollisionSide.Down)) > 0)
                {
                    //_vely = 0;
                    vy = 0;
                }
                if (vx != 0 || vy != 0)
                    FireMoved(this, new CancellableEventArgs(false));

            }

            // Détection de mouvement
            if (X / Map.EntityPixelPerCase != (X + vx) / Map.EntityPixelPerCase) // Side only
            {
                FireChangedCase(this, new MultiCaseEventArgs(Map[X / Map.EntityPixelPerCase, Y / Map.EntityPixelPerCase], new AbsCase[] {
                    Map[(X + vx) / Map.EntityPixelPerCase, (Y + vy) / Map.EntityPixelPerCase]
                }, false));
                if (Y / Map.EntityPixelPerCase != (Y + vy) / Map.EntityPixelPerCase) // Both side and up/down
                {
                    FireChangedCase(this, new MultiCaseEventArgs(Map[X / Map.EntityPixelPerCase, Y / Map.EntityPixelPerCase], new AbsCase[] {
                        Map[(X + vx) / Map.EntityPixelPerCase, (Y + vy) / Map.EntityPixelPerCase],
                        Map[(X + vx) / Map.EntityPixelPerCase, Y / Map.EntityPixelPerCase], Map[X / Map.EntityPixelPerCase, (Y + vy) / Map.EntityPixelPerCase]
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

            if (Math.Abs(VelX) < 0.01f)
                VelX = 0;
            if (Math.Abs(VelY) < 0.01f)
                VelY = 0;
        }

        private List<CollisionInfo> CheckFlyingCollision(int x, int y, float velx, float vely)
        {

            List<CollisionInfo> ret = new List<CollisionInfo>(2);
            int i = 0, n = 0;
            int xm = x + (int)velx - Size; // top left
            int ym = y + (int)vely - Size;
            int rad2 = Size + Size;
            int oldx = x - Size;
            int oldy = y - Size;
            AbsCase[] cases = GetCasesIn(x + (int)velx, y + (int)vely);


            if (xm < 0) // TODO - Faire que la bombe loop autour de la map
                ret.Add(new CollisionInfo(CollisionSide.Left, null));
            else if (xm + rad2 >= Map.NoCase * Map.EntityPixelPerCase)
                ret.Add(new CollisionInfo(CollisionSide.Right, null));
            if (ym < 0)
                ret.Add(new CollisionInfo(CollisionSide.Up, null));
            else if (ym + rad2 >= Map.NoCase * Map.EntityPixelPerCase)
                ret.Add(new CollisionInfo(CollisionSide.Down, null));

            for (; i < cases.Length; i++)
            {
                if (!cases[i].IsSolid) // Vérifier les cases pas solides
                    n++;
                //if (Cases[i] is CaseVide && Cases[i].IsBreaking) // Exploser la bombe
                //    Update();
            }
            if (n == 0)
            {
                if (ret.Count > 0)
                    i = 0;
                return ret;
            }
            cases = cases.Where((ca) => ca.IsSolid == false).OrderBy((a) => Math.Abs(xm - a.X * Map.EntityPixelPerCase) + Math.Abs(ym - a.Y * Map.EntityPixelPerCase)).ToArray();
            // Ignorer tout cela et trouver la case dans laquelle on tombe

            float dxm, dym, dx, dy;
            for (i = 0; i < n; i++)
            {
                dxm = xm - cases[i].X * Map.EntityPixelPerCase;
                dym = ym - cases[i].Y * Map.EntityPixelPerCase;
                dx = x - cases[i].X * Map.EntityPixelPerCase - Size;
                dy = y - cases[i].Y * Map.EntityPixelPerCase - Size;
                if (dx <= -rad2)
                {
                    if (-dxm <= rad2 - 1 && dym > -rad2 && dym < Map.EntityPixelPerCase)
                    {
                        ret.Add(new CollisionInfo(CollisionSide.Left, cases[i]));
                        xm = x - Size;
                    }
                }
                else if (dx >= Map.EntityPixelPerCase)
                {
                    if (dxm <= Map.EntityPixelPerCase && dym > -rad2 && dym < Map.EntityPixelPerCase)
                    {
                        ret.Add(new CollisionInfo(CollisionSide.Right, cases[i]));
                        xm = x - Size;
                    }
                }
                if (dy <= -rad2)
                {
                    if (-dym <= rad2 - 1 && dxm > -rad2 && dxm < Map.EntityPixelPerCase)
                    {
                        ret.Add(new CollisionInfo(CollisionSide.Up, cases[i]));
                        ym = y - Size;
                    }
                }
                else if (dy >= Map.EntityPixelPerCase)
                {
                    if (dym <= Map.EntityPixelPerCase && dxm > -rad2 && dxm < Map.EntityPixelPerCase)
                    {
                        ret.Add(new CollisionInfo(CollisionSide.Down, cases[i]));
                        ym = y - Size;
                    }
                }
            }
            // Retourner la case dans laquelle on tombe.
            return ret;
        }
        
        #endregion
        
    }
}*/
