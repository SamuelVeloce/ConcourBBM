using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TopDownGridBasedEngine;
using Competition.Armes;

namespace CompetitionV2
{
    public enum BonusType {Argent,Munitions,Sante, Arme };
    public class Bonus : AbsEntity
    {
        private BonusType bonType;

        public Bonus(int x, int y, Map m, BonusType Type) : base(x, y, m)
        {
            this.Size = 10;
            bonType = Type;
            EntityManager.Instance.Bonus.Add(this);
        }

        public Texture2D Texture => TextureManager.TextureBullet[0];

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
                        //*************
                        break;

                    case BonusType.Arme:
                        int i = 0;
                        //trouve la prochaine arme à ssss
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
                        EntityManager.Instance.Joueur.Health = 100;
                        break;
                }
                EntityManager.Instance.Bonus.Remove(this); 

            }

        }

    }
}
