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
        private SpriteFont font;


        IPartieDeJeu PartieDuJeu;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.PreferredBackBufferWidth = 900;
            _graphics.ApplyChanges();
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Screen = Window;
            Penumbra = new PenumbraComponent(this);
            Components.Add(Penumbra);
            Penumbra.AmbientColor = Color.White;
            TextureManager.InitInstance(Content);
            PartieDuJeu = new JeuMenu();


            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Font/Font");
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
            _spriteBatch.DrawString(font, "FPS: " + LastFPS, new Vector2(10, 10), Color.Yellow);
            _spriteBatch.End();
            base.Draw(gameTime);
            PartieDuJeu.DrawWithoutShadows(_spriteBatch, gameTime, GraphicsDevice);

        }
        
    }
}