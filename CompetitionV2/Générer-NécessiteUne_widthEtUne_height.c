        private void Generate()
        {
            List<Mur> listeMur = new List<Mur>();
            int maxDistanceBetweenWalls = this._width / 4;
            int minDistanceBetweenWalls = this._width / 20;
            // Note sur le balancement : Pour chaque 1/4, on a un peu plus d'un mur de 1/20 à 1/6 (voir Mur()).
            // Le surremplissage dû au random pouvant aller jusqu'à un mur aux 1/20 est moyenné par l'annulation en cas de collision. 

            int currPos = _random.Next(0, maxDistanceBetweenWalls);
            int mapLength = this._width * this._height;
            List<Mur> listeASupprimer = new List<Mur>();

            // Compteurs en x et en y
            byte i;
            byte j = 0;

            // Recouvrement de la map avec de la terre.
            while (j < this._height)
            {
                i = 0;
                while (i < this._width)
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
    }