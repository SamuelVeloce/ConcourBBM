﻿/************************
 * Samuel Goulet
 * Novembre 2016
 * Classe Singleton TextureManager, pour s'occuper des textures du jeu
 ***********************/
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace TopDownGridBasedEngine
{
    // Classe Singleton TextureManager, pour s'occuper des textures du jeu
    public class TextureManager
    {
        // Déclaration des textures possibles

        public Texture2D TextureCaseVide;
        public Texture2D TextureCaseWall;
        public Texture2D TextureCaseSolidWall;
        public Texture2D TextureCaseVerte;
        public Texture2D[,] TextureCaseBonus;

        public Texture2D[] TexturePlayerLeft;
        public Texture2D[] TexturePlayerRight;
        public Texture2D[] TexturePlayerUp;
        public Texture2D[] TexturePlayerDown;

        public Texture2D[] TextureFire;
        public Texture2D[] TextureBomb;
        
        public static Texture2D[] TextureBullet;

        public static Texture2D BackgroundImage;
        
        
        //Textures du jeu cyborg
        public static Texture2D TextureBarricade;
        public static Texture2D[] TextureTerre;
        public static Texture2D[] Robots;
        public static Texture2D Cyborg;
        public static Texture2D Explosion;


        public static SpriteFont Font;

        // Déclaration de l'instance unique de la classe
        private static TextureManager _instance;

        private readonly ContentManager _content;

        // Constructeur privé
        private TextureManager(ContentManager content)
        {
            _content = content;
            LoadAllTextures();
        }

        /// <summary>
        /// Propriété publique pour accéder à l'instance (qui doit avoir été préalablement créée)
        /// </summary>
        public static TextureManager Instance
        {
            get
            {
                if (_instance == null)
                    throw new ArgumentNullException("Singleton instance not created");
                return _instance;
            }
        }

        /// <summary>
        /// Création de l'instance
        /// </summary>
        /// <param name="content"></param>
        public static void InitInstance(ContentManager content)
        {
            if (_instance != null)
                _instance.LoadAllTextures();
            else
            {
                _instance = new TextureManager(content);
                _instance.LoadAllTextures();
            }
        }

        /// <summary>
        /// Recherche toutes les textures et les store en mémoire
        /// </summary>
        private void LoadAllTextures()
        {
            // Création des tableaux
            TexturePlayerLeft = new Texture2D[4];
            TexturePlayerRight = new Texture2D[4];
            TexturePlayerUp = new Texture2D[4];
            TexturePlayerDown = new Texture2D[4];
            TextureCaseBonus = new Texture2D[6, 2];
            TextureFire = new Texture2D[2];
            TextureBomb = new Texture2D[4];
            TextureBullet = new Texture2D[1];


            // Textures de cases
            TextureCaseVide = _content.Load<Texture2D>("Textures/Ground");
            TextureCaseWall = _content.Load<Texture2D>("Textures/Wall");
            TextureCaseSolidWall = _content.Load<Texture2D>("Textures/SolidWall");
            TextureCaseVerte = _content.Load<Texture2D>("Textures/Grass");

            TextureFire[0] = _content.Load<Texture2D>("Textures/TextureCaseFire1");
            TextureFire[1] = _content.Load<Texture2D>("Textures/TextureCaseFire2");

            // Textures de joueurs et de bombes
            TextureBomb[0] = _content.Load<Texture2D>("Textures/TextureBomb1");
            TexturePlayerUp[0] = _content.Load<Texture2D>("Textures/TexturePlayerBack1");
            TexturePlayerDown[0] =  _content.Load<Texture2D>("Textures/TexturePlayerFront1");
            TexturePlayerRight[0] = _content.Load<Texture2D>("Textures/TexturePlayerRight1");
            TexturePlayerLeft[0] = _content.Load<Texture2D>("Textures/TexturePlayerLeft1");
            TextureBomb[1] = _content.Load<Texture2D>("Textures/TextureBomb2");
            TexturePlayerUp[1] = _content.Load<Texture2D>("Textures/TexturePlayerBack2");
            TexturePlayerDown[1] =  _content.Load<Texture2D>("Textures/TexturePlayerFront2");
            TexturePlayerRight[1] = _content.Load<Texture2D>("Textures/TexturePlayerRight2");
            TexturePlayerLeft[1] = _content.Load<Texture2D>("Textures/TexturePlayerLeft2");
            TextureBomb[3] = _content.Load<Texture2D>("Textures/TextureBomb4");
            TexturePlayerUp[3] = _content.Load<Texture2D>("Textures/TexturePlayerBack4");
            TexturePlayerDown[3] =  _content.Load<Texture2D>("Textures/TexturePlayerFront4");
            TexturePlayerRight[3] = _content.Load<Texture2D>("Textures/TexturePlayerRight4");
            TexturePlayerLeft[3] = _content.Load<Texture2D>("Textures/TexturePlayerLeft4");
            TextureBomb[2] = TextureBomb[0];
            TexturePlayerUp[2] = TexturePlayerUp[0];
            TexturePlayerDown[2] = TexturePlayerDown[0];
            TexturePlayerRight[2] = TexturePlayerRight[0];
            TexturePlayerLeft[2] = TexturePlayerLeft[0];
                        
            // Textures des bonus
            TextureCaseBonus[0, 0] = _content.Load<Texture2D>("Textures/TextureBonusExtraBomb");
            TextureCaseBonus[0, 1] = _content.Load<Texture2D>("Textures/TextureBonusExtraBomb2");
            TextureCaseBonus[1, 0] = _content.Load<Texture2D>("Textures/TextureBonusPower");
            TextureCaseBonus[1, 1] = _content.Load<Texture2D>("Textures/TextureBonusPower2");
            TextureCaseBonus[2, 0] = _content.Load<Texture2D>("Textures/TextureBonusSpeed");
            TextureCaseBonus[2, 1] = _content.Load<Texture2D>("Textures/TextureBonusSpeed2");
            TextureCaseBonus[3, 0] = _content.Load<Texture2D>("Textures/TextureBonusShoot");
            TextureCaseBonus[3, 1] = _content.Load<Texture2D>("Textures/TextureBonusShoot2");
            TextureCaseBonus[4, 0] = _content.Load<Texture2D>("Textures/TextureBonusKick");
            TextureCaseBonus[4, 1] = _content.Load<Texture2D>("Textures/TextureBonusKick2");
            TextureCaseBonus[5, 0] = _content.Load<Texture2D>("Textures/TextureBonusMaxExplosion");
            TextureCaseBonus[5, 1] = _content.Load<Texture2D>("Textures/TextureBonusMaxExplosion2");


            TextureBullet[0] = _content.Load<Texture2D>("Textures/Bullet");
            //
            //Textures jeu cyborg vs robots
            //

            //Environnement
            TextureBarricade = _content.Load<Texture2D>("Textures/TileBarricade");

            //Arrière-Plan
            TextureTerre = new Texture2D[2];
            TextureTerre[0] = _content.Load<Texture2D>("Textures/TileTerre1");
            TextureTerre[1] = _content.Load<Texture2D>("Textures/TileTerre2");

            //Robots
            Robots = new Texture2D[4];
            Robots[0] = _content.Load<Texture2D>("Textures/Drone");
            Robots[1] = _content.Load<Texture2D>("Textures/Destroyer");
            Robots[2] = _content.Load<Texture2D>("Textures/Warmachine");
            Robots[3] = _content.Load<Texture2D>("Textures/Warmachine2");

            //Cyborg
            Cyborg = _content.Load<Texture2D>("Textures/Cyborg");

            //Explosion
            Explosion = _content.Load<Texture2D>("Textures/Explode");

            Font = _content.Load<SpriteFont>("Font/Font");



            BackgroundImage = _content.Load<Texture2D>("Textures/BackgroundBlur");

            /* Sources des textures
             * Toutes les textures sont libres de droits et peuvent être redistribuées gratuitement
                https://opengameart.org/content/dirt-004
                https://opengameart.org/content/dirt-004
                http://hasgraphics.com/8-bit-sinistar-clone-graphics/
                http://untamed.wild-refuge.net/images/rpgxp/avengers/warmachine.png (Warmachine1)
                http://untamed.wild-refuge.net/images/rpgxp/avengers/ironpatriot.png (Warmachine2)
                http://untamed.wild-refuge.net/images/rpgxp/mandalorian.png (Cyborg)
            */

        }
    }
}
