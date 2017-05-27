using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TopDownGridBasedEngine
{
    public class JeuMenu : IPartieDeJeu
    {

        public Hud _hud;
        public static Joueur Joueur;
        public static Map Map;
        private KeyWrapper _wrapper;


        public JeuMenu()
        {
            Game1.Penumbra.Lights.Clear();
            Game1.Penumbra.Hulls.Clear();
            Game1.Penumbra.AmbientColor = Color.Black;

            _hud = new Hud(Game1.Screen);
            Map = new Map(45, Game1.Screen.ClientBounds);

            Joueur = new Joueur(40, 40, Map);
            EntityManager.InitInstance(Joueur, Map, 0);

            System.Random r = new System.Random();
            Enemy e;
            for (int i = 0; i < 50; i++)
            {
                e = new Enemy(r.Next() % 900, r.Next() % 900, Map, 0.25f);
                EntityManager.Instance.Add(e);
            }

            _wrapper = new KeyWrapper(0, 0, 0);

        }


        public void Update(GameTime gameTime)
        {
            _wrapper.MouseX = Mouse.GetState().X;
            _wrapper.MouseY = Mouse.GetState().Y;
            _wrapper.State = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                _wrapper.State |= KeyState.Up;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                _wrapper.State |= KeyState.Down;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                _wrapper.State |= KeyState.Left;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                _wrapper.State |= KeyState.Right;
            _wrapper.ToggleSpace(Keyboard.GetState().IsKeyDown(Keys.Space));
            EntityManager.Instance.TickPlayer(0, gameTime.ElapsedGameTime.Milliseconds, _wrapper);
            _wrapper.ResetSpace();
            EntityManager.Instance.TickEntities(gameTime.ElapsedGameTime.Milliseconds);

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
