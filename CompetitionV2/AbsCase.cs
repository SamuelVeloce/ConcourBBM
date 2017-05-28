using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;

namespace CompetitionV2
{

    public enum CaseType
    {
        Vide = 0,
        Wall,
        SolidWall,
        Bonus
    };

    public abstract class AbsCase
    {
        protected AbsCase(int x, int y, Map m, bool solid, bool breakable, bool letsFireThrough)
        {
            X = x;
            Y = y;
            Map = m;
            IsSolid = solid;
            IsBreakable = breakable;
            //LetsFireThrough = letsFireThrough;
            color = Color.White;

            if (solid)
            {
                Hull = Hull.CreateRectangle(new Vector2(x * Map.TileWidth + Map.TileWidth / 2, y * Map.TileWidth + Map.TileWidth / 2), new Vector2(Map.TileWidth, Map.TileWidth));
                
                Game1.Penumbra.Hulls.Add(Hull);
            }

            Hitbox = new Rectangle(new Point(x * Map.EntityPixelPerCase, y * Map.EntityPixelPerCase), new Point(Map.EntityPixelPerCase, Map.EntityPixelPerCase));
        }

        public Color color { get; set; }

        /// <summary>
        /// Utile pour les raycasts. Déterminé selon Map.EntityPixelPerCase
        /// </summary>
        public Rectangle Hitbox { get; }
        
        /// <summary>
        /// Ce qui crée les ombres. Déterminé selon Map.TileWidth
        /// </summary>
        public Hull Hull { get; }

        /// <summary>
        /// Indice vertical dans le tableau de la carte
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Indice vertical dans le tableau de la carte
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// La carte qui contient cette case
        /// </summary>
        public Map Map { get; set; }

        /// <summary>
        /// Indique si la case est destructible ou non
        /// </summary>
        public bool IsBreakable { get; set; }

        /// <summary>
        /// Indique si la case est en processus de destruction
        /// </summary>
        public bool IsBreaking
        {
            get { /*return Fire != null;*/ return false; }
            set { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// Référence vers le feu contenu dans cette case, ou null
        /// </summary>
        //public Fire Fire { get; set; }

        /// <summary>
        /// Indique si le feu peut traverser librement cette case
        /// </summary>
        //public bool LetsFireThrough { get; set; }

        /// <summary>
        /// Indique si le joueur peut traverser librement cette case
        /// </summary>
        public virtual bool IsSolid { get; set; }

        public abstract Texture2D Texture { get; }

        public abstract CaseType Type { get; }

        public virtual void Draw(SpriteBatch sb, float width)
        {
            sb.Draw(Texture, new Rectangle((int)(X * width), (int)(Y * width), (int)width + 1, (int)width + 1), color);
        }

        public override string ToString()
        {
            return $"{Type} at {X}, {Y}";
        }
    }
}
