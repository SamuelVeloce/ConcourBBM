using Microsoft.Xna.Framework;
using System;
using System.Timers;

namespace CompetitionV2.Armes
{
    sealed class SelfService : Weapons
    {
        public override int NBulletLeft { get; set; }
        public override int NBulletInCharger { get; set; }

        // public override string WeaponName { get { return "Pistolet"; } }
        private const int m_BulletSpeed = 0;
        private const int m_ReloadingTime = 0;
        private const int m_ClipSize = 0;
        private const int m_Firerate = 0;
        private const int m_SpreadAngle = 0;
        private readonly Random m_RNG = new Random();
        private bool m_CanShoot = true;
        private bool m_Reloading;
        private readonly System.Timers.Timer m_WeaponTimer;
        private Vector2 Target;

        public bool STOP;

        public event EventHandler BOOM;


        public override int ClipSize
        {
            get { return m_ClipSize; }
        }
        public override void JouerSonTir()
        {
            SoundManager.ExplosionDebris.Play((float)1, 0, 0);
        }

        public override WeaponType WeaponType => WeaponType.Pistol;


        public override void Reload()
        {
            /*if (!m_Reloading && NBulletInCharger < m_ClipSize)
            {
                NBulletLeft += NBulletInCharger;
                NBulletInCharger = 0;
                m_Reloading = true;
                m_CanShoot = false;
                m_WeaponTimer.Interval = m_ReloadingTime;
                m_WeaponTimer.Start();
            }*/
            NBulletLeft = 1;
            NBulletInCharger = 1;
            m_Reloading = false;
            m_CanShoot = true;
            
        }




        public SelfService(AbsEntity Owner) : base(Owner)
        {
            NBulletLeft = 1;
            NBulletInCharger = 1;
            m_WeaponTimer = new System.Timers.Timer(600) { AutoReset = false };
            m_WeaponTimer.Elapsed += _timer_Elapsed;
            Nom = "SelfService";
            STOP = false;

        }

        public override void MouseDown()
        {
            //MouseDown((Mouse.GetState().Position.ToVector2() / EntityManager.Instance.Map.TileWidth * Map.EntityPixelPerCase).ToPoint());
        }

        public override void MouseDown(Point Target)//Vector2 MouseDir)
        {
            Vector2 v = new Vector2((Target.X - Owner.X) / Map.EntityPixelPerCase, (Target.Y - Owner.Y) / Map.EntityPixelPerCase);
            if (v.LengthSquared() < 4)
            {
                m_CanShoot = true;
                m_WeaponTimer.Start();
                //m_MouseDir = MouseDir;
                NBulletInCharger--;
                this.Target = Target.ToVector2();

                //double Radians = Math.Atan2(Target.Y - Owner.Y, Target.X - Owner.X) + ((m_RNG.NextDouble() * m_SpreadAngle) - m_SpreadAngle / 2.0) * (Math.PI / 180.0);
                //Vector2 MouseDir = new Vector2((float)Math.Cos(Radians), (float)Math.Sin(Radians));
                //ProjectileBullet bullet = new ProjectileBullet(TextureManager.TextureBullet, new Vector2(Owner.X, Owner.Y), new Vector2(8, 8), MouseDir * m_BulletSpeed, 10);
                //bullet.Friendly = true;
                //EntityManager.Instance.ProjectilesListFriendly.Add(bullet);

                //JouerSonTir();
            }
            else
            {
                //JouerSonVide();
            }
        }
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!STOP)
            {
                Vector2 v = new Vector2((Target.X - Owner.X) / Map.EntityPixelPerCase, (Target.Y - Owner.Y) / Map.EntityPixelPerCase);
                EntityManager.Instance.Joueur.DealDamage((int)(70 / v.Length()));
                BOOM?.Invoke(this, e);
            }

        }
        public override void MouseUp()
        {
            //int i = 9;  //debug
            //Do nothing in this case (somewhat useless)
        }
    }
}
