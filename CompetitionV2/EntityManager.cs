using System;
using System.Collections.Generic;
using System.Linq;
using TopDownGridBasedEngine.Projectile;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CompetitionV2;

namespace TopDownGridBasedEngine
{
    public class EntityManager
    {
        // Singleton! :D
        private List<AbsEntity> _entities;

        private object someLock;
        private int _id;
        private List<Fire> _deadFire;
        private List<absProjectile> m_projectilesListFriendly;
        private List<absProjectile> m_projectilesListHostile;
        private List<Bonus> m_listBonus;



        private static EntityManager _instance;

        public event OnFireStopHandler FireStopped;

        private EntityManager() : this(null, null, 0) { }
        
        private EntityManager(Joueur j, Map m, int numeroJoueur)
        {
            _entities = new List<AbsEntity>();
            Joueur = j;
            Map = m;
            _id = numeroJoueur;
            someLock = new object();
            _deadFire = new List<Fire>();
            m_projectilesListFriendly = new List<absProjectile>();
            m_projectilesListHostile = new List<absProjectile>();
            m_listBonus = new List<Bonus>();
        }

        protected void FireFireStopped(object sender, MultiFireEventArgs e)
        {
            FireStopped?.Invoke(sender, e);
        }

        public static EntityManager Instance
        {
            get
            {
                if (_instance == null)
                    return null;// throw new Exception("Instance inexistante. Utilisez InitInstance avant d'utiliser l'instance");
                return _instance;
            }
        }

        public static void InitInstance(Joueur j, Map m, int ID)
        {
            if (_instance != null)
            {
                _instance._entities = new List<AbsEntity>();
                
                _instance.Joueur = j;
                _instance.Map = m;
                _instance._id = ID;
                _instance.ProjectilesListFriendly = new List<absProjectile>();
            }
            _instance = new EntityManager(j, m, ID);
        }

        public List<AbsEntity> Entities => _entities;

        public List<Bonus> Bonus => m_listBonus;

        public Joueur Joueur { get; private set; }

        public Map Map { get; set; }

        public List<absProjectile> ProjectilesListFriendly
        {
            get { return m_projectilesListFriendly; }
            set { m_projectilesListFriendly = value; }
        }

        public List<absProjectile> ProjectilesListHostile
        {
            get { return m_projectilesListHostile; }
            set { m_projectilesListHostile = value; }
        }

        public void Add(AbsEntity e)
        {
            lock (someLock)
            {
                _entities.Add(e);
            }
        }

        public bool Remove(AbsEntity e)
        {
            Fire fire = e as Fire;
            if (fire != null)
            {
                _deadFire.Add(fire);
            }
            
            lock (someLock)
            {
                return _entities.Remove(e);
            }
        }

        public void TickPlayer(int idPlayer, GameTime gameTime, KeyWrapper wrapper)
        {
            if (!Joueur.IsDead)
                Joueur.TickPlayer((int)gameTime.ElapsedGameTime.TotalMilliseconds, wrapper);
        }

        public void TickEntities(GameTime gameTime)
        {
            lock (someLock)
            {
                List<AbsEntity> toUpdate = _entities.ToList();

                foreach (AbsEntity e in toUpdate)
                {
                    e.Tick((int)gameTime.ElapsedGameTime.TotalMilliseconds);
                    if (e is ITexturable)
                        ((ITexturable) e).UpdateTexture((int)gameTime.ElapsedGameTime.TotalMilliseconds);
                }
                if (Joueur != null && Joueur.IsDead == false)
                    Joueur.UpdateTexture((int)gameTime.ElapsedGameTime.TotalMilliseconds); //Tick(DeltaTime);
                if (Joueur.IsDead == false)
                    Joueur.Tick((long)gameTime.ElapsedGameTime.TotalMilliseconds);
                if (_deadFire.Count != 0)
                {
                    FireFireStopped(this, new MultiFireEventArgs(_deadFire.ToArray(), false));
                    _deadFire = new List<Fire>();
                }
                List<absProjectile> projli = EntityManager.Instance.ProjectilesListFriendly;
                List<absProjectile> projho = EntityManager.Instance.ProjectilesListHostile;

                lock (someLock)
                {
                    for (int i = projli.Count - 1; i >= 0; i--)
                    {
                        if (projli[i].Update(gameTime))
                        {
                            projli.RemoveAt(i);
                        }
                    }
                    for (int i = projho.Count - 1; i >= 0; i--)
                    {
                        if (projho[i].Update(gameTime))
                        {
                            projho.RemoveAt(i);
                        }
                    }
                    for (int i = this.Bonus.Count - 1; i >= 0; i--)
                    {
                        this.Bonus[i].Tick((long)gameTime.ElapsedGameTime.TotalMilliseconds);
                    }
                }
            }
        }




        public void Draw(SpriteBatch sb, Rectangle clientRect)
        {
            float w = Map.TileWidth;
            
            List<AbsEntity> toDraw = null;
            
            lock (someLock)
            {
                toDraw = _entities.ToList();

                foreach (absProjectile proj in ProjectilesListFriendly)
                    proj.Draw(sb, w);

                foreach (absProjectile proj in ProjectilesListHostile)
                    proj.Draw(sb, w);

                foreach (AbsEntity e in toDraw)
                    e.Draw(sb, w / 30);

                foreach (Bonus b in Bonus)
                    b.Draw(sb, w);
            }

            
        }

        public void DrawPlayers(SpriteBatch sb, Rectangle clientRect)
        {
            float w = Map.TileWidth;
            if (Joueur != null && Joueur.IsDead == false)
                Joueur.Draw(sb, w / 30);
        }

    }
}
