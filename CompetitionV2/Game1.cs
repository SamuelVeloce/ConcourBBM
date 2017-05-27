using System.Runtime.CompilerServices;
using CompetitionV2.Menu;
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



        static IPartieDeJeu[] PartieDuJeu;
        private static int m_IndexPartieDeJeu;
        private static Game1 Instance;

        public static void SetPartieDeJeu(int i)
        {
            if (m_IndexPartieDeJeu != i)
            {
                switch (i)
                {
                    case (int)TypesDePartieDeJeu.Jeu:
                        PartieDuJeu[i] = new JeuMenu();
                        break;
                    case (int)TypesDePartieDeJeu.MenuDefaut:
                        PartieDuJeu[i] = new MenuDefaut();
                        break;
                    case (int)TypesDePartieDeJeu.Armurerie:
                        PartieDuJeu[i] = new Armurerie();
                        break;
                    case (int)TypesDePartieDeJeu.FermerJeu:
                        Instance.Exit();
                        break;
                }
                Instance.GraphicsDevice.Clear(Color.Gray);
                m_IndexPartieDeJeu = i;
            }
        }
    
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            //_graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IndexPartieDeJeu = 0;
            Instance = this;
        }

        public int IndexPartieDeJeu
        {
            get { return m_IndexPartieDeJeu; }
            set { m_IndexPartieDeJeu = value; }
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

            
            PartieDuJeu = new IPartieDeJeu[3];
            PartieDuJeu[(int)TypesDePartieDeJeu.MenuDefaut] = new MenuDefaut();
            PartieDuJeu[(int)TypesDePartieDeJeu.Jeu] = new JeuMenu();
            PartieDuJeu[(int)TypesDePartieDeJeu.Armurerie] = new Armurerie();

            
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
            

            PartieDuJeu[IndexPartieDeJeu].Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            FPSCounter++;
            PartieDuJeu[IndexPartieDeJeu].DrawWithShadows(_spriteBatch, gameTime, GraphicsDevice);

            if (gameTime.TotalGameTime.TotalMilliseconds - 1000 > FPSTime)
            {
                FPSTime = gameTime.TotalGameTime.TotalMilliseconds;
                LastFPS = FPSCounter;
                FPSCounter = 0;
            }
            
            base.Draw(gameTime);

            PartieDuJeu[IndexPartieDeJeu].DrawWithoutShadows(_spriteBatch, gameTime, GraphicsDevice);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(TextureManager.Font, "FPS: " + LastFPS, new Vector2(10, 10), Color.Yellow);
            _spriteBatch.End();
            

        }
        
    }
}