using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompetitionV2
{
    enum TypesDePartieDeJeu
    {
        MenuDefaut = 0,
        Jeu = 1,
        Armurerie = 2,
        FermerJeu = 3,
        Perdu = 4,
        Gagne = 5,

    }
    public interface IPartieDeJeu
    {
        void DrawWithShadows(SpriteBatch sb, GameTime gameTime, GraphicsDevice gd);
        void DrawWithoutShadows(SpriteBatch sb, GameTime gameTime, GraphicsDevice gd);
        void Update(GameTime gameTime);
    }
}
