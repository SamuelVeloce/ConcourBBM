using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using TopDownGridBasedEngine;

namespace Competition.Armes
{
    class SemiAutomaticSniper : Weapons
    {
        public SemiAutomaticSniper(AbsEntity Owner) : base(Owner)
        {

        }

        public override int NBulletInCharger
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override int NBulletLeft
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override string WeaponName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void JouerSonTir()
        {
            throw new NotImplementedException();
        }

        public override void MouseDown()
        {
            throw new NotImplementedException();
        }

        public override void MouseDown(Point Target)
        {
            throw new NotImplementedException();
        }

        public override void MouseUp()
        {
            throw new NotImplementedException();
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }
    }
}
