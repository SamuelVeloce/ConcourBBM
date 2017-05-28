using System;
using System.Timers;
using CompetitionV2.Projectile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace CompetitionV2.Armes
{
    sealed class AssaultRifleAI : Weapons
    {
        public override int NBulletLeft { get; set; }
        public override int NBulletInCharger { get; set; }

      //  public override string WeaponName { get { return "Mitrailleuse antique"; } }

        public override WeaponType WeaponType
        {
            get
            {
                return WeaponType.AssaultRifle;
            }
        }

        public override int ClipSize
        {
            get { return m_ClipSize; }
        }
        private const byte m_BulletSpeed = 200;
        private const int m_ReloadingTime = 2000;
        private const int m_ClipSize = 30;//30;
        private const int m_Firerate = 150;//105;
        private const int m_SpreadAngle = 6;
        private readonly Random m_RNG = new Random();
        private readonly object m_WeaponLock = new object();
        private bool m_CanShoot = true;
        private bool m_Reloading;
        private readonly System.Timers.Timer m_WeaponTimer;




        public override void JouerSonTir()
        {
            SoundManager.Pistol.Play((float)0.5, 0, 0);
        }


        public override void Reload()
        {
            if (!m_Reloading && NBulletInCharger < m_ClipSize)
            {
                NBulletLeft += NBulletInCharger;
                NBulletInCharger = 0;
                m_Reloading = true;
                m_CanShoot = false;
                m_WeaponTimer.Interval = m_ReloadingTime;
                m_WeaponTimer.Start();
            }
        }




        public AssaultRifleAI(AbsEntity Owner) : base(Owner)
        {

            Nom = "Fusil d'assaut";
            NBulletLeft = 50;//int.MaxValue - 100;
            NBulletInCharger = m_ClipSize;
            m_WeaponTimer = new System.Timers.Timer(m_Firerate) { AutoReset = false };
            m_WeaponTimer.Elapsed += _timer_Elapsed;


        }

        public override void MouseDown()
        {
            MouseDown(Mouse.GetState().Position);
        }

        public override void MouseDown(Point Target)
        {
            if (m_CanShoot)
            {
                m_CanShoot = false;
                NBulletInCharger--;
                lock (m_WeaponLock)
                {
                    m_WeaponTimer.Start();
                }


                double Radians = Math.Atan2(Target.Y - Owner.Y, Target.X - Owner.X) + ((m_RNG.NextDouble() * m_SpreadAngle) - m_SpreadAngle / 2.0) * (Math.PI / 180.0);
                Vector2 MouseDir = new Vector2((float)Math.Cos(Radians), (float)Math.Sin(Radians));
                EntityManager.Instance.ProjectilesListHostile.Add(new ProjectileBullet(TextureManager.TextureBullet, new Vector2(Owner.X, Owner.Y), new Vector2(8, 8), MouseDir * 500, 8) { Friendly = false });


                JouerSonTir();
            }
            else
            {
                JouerSonVide();
            }
        }
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (m_WeaponLock)
            {
                if (m_Reloading)
                {
                    if (NBulletLeft <= 0)
                    {
                        m_WeaponTimer.Stop();
                        m_WeaponTimer.Interval = m_ReloadingTime;
                        m_WeaponTimer.Start();
                    }
                    else
                    {
                        if (NBulletLeft <= m_ClipSize)
                        {
                            NBulletInCharger = NBulletLeft;
                            NBulletLeft = 0;
                        }
                        else
                        {
                            NBulletLeft -= m_ClipSize;
                            NBulletInCharger = m_ClipSize;
                        }
                        m_WeaponTimer.Stop();
                        m_WeaponTimer.Interval = m_Firerate;
                        m_Reloading = false;
                        m_CanShoot = true;
                    }

                }
                else
                {

                    if (NBulletInCharger <= 0)
                    {
                        m_CanShoot = false;
                        m_Reloading = true;
                        m_WeaponTimer.Stop();
                        m_WeaponTimer.Interval = m_ReloadingTime;
                        m_WeaponTimer.Start();

                    }
                    else
                    {

                        m_CanShoot = true;

                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            NBulletInCharger--;
                            double Radians = Math.Atan2(Mouse.GetState().Position.Y / EntityManager.Instance.Map.TileWidth * Map.EntityPixelPerCase - EntityManager.Instance.Joueur.Y, Mouse.GetState().Position.X / EntityManager.Instance.Map.TileWidth * Map.EntityPixelPerCase - EntityManager.Instance.Joueur.X) +
                                             ((m_RNG.NextDouble() * m_SpreadAngle) - m_SpreadAngle / 2.0) * (Math.PI / 180.0);
                            Vector2 MouseDir = new Vector2((float)Math.Cos(Radians), (float)Math.Sin(Radians));
                            EntityManager.Instance.ProjectilesListHostile.Add(new ProjectileBullet(TextureManager.TextureBullet, new Vector2(EntityManager.Instance.Joueur.X, EntityManager.Instance.Joueur.Y), new Vector2(8, 8), MouseDir * 500, 8) { Friendly = false });
                            m_WeaponTimer.Start();
                            JouerSonTir();

                        }
                    }
                }
            }




        }
        public override void MouseUp()
        {


        }
    }
}
