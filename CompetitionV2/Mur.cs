using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace TopDownGridBasedEngine
{
    /// <summary>
    /// Sert à la construction de murs lors de la génération de la map. En décentralise le code.
    /// </summary>
    public class Mur
    {
        // Variables de ressources.
        private Map _Map;
        private Random _r;

        // Variables du mur.
        private int _LeftLength;
        private int _Length;
        private int _Orientation;
        private Point _CurrPos;

        public Mur(Map map, Random r, int position)
        {
            _Map = map;
            _r = r;
            _CurrPos.X = position % map.Width;
            _CurrPos.Y = position / map.Width;

            // Balance la longueur des murs en fonction de la largeur de l'écran et du random.
            _LeftLength = r.Next(map.Width / 20, map.Width / 6);
            _Length = _LeftLength;

            _Orientation = (3 * r.Next(0, 4)); // Clockwise;
        }

        /// <summary>
        /// Pose une tile sur le mur, renvoit vrai si la construction du mur est finie.
        /// </summary>
        /// <returns></returns>
        public bool Build()
        {
            AbsCase tile;
            bool over;

            if (_Orientation == 0) // Haut
            {
                over = UpperStart();
            }
            else if (_Orientation == 3) // Droite
            {
                over = RightStart();
            }
            else if (_Orientation == 6) // Bas
            {
                over = DownStart();
            }
            else // Gauche
            {
                over = LeftStart();
            }


            if (!over)
            {
                // Trois dixièmes des murs sont destructibles.
                if (_r.Next(10) < 3)
                    tile = new CaseWall(_CurrPos.X, _CurrPos.Y, _Map);
                else
                    tile = new CaseSolidWall(_CurrPos.X, _CurrPos.Y, _Map);

                _LeftLength -= 1;
                _Map[_CurrPos.X, _CurrPos.Y] = tile;
				
                if (_LeftLength <= 0)
                    over = true;
            }
            return over;
        }

        /// <summary>
        /// Verify the possibility of building a wall up there.
        /// Returns false and set current position if the wall can be correctly built.
        /// Returns true if no wall could ever be built here.
        /// </summary>
        /// <param name="otherOrientationsWereAlreadyChecked"></param>
        /// <returns></returns>
        bool CheckUpperWallBuilding(bool otherOrientationsWereAlreadyChecked)
        {
            bool retour = false;
            bool nextPosIsInvalid = false; // Basically another return but with more explicit name.


 /*           if (!otherOrientationsWereAlreadyChecked && _r.Next(_Length) < 2) // Environ deux angles par mur si possible.
            {
                nextPosIsInvalid = true;

                if (_r.Next() % 2 == 1)
                    retour = LeftStart();
                else
                    retour = RightStart();
            }
*/
            // Section dégueulasse, mais simple.
            if (!nextPosIsInvalid)
            {
                if (_CurrPos.Y < 2)
                {
                    nextPosIsInvalid = true;
                }
                else
                {
                    if (_CurrPos.X != 0)
                    {
                        if (_Map[_CurrPos.X - 1, _CurrPos.Y - 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X - 1, _CurrPos.Y - 2].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.X != _Map.Width - 1)
                    {
                        if (_Map[_CurrPos.X + 1, _CurrPos.Y - 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X + 1, _CurrPos.Y - 2].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.Y > 1 && _Map[_CurrPos.X, _CurrPos.Y].Type != CaseType.Vide)
                        nextPosIsInvalid = true;
                }
            }
            // Tentative d'autres directions
            if (nextPosIsInvalid)
                retour = true;
            else
            {
                _CurrPos = new Point(_CurrPos.X, _CurrPos.Y - 1);
                _Orientation = 0;
            }

            return retour;
        }

        /// <summary>
        /// Verify the possibility of building a wall right there.
        /// Returns false and set current position if the wall can be correctly built.
        /// Returns true if no wall could ever be built here.
        /// </summary>
        /// <param name="otherOrientationsWereAlreadyChecked"></param>
        /// <returns></returns>
        bool CheckRightWallBuilding(bool otherOrientationsWereAlreadyChecked)
        {
            bool retour = false;
            bool nextPosIsInvalid = false; // Basically another return but with more explicit name.

            /*if (!otherOrientationsWereAlreadyChecked && _r.Next(_Length) < 2) // Environ deux angles par mur si possible.
            {
                nextPosIsInvalid = true;

                if (_r.Next() % 2 == 1)
                    retour = UpperStart();
                else
                    retour = DownStart();
            }*/

            // Section dégueulasse, mais simple.
            if (!nextPosIsInvalid)
            {
                if (_CurrPos.X >= _Map.Width - 2)
                {
                    nextPosIsInvalid = true;
                }
                else
                {
                    if (_CurrPos.Y != 0)
                    {
                        if (_Map[_CurrPos.X + 1, _CurrPos.Y - 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X + 2, _CurrPos.Y - 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.Y != _Map.Height - 1)
                    {
                        if (_Map[_CurrPos.X + 1, _CurrPos.Y + 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X + 2, _CurrPos.Y + 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.X < _Map.Width - 2 && _Map[_CurrPos.X, _CurrPos.Y].Type != CaseType.Vide)
                        nextPosIsInvalid = true;
                }
            }
            // Tentative d'autres directions
            if (nextPosIsInvalid)
                retour = true;
            else
            {
                _CurrPos = new Point(_CurrPos.X + 1, _CurrPos.Y);
                _Orientation = 3;
            }

            return retour;
        }

        /// <summary>
        /// Verify the possibility of building a wall down there.
        /// Returns false and set current position if the wall can be correctly built.
        /// Returns true if no wall could ever be built here.
        /// </summary>
        /// <param name="otherOrientationsWereAlreadyChecked"></param>
        /// <returns></returns>
        bool CheckDownWallBuilding(bool otherOrientationsWereAlreadyChecked)
        {
            bool retour = false;
            bool nextPosIsInvalid = false; // Basically another return but with more explicit name.


/*            if (!otherOrientationsWereAlreadyChecked && _r.Next(_Length) < 2) // Environ deux angles par mur si possible.
            {
                nextPosIsInvalid = true;

                if (_r.Next() % 2 == 0)
                    retour = LeftStart();
                else
                    retour = RightStart();
            }
*/
            // Section dégueulasse, mais simple.
            if (!nextPosIsInvalid)
            {
                if (_CurrPos.Y >= _Map.Height - 2)
                {
                    nextPosIsInvalid = true;
                }
                else
                {
                    if (_CurrPos.Y > 1 && _Map[_CurrPos.X, _CurrPos.Y].Type != CaseType.Vide)
                        nextPosIsInvalid = true;
                    else if (_CurrPos.X != 0)
                    {
                        if (_Map[_CurrPos.X - 1, _CurrPos.Y + 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X - 1, _CurrPos.Y + 2].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.X != _Map.Width - 1)
                    {
                        if (_Map[_CurrPos.X + 1, _CurrPos.Y - 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X + 1, _CurrPos.Y + 2].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }

                }
            }
            // Tentative d'autres directions
            if (nextPosIsInvalid)
                retour = true;
            else
            {
                _CurrPos = new Point(_CurrPos.X, _CurrPos.Y + 1);
                _Orientation = 6;
            }

            return retour;
        }


        /// <summary>
        /// Verify if a wall can be left there.
        /// Returns false and set current position if the wall can be correctly built.
        /// Returns true if no wall could ever be built here.
        /// </summary>
        /// <param name="otherOrientationsWereAlreadyChecked"></param>
        /// <returns></returns>
        bool CheckLeftWallBuilding(bool otherOrientationsWereAlreadyChecked)
        {
            bool retour = false;
            bool nextPosIsInvalid = false; // Basically another return but with more explicit name.

/*            if (!otherOrientationsWereAlreadyChecked && _r.Next(_Length) < 2) // Environ deux angles par mur si possible.
            {
                nextPosIsInvalid = true;

                if (_r.Next() % 2 == 0)
                    retour = UpperStart();
                else
                    retour = DownStart();
            }
*/
            // Section dégueulasse, mais simple.
            if (!nextPosIsInvalid)
            {
                if (_CurrPos.X <  2)
                {
                    nextPosIsInvalid = true;
                }
                else
                {
                    if (_CurrPos.Y != 0)
                    {
                        if (_Map[_CurrPos.X - 1, _CurrPos.Y - 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X - 2, _CurrPos.Y - 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.Y != _Map.Height - 1)
                    {
                        if (_Map[_CurrPos.X - 1, _CurrPos.Y + 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                        else if (_Map[_CurrPos.X - 2, _CurrPos.Y + 1].Type != CaseType.Vide)
                            nextPosIsInvalid = true;
                    }
                    else if (_CurrPos.X < _Map.Width - 2 && _Map[_CurrPos.X, _CurrPos.Y].Type != CaseType.Vide)
                        nextPosIsInvalid = true;
                }
            }
            // Tentative d'autres directions
            if (nextPosIsInvalid)
                retour = true;
            else
            {
                _CurrPos = new Point(_CurrPos.X - 1, _CurrPos.Y);
                _Orientation = 9;
            }

            return retour;
        }


        bool UpperStart()
        {
            bool over = false;

            if (CheckUpperWallBuilding(false) &&
                CheckRightWallBuilding(true) &&
                CheckLeftWallBuilding(true))
                over = true;

            return over;
        }

        bool RightStart()
        {
            bool over = false;

            if (CheckRightWallBuilding(false) &&
                CheckDownWallBuilding(true) &&
                CheckUpperWallBuilding(true))
                over = true;

            return over;
        }

        bool DownStart()
        {
            bool over = false;

            if (CheckDownWallBuilding(false) &&
                CheckLeftWallBuilding(true) &&
                CheckRightWallBuilding(true))
                over = true;

            return over;
        }

        bool LeftStart()
        {
            bool over = false;

            if (CheckLeftWallBuilding(false) &&
                CheckUpperWallBuilding(true) &&
                CheckDownWallBuilding(true))
                over = true;

            return over;
        }
    }
}
