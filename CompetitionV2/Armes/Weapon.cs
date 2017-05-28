using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TopDownGridBasedEngine;

namespace Competition.Armes
{

    public enum WeaponType
    {
        Pistol = 0,
        AssaultRifle = 1,
        Shotgun = 2,
        SemiAutoSniper = 3,

    };

    public abstract class Weapons //Classe abstraite utilisé comme template pour toutes les armes
    {

        public abstract WeaponType WeaponType { get; }
        private string m_Nom;
        public abstract void MouseDown();//Vector2 MouseDir); //methode utilisé quand un bouton de la sourie est appuyé
        public abstract void MouseDown(Point Target);//Vector2 MouseDir); //methode utilisé quand un bouton de la sourie est appuyé
        public abstract void MouseUp();//methode utilisé quand un bouton de la sourie est relaché (utilisé pour les armes automatiques)
        public abstract void Reload();//methode utilisé quand le joueur recharge une arme
        public abstract int ClipSize { get; }
        //  public abstract void Reloaded();
        public abstract int NBulletLeft { get; set; } //propriété utilisé pour obtenir ou modifier pour le nombre de balles restantes en inventaire
        public abstract int NBulletInCharger { get; set; }//propriété utilisé pour obtenir ou modifier pour le nombre de balles restantes dans le chargeur
   //     public abstract Vector2 MouseDirection { set; } //propriété utilisé pour mettre a jours la direction pointé par la sourie
     //   public abstract string WeaponName { get; } //le nom de l'arme... duuuuh

        protected Weapons (AbsEntity Owner)
        {
            this.Owner = Owner;
        }

        public string Nom
        {
            get { return m_Nom; }
            set { m_Nom = value; }
        }

        public void JouerSonVide()
        {
            SoundManager.EmptyGun.Play();
        }

        public AbsEntity Owner { get; set; }

//Donnés de l'utilisateur de l'arme
      

        public abstract void JouerSonTir();//methode pour jouer le son de l'arme


    }


    
}
