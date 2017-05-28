using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;

using System.Collections.Generic;
using System.Timers;

using CompetitionV2;

namespace TopDownGridBasedEngine
{
    /*

        LES PARTIES SONT EN STATIQUE EN BAS DE LA PAGE

    */

    public class Map
    {
        private readonly AbsCase[,] _cases;
        private int _noCase;

        private readonly Random _random;

        // Nombre d'unités contenus dans une case, utilisées par les entités
        public const int EntityPixelPerCase = 30;

        public float TileWidth;

        public int NoCase => _noCase;

        public int Width => _noCase;
        public int Height => _noCase;

        public List<AbsCase> Walls;

        private CaseVide[] _Spawner;
        private int _MobsSpawned;
        public int Difficulty;

        public int TimeLeft
        {
            get; set;
        }

        
        public event EventHandler TimerFinished;

        public Map(int size, Rectangle clientRect)
        {

            _noCase = size + (size % 2 - 1);
            _cases = new AbsCase[size, size];
            _random = new Random();
            Walls = new List<AbsCase>();
            _MobsSpawned = 0;
            Difficulty = Math.Min(7,ProgressManager.LvlDebloque+10);

            TileWidth = Math.Min((float)clientRect.Width / NoCase, (float)clientRect.Height / NoCase);

            #region oldgen
            /*
            // Remplissage aléatoire de la carte
            for (int x = 1; x < size - 1; x++)
                for (int y = 1; y < size - 1; y++)
                {
                    if (x % 2 == 0 && y % 2 == 0)
                        this[x, y] = new CaseVide(x, y, this);
                    else if (_random.Next() % 2 != 0 && false)
                        this[x, y] = new CaseWall(x, y, this);
                    else
                        this[x, y] = new CaseVide(x, y, this);
                }

            for (int x = 0; x < size - 1; x++)
            {
                this[x, 0] = new CaseSolidWall(x, 0, this);
                this[size - 1, x] = new CaseSolidWall(size - 1, x, this);
                this[size - 1 - x, size - 1] = new CaseSolidWall(size - 1 - x, size - 1, this);
                this[0, size - 1 - x] = new CaseSolidWall(0, size - 1 - x, this);
            }

            // Génération des coins

            int i = 1;
            this[1, 1] = new CaseVide(1, 1, this);
            this[1, _noCase - 2] = new CaseVide(1, _noCase - 2, this);
            this[_noCase - 2, 1] = new CaseVide(_noCase - 2, 1, this);
            this[_noCase - 2, _noCase - 2] = new CaseVide(_noCase - 2, _noCase - 2, this);
            bool tld = true, tlr = true, trd = true, trl = true, blu = true, blr = true, bru = true, brl = true;
            while (i < 4 && (tld || tlr || trd || trl || blu || blr || bru || brl))
            {
                if (tld)
                {
                    this[1, 1 + i] = new CaseVide(1, 1 + i, this);
                    if (_random.Next() % 7 == 0) tld = false;
                }
                if (tlr)
                {
                    this[1 + i, 1] = new CaseVide(1 + i, 1, this);
                    if (_random.Next() % 7 == 0) tlr = false;
                }
                if (trd)
                {
                    this[_noCase - 2, 1 +  i] = new CaseVide(_noCase - 2, 1 +  i, this);
                    if (_random.Next() % 7 == 0) trd = false;
                }
                if (trl)
                {
                    this[_noCase - 2 - i, 1] = new CaseVide(_noCase - 2 - i, 1, this);
                    if (_random.Next() % 7 == 0) trl = false;
                }
                if (blu)
                {
                    this[1, _noCase - 2 - i] = new CaseVide(1, _noCase - 2 - i, this);
                    if (_random.Next() % 7 == 0) blu = false;
                }
                if (blr)
                {
                    this[1 + i, _noCase - 2] = new CaseVide(1 + i, _noCase - 2, this);
                    if (_random.Next() % 7 == 0) blr = false;
                }
                if (bru)
                {
                    this[_noCase - 2, _noCase - 2 - i] = new CaseVide(_noCase - 2, _noCase - 2 - i, this);
                    if (_random.Next() % 7 == 0) bru = false;
                }
                if (brl)
                {
                    this[_noCase - 2 - i, _noCase - 2] = new CaseVide(_noCase - 2 - i, _noCase - 2, this);
                    if (_random.Next() % 7 == 0) brl = false;
                }
                i++;
            }*/
            #endregion

            Generate();

            Random r = new Random();
            _Spawner = new CaseVide[6] { null, null, null, null, null, null };

            do
            {
                _Spawner[0] = this[r.Next() % Width, 0] as CaseVide;
            } while (_Spawner[0] == null);
            do
            {
                int p = r.Next() % Width;
                if (p != _Spawner[0].X)
                    _Spawner[1] = this[p, 0] as CaseVide;
            } while (_Spawner[1] == null);
            do
            {
                _Spawner[2] = this[r.Next() % Width, Height - 1] as CaseVide;
            } while (_Spawner[2] == null);
            do
            {
                int p = r.Next() % Width;
                if (p != _Spawner[2].X)
                    _Spawner[3] = this[p, Height - 1] as CaseVide;
            } while (_Spawner[3] == null);
            do
            {
                _Spawner[4] = this[0, r.Next() % Height] as CaseVide;
            } while (_Spawner[4] == null);
            do
            {
                _Spawner[5] = this[0, r.Next() % Height] as CaseVide;
            } while (_Spawner[5] == null);

            Timer t = new Timer();
            t.Interval = 20000;
            t.Elapsed += T_Elapsed;
            t.Start();


        }

