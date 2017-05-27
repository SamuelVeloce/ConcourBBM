using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Competition.Armes;

namespace TopDownGridBasedEngine
{
    [Serializable]
    public static class Sauvegarde
    {
        private static byte[] _DropCountForEachWeapon;

        public static void SetSauvegarde()
        {
            if (!File.Exists("../Save/Safe.Sound"))
            {
                _DropCountForEachWeapon = new byte[6]; // Change selon le nombre d'armes.
            }
            else
            {
                FileStream fs = new FileStream("../Save/Safe.Sound", FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                _DropCountForEachWeapon = (byte[])bf.Deserialize(fs);
                fs.Close();
            }
        }

        public static void Save()
        {
            FileStream fs = new FileStream("../Save/Safe.Sound", FileMode.OpenOrCreate);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, _DropCountForEachWeapon);

            fs.Close();
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
