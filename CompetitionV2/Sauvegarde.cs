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
    public static class Sauvegarde
    {
        private static byte[] _DropCountForEachWeapon;

        public static void SetSauvegarde(FileStream fs)
        {
            BinaryFormatter bf = new BinaryFormatter();
            _DropCountForEachWeapon = (byte[])bf.Deserialize(fs);
        }

        public static void SetSauvegarde()
        {
            _DropCountForEachWeapon = new byte[6]; // Change selon le nombre d'armes.
        }

        static bool Save(FileStream fs)
        {
            if (fs.CanWrite == false)
                return false;

            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, _DropCountForEachWeapon);
            return true;
        }

        public static bool IsWeaponUnlocked(WeaponType w)
        {
            if (_DropCountForEachWeapon[(int)w] >= 30)
                return true;
            else
                return false;
        }

        public static void AddDrop(WeaponType Type)
        {
            _DropCountForEachWeapon[(int)Type] += 1;
        }
    }
}
