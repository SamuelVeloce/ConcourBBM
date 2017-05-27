using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;

using System.Collections.Generic;

namespace TopDownGridBasedEngine
{
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

        public Map(int size, Rectangle clientRect)
        {
            
            _noCase = size + (size % 2 - 1);
            _cases = new AbsCase[size, size];
            _random = new Random();
            Walls = new List<AbsCase>();
            
            TileWidth = Math.Min((float)clientRect.Width / NoCase, (float)clientRect.Height / NoCase);

            #region oldgen
            /*
            // Remplissage aléatoire de la carte
            for (int x = 1; x < size - 1; x++)
                for (int y = 1; y < size - 1; y++)
                {
                    if (x % 2 == 0 && y % 2 == 0)
                        this[x, y] = new CaseVide(x, y, this);
                    else if (_random.Next() % 2 != 0)
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
        }

        // Indexeur pour aller chercher facilement des cases
        public AbsCase this[int x, int y]
        {
            get
            {
                if (x >= 0 && y >= 0 && x < _noCase && y < _noCase)
                    return _cases[x, y];
                throw new IndexOutOfRangeException($"{x}, {y} is outside the range 0-{_noCase - 1}");
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
                            if (Game1.Joueurs != null)
                                foreach (Light light in Game1.Joueurs[0].Lights)
                                {
                                    light.Position += Vector2.One;
                                    light.Position -= Vector2.One;
                                }
                        }
                        if (this[x, y].Fire != null)
                            this[x, y].Fire = null;

                        CaseVide vide = this[x, y] as CaseVide;
                        if (vide != null)
                            if (vide.ContainsBomb)
                                vide.Bomb = null;
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

        /// <summary>
        /// Tries to place a bonus at the specified spot in the map
        /// </summary>
        /// <param name="x">X coords of the case to place a bonus in</param>
        /// <param name="y">Y coords of the case to place a bonus in</param>
        /// <returns>True if a bonus was place, false otherwise</returns>
        public bool MakeRandomBonus(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _noCase || y >= _noCase)
                return false;
            if (!(this[x, y] is CaseWall))
                return false;

            if (_random.Next() % 3 != 0)
                return false;

            this[x, y] = new CaseBonus(x, y, this, _random);
            return true;

        }

        // Size, byte[Size, Size]
        public bool FromByteArray(byte[] data, ref int position)
        {
            if (data.Length < position + 1)
                return false;
            _noCase = data[position++];
            if (data.Length < position + 1 + _noCase * _noCase)
                return false;
            for (int x = 0; x < _noCase; x++)
            {
                for (int y = 0; y < _noCase; y++)
                {
                    if (data[position] == 2)
                        this[x, y] = new CaseSolidWall(x, y, this);
                    else if (data[position] == 1)
                        this[x, y] = new CaseWall(x, y, this);
                    else
                        this[x, y] = new CaseVide(x, y, this);
                    position++;
                }
            }
            return true;
        }

        public byte[] ToByteArray()
        {
            byte[] ret = new byte[_noCase * _noCase + 1];
            ret[0] = (byte)_noCase;
            for (int x = 0; x < _noCase; x++)
            {
                for (int y = 0; y < _noCase; y++)
                {
                    ret[1 + x * _noCase + y] = (byte)this[x, y].Type;
                }
            }
            return ret;
        }

    }
}
