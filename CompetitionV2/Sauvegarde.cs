using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



namespace TopDownGridBasedEngine
{
    [Serializable]
    public class Sauvegarde
    {
        private byte[] _DropCountForEachWeapon;

        public static Sauvegarde GetSauvegarde(FileStream fs)
        {
            BinaryFormatter bf = new BinaryFormatter();
            return (Sauvegarde)bf.Deserialize(fs);
        }


        public Sauvegarde()
        {
            _DropCountForEachWeapon = new byte[6]; // Change avec le nombre d'armes.
        }

        bool Save(FileStream fs)
        {
            if (fs.CanWrite == false)
                return false;

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, this);
            return true;
        }

        public bool IsWeaponUnlocked(WeaponType w)
        {
            if (_DropCountForEachWeapon[(int)w] >= 30)
                return true;
            else
                return false;
        }

        public void AddDrop(WeaponType Type)
        {
            _DropCountForEachWeapon[(int)Type] += 1;
        }
    }
}
