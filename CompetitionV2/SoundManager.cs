using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace CompetitionV2
{
    class SoundManager
    {
        private static SoundManager _instance;
        private static ContentManager _content;

        public static SoundEffect Explosion;
        public static SoundEffect ExplosionDebris;
        public static SoundEffect ImpactRobot;
        public static SoundEffect Pistol;
        public static SoundEffect Rifle;
        public static SoundEffect Shotgun;
        public static SoundEffect EmptyGun;
        public static SoundEffect TrameSonoreMenu;
        public static SoundEffect TrameSonoreJeu;

        private SoundManager(ContentManager content)
        {
            _content = content;
            LoadAllSounds();
        }

        public static void InitInstance(ContentManager content)
        {
            if (_instance != null)
                _instance.LoadAllSounds();
            else
            {
                _instance = new SoundManager(content);
                _instance.LoadAllSounds();
            }
        }

        private void LoadAllSounds()
        {
            Explosion = _content.Load<SoundEffect>("SoundEffect/Explosion");
            ExplosionDebris = _content.Load<SoundEffect>("SoundEffect/ExplosionDebris");
            ImpactRobot = _content.Load<SoundEffect>("SoundEffect/ImpactRobot");
            Pistol = _content.Load<SoundEffect>("SoundEffect/PistolShot");
            Rifle = _content.Load<SoundEffect>("SoundEffect/RifleShot");
            Shotgun = _content.Load<SoundEffect>("SoundEffect/Shotgun");
            EmptyGun = _content.Load<SoundEffect>("SoundEffect/EmptyGun");
            TrameSonoreMenu = _content.Load<SoundEffect>("SoundEffect/TrameSonore");
            TrameSonoreJeu = _content.Load<SoundEffect>("SoundEffect/ExtremeAction");
            /*
                Tous les effets sonores sont libres de droit et ont étés 
                téléchargés depuis https://www.zapsplat.com et édités à 
                l'aide de https://audiotrimmer.com/

                La trame sonore du jeu a été téléchargée sur le site:
                http://www.bensound.com/
            */
        }
    }
}
