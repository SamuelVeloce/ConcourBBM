using System;
using System.Threading;
using Competition.Armes;
using TopDownGridBasedEngine.Armes;
using TopDownGridBasedEngine.Projectile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Penumbra;
using EventHandler = CompetitionV2.Menu.EventHandler;


namespace TopDownGridBasedEngine
{
    public class Joueur : AbsMoveableEntity, ITexturable
    {
        private int _textureVariant;

        Weapons[] m_WeaponList;

        

        public event OnDropBombHandler DroppedBomb;
        public event OnBombExplodeHandler BombExploded;
        public event OnGenericBlockEventHandler BombPlacedBonus;
        public event OnGenericMultiblockEventHandler BombBrokeBlocks;

        protected void FireBombBrokeBlocks(object sender, MultiCaseEventArgs e)
        {
            BombBrokeBlocks?.Invoke(sender, e);
        }
        protected void FireBombPlacedBonus(object sender, CaseEventArgs e)
        {
            BombPlacedBonus?.Invoke(sender, e);
        }
        public void FireDroppedBomb(object sender, CaseEventArgs e)
        {
            DroppedBomb?.Invoke(sender, e);
        }

        public Joueur(int x, int y, Map m) : base(x, y, m)
        {
            Size = 25;
            _textureVariant = 0;

            BombsLeft = 1;

            m_WeaponList = new Weapons[] {new Pistol(this), new MachineGun(this)};


            Lights = new Light[2];

            Lights[0] = new PointLight();
            Lights[0].Color = Color.DarkGray;
            Lights[0].Intensity = 1f;
            Lights[0].Scale = new Vector2(900, 900);
            Lights[0].CastsShadows = true;
            Lights[0].ShadowType = ShadowType.Occluded;
            Lights[0].Radius = 5;
            Game1.Penumbra.Lights.Add(Lights[0]);

            Lights[1] = new PointLight();
            Lights[1].Color = Color.DarkGray;
            Lights[1].Intensity = 1f;
            Lights[1].Scale = new Vector2(900, 900);
            Lights[1].CastsShadows = false;
            Lights[1].ShadowType = ShadowType.Solid;
            Lights[1].Radius = 5;
            Game1.Penumbra.Lights.Add(Lights[1]);

            ChangedCase += Joueur_ChangedCase;
            Moved += Joueur_Moved;
            Died += Die;
        }

        public void DealDamage(int Damage)
        {

        }
        
        public Light[] Lights { get; }

        public void Die(object sender, CancellableEventArgs e)
        {
            IsDead = true;
            Game1.Penumbra.Lights.Remove(Lights[0]);
            Game1.Penumbra.Lights.Remove(Lights[1]); 
            
            Game1.SetPartieDeJeu((int)TypesDePartieDeJeu.MenuDefaut);
    }

        private void Joueur_Moved(object sender, CancellableEventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                Lights[i].Position = new Vector2(X * Map.TileWidth / Map.EntityPixelPerCase + Size / 2,
                    Y * Map.TileWidth / Map.EntityPixelPerCase + Size / 2);
            }

        }

        private void Joueur_ChangedCase(object sender, MultiCaseEventArgs e)
        {
            foreach (AbsCase c in e.Cases)
            {
                if (c.IsBreaking)
                {
                    FireDied(this, new CancellableEventArgs(false));
                }
            }
        }

        public Bomb Bomb { get; set; }

        public int BombsLeft { get; set; }

        public override EntityType Type { get; } = EntityType.Joueur;

        public Weapons[] Weapon
        {
            get { return m_WeaponList; }
            set { m_WeaponList = value; }
        }

        public Weapons CurrentWeapon()
        {
            return m_WeaponList[Math.Abs(Mouse.GetState().ScrollWheelValue/120)%m_WeaponList.Length];

        }


        public override void Tick(long deltaTime)
        {
            base.Tick(deltaTime);
            VelX *= (float)Math.Pow(0.9f, deltaTime / 8);
            VelY *= (float)Math.Pow(0.9f, deltaTime / 8);
            //UpdateTexture(DeltaTime);
        }

        public void UpdateTexture(long deltaTime)
        {
            _textureVariant += (int)deltaTime / 5;
            if (_textureVariant > 79)
                _textureVariant %= 80;
        }

        public void TickPlayer(long deltaTime, KeyWrapper wrapper)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                VelY -= (1.1f + VelY) / 15;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                VelY += (1.1f - VelY) / 15;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                VelX -= (1.1f + VelX) / 15;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                VelX += (1.1f - VelX) / 15;
            if ((wrapper.State & KeyState.Space) > 0)
            {
                DropBomb(X / 30, Y / 30);
            }
            //Lights[0].Rotation = (float)Math.Atan2(Mouse.GetState().Position.Y - Y * Map.TileWidth / Map.EntityPixelPerCase,
            //    Mouse.GetState().Position.X - X * Map.TileWidth / Map.EntityPixelPerCase);
        }

        public bool DropBomb(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Map.NoCase || y >= Map.NoCase)
                return false;
            CaseVide cv;
            if (!(Map[x, y] is CaseVide))
                Map[x, y] = new CaseVide(x, y, Map);
            cv = (CaseVide)Map[x, y];
            cv.Bomb = new Bomb(x * 30 + 15, y * 30 + 15, Map, this, 1500, 3);
            if (cv.IsBreaking)
                cv.Bomb.LifeTime = 150;
            BombsLeft--;
            EntityManager.Instance.Add(cv.Bomb);
            FireDroppedBomb(this, new CaseEventArgs(Map[x, y], false));
            cv.Bomb.Explode += Bomb_Exploded;
            cv.Bomb.PlaceBonus += FireBombPlacedBonus;
            cv.Bomb.BreakBlocks += FireBombBrokeBlocks;
            return true;
        }

        private void Bomb_Exploded(object sender, CaseEventArgs e)
        {
            BombExploded?.Invoke(sender, e);
        }

        public override void Draw(SpriteBatch sb, float w)
        {
            Draw(sb, w, Color.White);
        }

        public override void Draw(SpriteBatch sb, float w, Color color)
        {
            //b.Color = Color.Gold;
            Texture2D bit;
            if (Math.Abs(VelX) > Math.Abs(VelY)) // Left or right
            {
                if (VelX > 0)
                    bit = TextureManager.Instance.TexturePlayerRight[_textureVariant / 20];
                else
                    bit = TextureManager.Instance.TexturePlayerLeft[_textureVariant / 20];
            }
            else // Up or down
            {
                if (VelY > 0)
                    bit = TextureManager.Instance.TexturePlayerDown[_textureVariant / 20];
                else
                    bit = TextureManager.Instance.TexturePlayerUp[_textureVariant / 20];
            }
            if (VelX == 0 && VelY == 0)
                bit = TextureManager.Instance.TexturePlayerDown[0];
            //g.FillRectangle(b, (X - m_Radius) * w, (Y - m_Radius) * w, m_Radius * 2 * w, m_Radius * 2 * w);
            //g.DrawImage(bit, (X - rad) * w, (Y - rad - 20) * w, rad * w * 2, rad * w * 3);
            sb.Draw(bit, new Rectangle((int)(X * w), (int)(Y * w - Size * w), (int)(Size * w), (int)(Size * w * 2)), color);
            //Console.WriteLine($"{X}, {Y}\r\n{VelX}, {VelY}");
            //g.DrawString(string.Format("{0}, {1}\r\n{2}, {3}", X, Y, VelX, VelY), new Font("Arial", 12), b, 10, 45 * (_idJoueur));
        }

    }
}
