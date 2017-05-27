using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitionV2
{
    public static class ProgressManager
    {
        private static int m_Argent;
        private static int m_LvlDebloque;

        private static bool[] m_ArmesDebloque;
        private static bool[] m_ArmesAchete;
        

     
        static ProgressManager()
        {
            Load();
        }

        private static void Load()
        {
            //todo load from disk
            Argent = 1000;
            LvlDebloque = 5;
            ArmesDebloque = new bool[] {true, true, true, true};
            ArmesAchete = ArmesDebloque;
        }


        public static void Save()
        {
            
        }

        public static int Argent
        {
            get { return m_Argent; }
            set { m_Argent = value; }
        }

        public static int LvlDebloque
        {
            get { return m_LvlDebloque; }
            set { m_LvlDebloque = value; }
        }

        public static bool[] ArmesDebloque
        {
            get { return m_ArmesDebloque; }
            set { m_ArmesDebloque = value; }
        }

        public static bool[] ArmesAchete
        {
            get { return m_ArmesAchete; }
            set { m_ArmesAchete = value; }
        }

       


    }
}
