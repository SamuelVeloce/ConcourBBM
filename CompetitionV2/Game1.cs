using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Penumbra;

namespace TopDownGridBasedEngine
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static GameWindow Screen;
        public static PenumbraComponent Penumbra;


        private int FPSCounter;
        private double FPSTime;
        private int LastFPS;


        IPartieDeJeu PartieDuJeu;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            int Dimension = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
            _graphics.PreferredBackBufferHeight = Dimension;
            _graphics.PreferredBackBufferWidth = Dimension;
            _graphics.ApplyChanges();
            IsMouseVisible = true;
            //Window.AllowUserResizing = true;
            Screen = Window;
            Penumbra = new PenumbraComponent(this);
            Components.Add(Penumbra);
            Penumbra.AmbientColor = Color.White;
            TextureManager.InitInstance(Content);
            PartieDuJeu = new JeuMenu();
            SoundManager.InitInstance(Content);
            SoundManager.TrameSonoreJeu.Play((float)0.5, 0, 0);
            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            
            Penumbra.Initialize();
            Penumbra.Visible = true;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == 
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                    Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad9))
                PartieDuJeu = new JeuMenu();

            PartieDuJeu.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            FPSCounter++;
            PartieDuJeu.DrawWithShadows(_spriteBatch, gameTime, GraphicsDevice);

            if (gameTime.TotalGameTime.TotalMilliseconds - 1000 > FPSTime)
            {
                FPSTime = gameTime.TotalGameTime.TotalMilliseconds;
                LastFPS = FPSCounter;
                FPSCounter = 0;
            }
            _spriteBatch.Begin();
            _spriteBatch.DrawString(TextureManager.Font, "FPS: " + LastFPS, new Vector2(10, 10), Color.Yellow);
            _spriteBatch.End();
            base.Draw(gameTime);
            PartieDuJeu.DrawWithoutShadows(_spriteBatch, gameTime, GraphicsDevice);

        }
        
    }
}