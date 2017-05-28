using System;
using System.Timers;
using CompetitionV2.Projectile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CompetitionV2.Armes
{
    class ShotgunAI : Weapons
    {
        public override int NBulletLeft { get; set; }
        public override int NBulletInCharger { get; set; }

        private readonly Random m_RNG;

        //public override string WeaponName { get { return "Shotgun"; } }
        private const byte m_BulletSpeed = 200;
        private const int m_ReloadingTime = 700;
        private const int m_SpreadAngle = 30;
        private const int m_NumberOfBuckshot = 5;
        private const int m_ClipSize = 8;
        private const int m_Firerate = 2000;
        private bool m_CanShoot = true;
        private bool m_Reloading;
        private readonly System.Timers.Timer m_WeaponTimer;

        public override int ClipSize
        {
            get { return m_ClipSize; }
        }
        public override void JouerSonTir()
        {
            SoundManager.Shotgun.Play((float)0.5, 0, 0);
        }

        public override WeaponType WeaponType => WeaponType.Shotgun;

        public ShotgunAI(AbsEntity owner):base(owner)
        {
            Owner = owner;
            Nom = "Shotgun";
            m_RNG = new Random();
            NBulletLeft = 16;//int.MaxValue - 100;
            NBulletInCharger = m_ClipSize;
            m_WeaponTimer = new System.Timers.Timer(m_Firerate) { AutoReset = false };
            m_WeaponTimer.Elapsed += _timer_Elapsed;

        }

        public override void Reload()
        {
            if (!m_Reloading && NBulletInCharger < m_ClipSize)
            {
                //          NBulletLeft += NBulletInCharger;
                //           NBulletInCharger = 0;
                m_Reloading = true;
                m_CanShoot = false;
                m_WeaponTimer.Interval = m_ReloadingTime;
                m_WeaponTimer.Start();
            }
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
                m_WeaponTimer.Start();

                NBulletInCharger--;

                for (int i = 0; i < m_NumberOfBuckshot; i++)
                {
                    double Radians = Math.Atan2((Target.Y) - Owner.Y, Target.X - Owner.X) + ((m_RNG.NextDouble() * m_SpreadAngle) - m_SpreadAngle / 2.0) * (Math.PI / 180.0);
                    Vector2 MouseDir = new Vector2((float)Math.Cos(Radians), (float)Math.Sin(Radians));
                    EntityManager.Instance.ProjectilesListHostile.Add(new ProjectileBullet(
                        TextureManager.TextureBullet, new Vector2(Owner.X, Owner.Y), new Vector2(8, 8), MouseDir * m_BulletSpeed,
                        15)
                    { Friendly = false });
                }

                JouerSonTir();
            }
            else
            {
                if (m_Reloading)
                {
                    m_Reloading = false;
                    if (NBulletInCharger > 0)
                    {
                        m_CanShoot = true;
                    }
                    m_WeaponTimer.Interval = m_Firerate;

                    m_WeaponTimer.Start();
                }

            }
        }
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
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
                    if (NBulletLeft > 0)
                    {
                        NBulletLeft--;
                        NBulletInCharger++;
                        if (NBulletInCharger < m_ClipSize && NBulletLeft > 0)
                        {
                            m_WeaponTimer.Start();
                        }
                        else
                        {
                            m_WeaponTimer.Stop();
                            m_WeaponTimer.Interval = m_Firerate;
                            m_Reloading = false;
                            m_CanShoot = true;
                        }
                    }


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
                }
            }

        }
        public override void MouseUp()
        {
            //Do nothing in this case (somewhat useless)
        }

    }
}

