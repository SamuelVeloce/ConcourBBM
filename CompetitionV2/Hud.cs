using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompetitionV2
{
    public class Hud
    {
        private GameWindow _window;
        private bool ShowTuto;
        //private Joueur _joueur;

        public Hud(GameWindow win)
        {
            _window = win;
            Timer tutoTimer = new Timer();
            tutoTimer.Interval = 10000;
            tutoTimer.Elapsed += TutoTimer_Elapsed;
            ShowTuto = true;
        }

        private void TutoTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ShowTuto = false;
        }

        public void Draw(SpriteBatch sb)
        {
            //Gestion du statut de l'armes (Balles)
            int nbBalles = EntityManager.Instance.Joueur.CurrentWeapon().NBulletInCharger;
            int nbTotBalles = EntityManager.Instance.Joueur.CurrentWeapon().NBulletLeft;
            int ChargeurCap = EntityManager.Instance.Joueur.CurrentWeapon().ClipSize;
            string strBalles = "Balles: ";
            if (nbBalles == 0)
            {
                if (nbTotBalles == 0)
                {
                    strBalles += "Vide";
                }
                else
                {
                    strBalles += "Recharg.";
                }
            }
            else
            {
                strBalles += nbBalles.ToString() + '/' + ChargeurCap.ToString();
            }
            string TotalBalle = "Balles retantes: " + (nbTotBalles < 99999 ? nbTotBalles.ToString() : "Infini");

            //Dessine les informations dans le coin supérieur droit
            sb.DrawString(TextureManager.Font, EntityManager.Instance.Joueur.CurrentWeapon().Nom, new Vector2(Game1.Screen.ClientBounds.Width - 200, 10), Color.Yellow);
            sb.DrawString(TextureManager.Font, EntityManager.Instance.Entities.Count.ToString() + " Enemis", new Vector2(Game1.Screen.ClientBounds.Width - 200, 30), Color.Yellow);
            sb.DrawString(TextureManager.Font, strBalles, new Vector2(Game1.Screen.ClientBounds.Width - 200, 50), Color.Red);
            sb.DrawString(TextureManager.Font, TotalBalle, new Vector2(Game1.Screen.ClientBounds.Width - 200, 70), Color.Yellow);
            sb.DrawString(TextureManager.Font, $"HP: {EntityManager.Instance.Joueur.Health} / {EntityManager.Instance.Joueur.MaxHealth}", new Vector2(Game1.Screen.ClientBounds.Width - 200, 90), Color.Yellow);

        }
    }
}