        public void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            int SpawnerNumber = 0;
            foreach (MobEntry me in Waves[Difficulty])
            {
                AbsEntity ent = null;

                for (int i = 0; i < me.PerWave; i++)
                {
                    switch (me.Type)
                    {
                        case EntityType.FighterRobot:
                            EntityManager.Instance.Add(
                                new FighterRobot(_Spawner[SpawnerNumber].X * Map.EntityPixelPerCase,
                                _Spawner[SpawnerNumber].Y * Map.EntityPixelPerCase, this));
                            break;
                        case EntityType.OPFighterRobot:
                            EntityManager.Instance.Add(
                                new OPFighterRobot(_Spawner[SpawnerNumber].X * Map.EntityPixelPerCase,
                                _Spawner[SpawnerNumber].Y * Map.EntityPixelPerCase, this));
                            break;
                        case EntityType.SoldierRobot:
                            EntityManager.Instance.Add(
                                new SoldierRobot(_Spawner[SpawnerNumber].X * Map.EntityPixelPerCase,
                                _Spawner[SpawnerNumber].Y * Map.EntityPixelPerCase, this));
                            break;
                        case EntityType.OPSoldierRobot:
                            EntityManager.Instance.Add(
                                new OPSoldierRobot(_Spawner[SpawnerNumber].X * Map.EntityPixelPerCase,
                                _Spawner[SpawnerNumber].Y * Map.EntityPixelPerCase, this));
                            break;
                        case EntityType.SniperRobot:
                            EntityManager.Instance.Add(
                                new SniperRobot(_Spawner[SpawnerNumber].X * Map.EntityPixelPerCase,
                                _Spawner[SpawnerNumber].Y * Map.EntityPixelPerCase, this));
                            break;
                        case EntityType.OPSniperRobot:
                            EntityManager.Instance.Add(
                                new OPSniperRobot(_Spawner[SpawnerNumber].X * Map.EntityPixelPerCase,
                                _Spawner[SpawnerNumber].Y * Map.EntityPixelPerCase, this));
                            break;
                        case EntityType.Kamikaze:
                            EntityManager.Instance.Add(
                                new KamikazeRobot(_Spawner[SpawnerNumber].X * Map.EntityPixelPerCase,
                                _Spawner[SpawnerNumber].Y * Map.EntityPixelPerCase, this));
                            break;
                        default:
                            ent = null;
                            break;
                    }
                    SpawnerNumber++;
                    if (SpawnerNumber >= _Spawner.Length)
                        SpawnerNumber = 0;
                }
                
                if (ent != null)
                    EntityManager.Instance.Add(ent);
            }

            ((Timer)sender).Interval -= 200;
            if (_MobsSpawned >= 100)
                ((Timer)sender).Stop();

        }

        private void Generate()
        {
            List<Mur> listeMur = new List<Mur>();
            int maxDistanceBetweenWalls = this.Width;
            int minDistanceBetweenWalls = this.Width / 10;
            // Note sur le balancement : Pour chaque 1/3, on a un peu plus d'un mur de 1/20 à 1/6 (voir Mur()).
            // Le surremplissage dû au random pouvant aller jusqu'à un mur aux 1/13 est moyenné par l'annulation en cas de collision. 

            int currPos = _random.Next(0, maxDistanceBetweenWalls);
            int mapLength = this.Width * this.Height;
            List<Mur> listeASupprimer = new List<Mur>();

            // Compteurs en x et en y
            byte i;
            byte j = 0;

            // Recouvrement de la map avec de la terre.
            while (j < this.Height)
            {
                i = 0;
                while (i < this.Width)
                {
                    this[j, i] = new CaseVide(j, i, this);
                    i += 1;
                }
                j += 1;
            }


            // Génération de murs pour un futur remplissage.
            do
            {
                listeMur.Add(new Mur(this, _random, currPos));
                currPos += _random.Next(minDistanceBetweenWalls, maxDistanceBetweenWalls);
            } while (currPos < mapLength);

            // Remplissage de la carte à partir des murs.
            do
            {
                // L'appel procédural des murs tile par tile évite qu'uniquement
                // les murs du bas soient "bloqués" par les murs complets du haut.

                foreach (Mur mur in listeMur)
                {
                    if (mur.Build())
                    {
                        listeASupprimer.Add(mur);
                    }
                }

                // Supression des murs achevés ou bloqués.
                foreach (Mur mur in listeASupprimer)
                {
                    listeMur.Remove(mur);
                }
                listeASupprimer.Clear();

            } while (listeMur.Count > 0);

            // Mettre ici le code pour remplacer de la terre par de l'herbe si besoin.       

             // Version unsafe
             currPos = _random.Next(mapLength / 30);
             do
             {
                 PousserHerbe(currPos);
                 currPos += _random.Next(mapLength / 4);
             } while (currPos < mapLength);
            
        }

