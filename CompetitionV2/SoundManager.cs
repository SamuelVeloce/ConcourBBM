using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public static SoundEffect EmptyGun;
        public static SoundEffect TrameSonoreMenu;

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
            EmptyGun = _content.Load<SoundEffect>("SoundEffect/EmptyGun");
            TrameSonoreMenu = _content.Load<SoundEffect>("SoundEffect/TrameSonore");

            /*
                Tous les effets sonores sont libres de droit et ont étés 
                téléchargés depuis https://www.zapsplat.com et édités à 
                l'aide de https://audiotrimmer.com/
            */
        }
    }
}
