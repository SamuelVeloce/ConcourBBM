using Competition.Armes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TopDownGridBasedEngine;
using TopDownGridBasedEngine.Projectile;

namespace CompetitionV2.Armes
{
    class BoltActionSniperAI : Weapons
    {
        public override int NBulletLeft { get; set; }
        public override int NBulletInCharger { get; set; }

        private const int m_BulletSpeed = 2000;
        private const int m_ReloadingTime = 8000;
        private const int m_ClipSize = 5;
        private const int m_Firerate = 300;
        private const int m_SpreadAngle = 2;

        private readonly Random m_RNG = new Random();
        private readonly System.Timers.Timer m_WeaponTimer;

        private bool m_CanShoot = true;
        private bool m_Reloading;

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

        public BoltActionSniperAI(AbsEntity owner) : base(owner)
        {
            Owner = owner;

            NBulletLeft = 15;//int.MaxValue - 100;
            NBulletInCharger = m_ClipSize;
            m_WeaponTimer = new System.Timers.Timer(m_Firerate) { AutoReset = false };
            m_WeaponTimer.Elapsed += _timer_Elapsed;
            Nom = "Sniper";
        }

        public override int ClipSize
        {
            get { return m_ClipSize; }
        }

        public override void MouseDown()
        {
            //MouseDown();
        }


        public override void MouseDown(Point Target)
        {
            if (m_CanShoot)
            {
                m_CanShoot = false;
                m_WeaponTimer.Start();
                NBulletInCharger--;
                double Radians = Math.Atan2(Target.Y - Owner.Y, Target.X - Owner.X) + ((m_RNG.NextDouble() * m_SpreadAngle) - m_SpreadAngle / 2.0) * (Math.PI / 180.0);
                Vector2 MouseDir = new Vector2((float)Math.Cos(Radians), (float)Math.Sin(Radians));
                EntityManager.Instance.ProjectilesListHostile.Add(new ProjectileBullet(TextureManager.TextureBullet, new Vector2(Owner.X, Owner.Y), new Vector2(8, 8), MouseDir * m_BulletSpeed, 40) { Friendly = true });
                JouerSonTir();
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
                }
            }
        }
        public override void MouseUp()
        {
            //Do nothing in this case (somewhat useless)
        }

        public override WeaponType WeaponType
        {
            get
            {
                return WeaponType.SemiAutoSniper;
            }
        }

        public override void JouerSonTir()
        {
            SoundManager.Rifle.Play((float)0.2, 0, 0);
        }

    }
}
