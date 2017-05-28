using CompetitionV2.Armes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using System.Timers;
using Microsoft.Xna.Framework;

namespace TopDownGridBasedEngine
{
    class KamikazeRobot : Enemy     
    {
        Timer timer;
        bool exploding;

        public KamikazeRobot(int x, int y, Map m) : base(x, y, m, 0.60f) // Speedfactor changed there
        {
            _Hp = 10;
            SelfService ss = new SelfService(this);
            ss.BOOM += KamikazeRobot_BOOM;
            Arme = ss;
            DistanceFromPlayer = 0;
            this.Died += KamikazeRobot_Died;
            Couleur = Color.White;

            timer = new Timer();
            timer.AutoReset = false;
            timer.Interval = 500;
            timer.Elapsed += Timer_Elapsed;
            exploding = false;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            FireDied(sender, new CancellableEventArgs(false));
            EntityManager.Instance.Remove(this);
        }

        private void KamikazeRobot_BOOM(object sender, EventArgs e)
        {
            exploding = true;
            timer.Start();
        }

        private void KamikazeRobot_Died(object sender, CancellableEventArgs e)
        {
            ((SelfService)Arme).STOP = true;
        }



        public override Texture2D Texture
        {
            get
            {
                if (exploding)
                    return TextureManager.Explosion;
                return TextureManager.Instance.TextureLapin[_textureVariant / 20];
            }
        }

        public override void Tick(long deltaTime)
        {
            if (!exploding)
                base.Tick(deltaTime);
        }

        public override void UpdateTexture(long deltaTime)
        {
            //_textureVariant += (int)deltaTime / 1;
            if (_textureVariant > 59)
                _textureVariant %= 60;

        }



    }
}
