using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Competition.Armes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TopDownGridBasedEngine;

namespace CompetitionV2.Projectile
{
    sealed class Pistol : Weapons
    {

        public override int NBulletLeft { get; set; }
        public override int NBulletInCharger { get; set; }

        public override string WeaponName { get { return "Pistolet"; } }
       // private const byte m_BulletSpeed = 25;
        private const int m_ReloadingTime = 2000;
        private const int m_ClipSize = 17;
        private const int m_Firerate = 70;
        private const int m_SpreadAngle = 2;
        private readonly Random m_RNG = new Random();
        private bool m_CanShoot = true;
        private bool m_Reloading;
        private readonly System.Timers.Timer m_WeaponTimer;
        
        public override void JouerSonTir()
        {
            SoundManager.Pistol.Play((float)0.5,0,0);
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
        

        public Pistol()
        {
            NBulletLeft = int.MaxValue - 100;
            NBulletInCharger = m_ClipSize;
            m_WeaponTimer = new System.Timers.Timer(m_Firerate) { AutoReset = false };
            m_WeaponTimer.Elapsed += _timer_Elapsed;
            Nom = "Pistol";

        }
        public override void MouseDown()//Vector2 MouseDir)
        {
            if (m_CanShoot)
            {
                m_CanShoot = false;
                m_WeaponTimer.Start();
                //m_MouseDir = MouseDir;
                NBulletInCharger--;

                double Radians = Math.Atan2(Mouse.GetState().Position.Y / EntityManager.Instance.Map.TileWidth * Map.EntityPixelPerCase - EntityManager.Instance.Joueur.Y, Mouse.GetState().Position.X/ EntityManager.Instance.Map.TileWidth * Map.EntityPixelPerCase - EntityManager.Instance.Joueur.X) + ((m_RNG.NextDouble() * m_SpreadAngle) - m_SpreadAngle / 2.0) * (Math.PI / 180.0);
                Vector2 MouseDir = new Vector2((float)Math.Cos(Radians), (float)Math.Sin(Radians));
                EntityManager.Instance.ProjectilesListFriendly.Add(new ProjectileBullet(TextureManager.TextureBullet,new Vector2(EntityManager.Instance.Joueur.X,EntityManager.Instance.Joueur.Y),new Vector2(8,8), MouseDir*1000,100));

                JouerSonTir();
            }
            else
            {
                JouerSonVide();
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
            //int i = 9;  //debug
            //Do nothing in this case (somewhat useless)
        }




    }
}
