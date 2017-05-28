using CompetitionV2.Armes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CompetitionV2
{
    public enum BonusType {Argent,Munitions,Sante, Arme };
    public class Bonus : AbsEntity
    {
        private readonly BonusType bonType;
        public Texture2D Texture;

        public Bonus(int x, int y, Map m, BonusType Type) : base(x, y, m)
        {
            this.Size = 20;
            bonType = Type;

            switch (this.bonType)
            {
                case BonusType.Argent:
                    Texture = TextureManager.Bonus[1];
                    break;

                case BonusType.Arme:
                    Texture = TextureManager.Bonus[3];
                    break;

                case BonusType.Munitions:
                    Texture = TextureManager.Bonus[0];
                    break;

                case BonusType.Sante:
                    Texture = TextureManager.Bonus[2];
                    break;
            }

            EntityManager.Instance.Bonus.Add(this);
        }

        public override EntityType Type => EntityType.Bonus;

        public override void Draw(SpriteBatch sb, float width)
        {
            Draw(sb, width, Color.White);
        }

        public override void Draw(SpriteBatch sb, float width, Color color)
        {
            sb.Draw(this.Texture, new Rectangle((int)(X * width / Map.EntityPixelPerCase), (int)(Y * width / Map.EntityPixelPerCase), (int)(Size), (int)(Size)), color);
        }

        public override void Tick(long deltaTime)
        {
            Joueur j = EntityManager.Instance.Joueur;
            if (j.X > this.X - 10 && j.X < this.X + this.Size + 10 &&
                j.Y > this.Y - 10 && j.Y < this.Y + this.Size + 10)
            {
                switch(this.bonType)
                {
                    case BonusType.Argent:
                        //Bonus d'argent
                        ProgressManager.ArgentDernierePartie += 100;
                        break;

                    case BonusType.Arme:
                        int i = 0;
                        //trouve la prochaine arme à débloquer
                        while (i < ProgressManager.ArmesDebloque.Length && ProgressManager.ArmesDebloque[i])
                        {
                            i++;
                        }
                        //Débloque l'arme
                        if (i < ProgressManager.ArmesDebloque.Length)
                        {
                            ProgressManager.ArmesDebloque[i] = true;
                        }
                        break;

                    case BonusType.Munitions:
                        //Ajoute 2 chargeurs à chaque arme porté par le joueur
                        foreach(Weapons w in EntityManager.Instance.Joueur.Weapon)
                        {
                            w.NBulletLeft += w.ClipSize * 2;
                        }
                        break;

                    case BonusType.Sante:
                        //Remet la santé du joueur à 100%
                        EntityManager.Instance.Joueur.Health = EntityManager.Instance.Joueur.Health >= EntityManager.Instance.Joueur.MaxHealth - 100 ?
                            EntityManager.Instance.Joueur.MaxHealth : EntityManager.Instance.Joueur.Health + 100;
                        break;
                }
                EntityManager.Instance.Bonus.Remove(this); 

            }

        }

    }
}
