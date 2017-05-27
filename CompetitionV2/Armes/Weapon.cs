using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Competition.Armes
{
    public abstract class Weapons //Classe abstraite utilisé comme template pour toutes les armes
    {
        public abstract void MouseDown();//Vector2 MouseDir); //methode utilisé quand un bouton de la sourie est appuyé
        public abstract void MouseUp();//methode utilisé quand un bouton de la sourie est relaché (utilisé pour les armes automatiques)
        public abstract void Reload();//methode utilisé quand le joueur recharge une arme
                                      //  public abstract void Reloaded();
        public abstract int NBulletLeft { get; set; } //propriété utilisé pour obtenir ou modifier pour le nombre de balles restantes en inventaire
        public abstract int NBulletInCharger { get; set; }//propriété utilisé pour obtenir ou modifier pour le nombre de balles restantes dans le chargeur
   //     public abstract Vector2 MouseDirection { set; } //propriété utilisé pour mettre a jours la direction pointé par la sourie
        public abstract string WeaponName { get; } //le nom de l'arme... duuuuh
        public Vector2 User { get; set; } //Donnés de l'utilisateur de l'arme
      

        public abstract void JouerSonTir();//methode pour jouer le son de l'arme


    }


    sealed class Pistol : Weapons
    {

        public override int NBulletLeft { get; set; }
        public override int NBulletInCharger { get; set; }

        public override string WeaponName { get { return "Pistolet"; } }
        private const byte m_BulletSpeed = 25;
        private const int m_ReloadingTime = 2000;
        private const int m_ClipSize = 17;
        private const int m_Firerate = 70;
        private const int m_SpreadAngle = 2;
        private readonly Random m_RNG = new Random();
        private bool m_CanShoot = true;
        private bool m_Reloading;
        private readonly System.Timers.Timer m_WeaponTimer;
       


        private volatile bool m_ShootingSound;
        public override void JouerSonTir()
        {
            //TODO
            //ShootingSound.Play(0.3f, 1,0);
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
      //  private Vector2 m_MouseDir;
      /*  public override Vector2 MouseDirection
        {
            set { m_MouseDir = value; }
        }*/

        public Pistol()
        {
            NBulletLeft = int.MaxValue - 100;
            NBulletInCharger = m_ClipSize;
            m_WeaponTimer = new System.Timers.Timer(m_Firerate) { AutoReset = false };
            m_WeaponTimer.Elapsed += _timer_Elapsed;


        }
        public override void MouseDown()//Vector2 MouseDir)
        {
            if (m_CanShoot)
            {
                m_CanShoot = false;
                m_WeaponTimer.Start();
                //m_MouseDir = MouseDir;
                NBulletInCharger--;

                double Radians = Math.Atan2(Mouse.GetState().Position.Y, Mouse.GetState().Position.X) + ((m_RNG.NextDouble() * m_SpreadAngle) - m_SpreadAngle / 2.0) * (Math.PI / 180.0);
                Vector2 MouseDir = new Vector2((float)Math.Cos(Radians), (float)Math.Sin(Radians));
               // m_Player.AddProjectile(new Projectile(m_Player.Position, MouseDir, m_BulletSpeed, (byte)ProjectileType.Bullet));

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




    }
}