                void PousserHerbe(int pos)
                {
                    Point Centre = new Point(pos % Width, pos / Width);
                    int Rayon = (_random.Next(0, 6) + 1) << 1;
                    int Step = 0;

                    for (int j = 0; j < Rayon; j += 1)
                    {
                        Step = Rayon - j;
                        for (int i = 0; i < Step; i += 1)
                        {
                            if (Centre.X - j >= 0 && Centre.Y - i >= 0 && this[Centre.X - j, Centre.Y - i].Type == CaseType.Vide)
                                this[Centre.X - j, Centre.Y - i] = new CaseVerte(Centre.X - j, Centre.Y - i, this);
                            if (Centre.X - j >= 0 && Centre.Y + i < Height && this[Centre.X - j, Centre.Y + i].Type == CaseType.Vide)
                                this[Centre.X - j, Centre.Y + i] = new CaseVerte(Centre.X - j, Centre.Y + i, this);
                            if (Centre.X + j < Width && Centre.Y - i >= 0 && this[Centre.X + j, Centre.Y - i].Type == CaseType.Vide)
                                this[Centre.X + j, Centre.Y - i] = new CaseVerte(Centre.X + j, Centre.Y - i, this);
                            if (Centre.X + j < Width && Centre.Y + i < Height &&  this[Centre.X + j, Centre.Y + i].Type == CaseType.Vide)
                                this[Centre.X + j, Centre.Y + i] = new CaseVerte(Centre.X + j, Centre.Y + i, this);
                        }
                    }
                }

        // Indexeur pour aller chercher facilement des cases
        public AbsCase this[int x, int y]
        {
            get
            {
                if (x >= 0 && y >= 0 && x < _noCase && y < _noCase)
                    return _cases[x, y];
                return new CaseVide(x, y, this);
            }
            set
            {
                if (x >= 0 && y >= 0 && x < _noCase && y < _noCase)
                {
                    if (this[x, y] != null)
                    {
                        if (this[x, y].IsSolid)
                            Walls.Remove(this[x, y]);
                        if (this[x, y].Hull != null)
                        {
                            Game1.Penumbra.Hulls.Remove(this[x, y].Hull);
                            if (EntityManager.Instance != null)
                            {
                                if (EntityManager.Instance.Joueur != null)
                                    foreach (Light light in EntityManager.Instance.Joueur.Lights)
                                    {
                                        light.Position += Vector2.One;
                                        light.Position -= Vector2.One;
                                    }
                            }
                        }
                        /*if (this[x, y].Fire != null)
                            this[x, y].Fire = null;*/

                        /*CaseVide vide = this[x, y] as CaseVide;
                        if (vide != null)
                            if (vide.ContainsBomb)
                                vide.Bomb = null;*/
                    }
                    
                    if (value.IsSolid)
                        Walls.Add(value);
                    _cases[x, y] = value;
                }
            }
        }
        
        
        public void Draw(SpriteBatch sb, Rectangle clientRect)
        {
            foreach (AbsCase c in _cases)
                c.Draw(sb, TileWidth);
        }

        struct MobEntry
        {
            public int Total;
            public int PerWave;
            public EntityType Type;
            public MobEntry(EntityType type, int Nb, int perwave)
            {
                Total = Nb;
                PerWave = perwave;
                Type = type;
            }
        }

        private static MobEntry[][] Waves =
        {
            new MobEntry[] {new MobEntry(EntityType.SoldierRobot, 50, 3)},
            new MobEntry[] {new MobEntry(EntityType.SoldierRobot, 50, 4)},
            new MobEntry[] {new MobEntry(EntityType.SoldierRobot, 30, 4), new MobEntry(EntityType.FighterRobot, 10, 1)},
            new MobEntry[] {new MobEntry(EntityType.SoldierRobot, 30, 3), new MobEntry(EntityType.FighterRobot, 10, 2)},
            new MobEntry[] {new MobEntry(EntityType.SoldierRobot, 30, 3), new MobEntry(EntityType.OPFighterRobot, 30, 3)},
            new MobEntry[] {new MobEntry(EntityType.SoldierRobot, 50, 3), new MobEntry(EntityType.SniperRobot, 10, 1)},
            new MobEntry[] {new MobEntry(EntityType.FighterRobot, 30, 3), new MobEntry(EntityType.OPFighterRobot, 10, 1), new MobEntry(EntityType.SniperRobot, 10, 1)},
            new MobEntry[] {new MobEntry(EntityType.OPSoldierRobot, 50, 3), new MobEntry(EntityType.OPSniperRobot, 10, 1), new MobEntry(EntityType.Kamikaze, 10, 1)}
        };

    }


}
