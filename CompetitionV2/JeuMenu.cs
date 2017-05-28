using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Competition.Armes;
using CompetitionV2;
using CompetitionV2.Armes;
using TopDownGridBasedEngine.Projectile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TopDownGridBasedEngine.Armes;

namespace TopDownGridBasedEngine
{
    public class JeuMenu : IPartieDeJeu
    {

        public Hud _hud;
        public static Joueur m_Joueur;
        public static Map Map;
        private KeyWrapper _wrapper;
        private bool MouseDown = false;
        private double StartTime;

        public JeuMenu()
        {
            StartTime = 0;
            Game1.Penumbra.Lights.Clear();
            Game1.Penumbra.Hulls.Clear();
            Game1.Penumbra.AmbientColor = Color.Black;

            _hud = new Hud(Game1.Screen);
            Map = new Map(45, Game1.Screen.ClientBounds, 0);
           // m_Joueur = null;

            System.Random r = new System.Random();
            Point p;
            bool ShouldRun = true;
            do
            {
                p = new Point(r.Next() % Map.Width, r.Next() % Map.Height);
                if (!Map[p.X, p.Y].IsSolid)
                    if (m_Joueur?.Weapon == null)
                    {
                        m_Joueur = new Joueur(p.X * Map.EntityPixelPerCase, p.Y * Map.EntityPixelPerCase, Map);
                        ShouldRun=false;
                    }
                    else
                    {
                        Joueur tempJ = new Joueur(p.X * Map.EntityPixelPerCase, p.Y * Map.EntityPixelPerCase, Map);
                        for (int i = 0; i < m_Joueur.Weapon.Length; i++)
                        {
                            switch (m_Joueur.Weapon[i].WeaponType)
                            {
                                case WeaponType.Pistol:
                                    tempJ.Weapon[i] = new Pistol(tempJ);
                                    break;
                                case WeaponType.AssaultRifle:
                                    tempJ.Weapon[i] = new AssaultRifle(tempJ);
                                    break;
                                case WeaponType.Shotgun:
                                    tempJ.Weapon[i] = new Shotgun(tempJ);
                                    break;
                                case WeaponType.SemiAutoSniper:
                                    tempJ.Weapon[i] = new SemiAutomaticSniper(tempJ);
                                    break;
                            }
                        }
                        m_Joueur = tempJ;
                        ShouldRun = false;
                    }
            }
            while (ShouldRun);
            EntityManager.InitInstance(m_Joueur, Map, 0);

            
            Enemy e;
            for (int i = 0; i < 10; i++)
            {

                p = new Point(r.Next() % Map.Width, r.Next() % Map.Height);
                if (!Map[p.X, p.Y].IsSolid)
                {
                    e = new KamikazeRobot(p.X * Map.EntityPixelPerCase, p.Y * Map.EntityPixelPerCase, Map);
                    EntityManager.Instance.Add(e);
                }
                
                
            }

            _wrapper = new KeyWrapper(0, 0, 0);

        }


        public void Update(GameTime gameTime)
        {
            if (StartTime == 0)
            {
                StartTime = gameTime.TotalGameTime.TotalMilliseconds;
                ProgressManager.ArgentDernierePartie = 0;
            }
            else
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - StartTime > 120000)//2 minutes
                {
                    ProgressManager.ArgentDernierePartie = (int)(1.1*ProgressManager.ArgentDernierePartie);
                    Game1.SetPartieDeJeu((int)TypesDePartieDeJeu.Gagne);
                }
            }


            _wrapper.MouseX = Mouse.GetState().X;
            _wrapper.MouseY = Mouse.GetState().Y;
            _wrapper.State = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
                _wrapper.State |= KeyState.Up;
            if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
                _wrapper.State |= KeyState.Down;
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
                _wrapper.State |= KeyState.Left;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
                _wrapper.State |= KeyState.Right;
            _wrapper.ToggleSpace(Keyboard.GetState().IsKeyDown(Keys.Space));

            if (Keyboard.GetState().IsKeyDown(Keys.R))
                m_Joueur.CurrentWeapon().Reload();

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (!MouseDown)
                {
                    m_Joueur.CurrentWeapon().MouseDown();
                    MouseDown = true;
                }
            }
            else
            {
                if (MouseDown)
                {
                    m_Joueur.CurrentWeapon().MouseUp();
                    MouseDown = false;
                }
            }
            


            EntityManager.Instance.TickPlayer(0, gameTime, _wrapper);
            _wrapper.ResetSpace();
            EntityManager.Instance.TickEntities(gameTime);

        }

        public void DrawWithShadows(SpriteBatch sb, GameTime gameTime, GraphicsDevice gd)
        {
            // Light affected stuff.
            Game1.Penumbra.Visible = true;
            Game1.Penumbra.BeginDraw();

            gd.Clear(Color.Black);
            sb.Begin();
            Map.Draw(sb, Game1.Screen.ClientBounds);
            EntityManager.Instance.Draw(sb, Game1.Screen.ClientBounds);
            
            //EntityManager.Instance.DrawPlayers(_spriteBatch, Window.ClientBounds);
            sb.End();

            
        }

        public void DrawWithoutShadows(SpriteBatch sb, GameTime gameTime, GraphicsDevice gd)
        {
            sb.Begin();
            EntityManager.Instance.DrawPlayers(sb, Game1.Screen.ClientBounds);

            _hud.Draw(sb);
            sb.End();
        }
    }
}
