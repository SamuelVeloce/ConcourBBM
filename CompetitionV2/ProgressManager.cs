namespace CompetitionV2
{
    public static class ProgressManager
    {
        private static int m_Argent;
        private static int m_LvlDebloque;

        private static bool[] m_ArmesDebloque;
        private static bool[] m_ArmesAchete;

        private static int m_ArgentDernierePartie;

        private static double m_TempsSurvecuDernierePartie;
     
        static ProgressManager()
        {
            Load();
        }

        private static void Load()
        {
            //todo load from disk
            Argent = 1000;
            LvlDebloque = 2;
            ArmesDebloque = new bool[] {true, true, true, true,true};
            ArmesAchete = ArmesDebloque;
            ArgentDernierePartie = 0;
            m_TempsSurvecuDernierePartie = 0;
        }


        public static void Save()
        {
            
        }

        public static int Argent
        {
            get { return m_Argent; }
            set
            {
                m_Argent = value;
            }
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

        public static int ArgentDernierePartie
        {
            get { return m_ArgentDernierePartie; }
            set { m_ArgentDernierePartie = value; }
        }

        public static double TempsSurvecuDernierePartie
        {
            get { return m_TempsSurvecuDernierePartie; }
            set { m_TempsSurvecuDernierePartie = value; }
        }
    }
}
