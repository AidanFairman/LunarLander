using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameLibrary
{
    public static class VectorFont
    {

        private static PrimitiveBatch pb = null;
        private static FontStorage[] alphabet = null;
        private static Game game = null;

        public static void Initialize(Game inGame)
        {
            if (game == null)
            {
                game = inGame;
            }
            if (pb == null)
            {
                pb = new PrimitiveBatch(game.GraphicsDevice);
            }
            if (alphabet == null)
            {
                //Load the letters in from their respective files
                #region Letter Loading
                LinkedList<FontStorage> alpha = new LinkedList<FontStorage>();
                List<object[]> Letters = LanderTextToVector.GetLetters(string.Format("{0}\\{1}.txt", game.Content.RootDirectory, "alphabet"));

                foreach (object[] obj in Letters)
                {
                    alpha.AddLast(new FontStorage((char)obj[0], (List<Vector2>)obj[1]));
                }
                #endregion
                alphabet = new FontStorage[alpha.Count];
                alpha.CopyTo(alphabet, 0);
            }
            
        }

        public static void DrawString(string str, float scale, Vector2 position, Color color)
        {

            List<Vector2> stringVector = GetScaledString(str, scale);
            pb.Begin(PrimitiveType.LineList);
            foreach (Vector2 v2 in stringVector)
            {
                pb.AddVertex(v2 + position, color);
            }
            pb.End();
        }

        /// <summary>
        /// Turns a word into vector text
        /// </summary>
        /// <param name="word">string to be turned into vector text</param>
        /// <param name="scale">Scale of the text</param>
        /// <returns>List of Vector2, representing the vertices of the vector text</returns>
        private static List<Vector2> GetScaledString(string word, float scale)
        {
            List<Vector2> toReturn = new List<Vector2>();
            List<Vector2> letter;
            Vector2 position = new Vector2(0, 0);
            foreach (char c in word)
            {
                letter = GetScaledLetter(c, scale);
                foreach (Vector2 v2 in letter)
                {
                    toReturn.Add(v2 + position);
                }
                position.X += (3 * scale);
            }
            return toReturn;
        }

        /// <summary>
        /// Turns the specified letter into vector2 vertices at the specified scale
        /// </summary>
        /// <param name="letter">letter to vectorize</param>
        /// <param name="scale">scale to use</param>
        /// <returns>List of Vector2, representing the vector of the letter provided</returns>
        private static List<Vector2> GetScaledLetter(char letter, float scale)
        {
            List<Vector2> letterList = LetterGetter(letter);
            List<Vector2> toReturn = new List<Vector2>();
            foreach(Vector2 v2 in letterList)
            {
                toReturn.Add(new Vector2(v2.X * scale, v2.Y * scale));
            }
            return toReturn;
        }

       

        /// <summary>
        /// gets the needed letter from storage as a 2d array
        /// </summary>
        /// <param name="letter">letter to retrieve from storage</param>
        /// <returns>2 dimensional integer array representing the x and y of vector points to make the specified letter</returns>
        private static List<Vector2> LetterGetter(char letter)
        {
            letter = Char.ToUpper(letter);
            List<Vector2> toReturn = null;
            foreach (FontStorage F in alphabet)
            {
                if (F.Character == letter)
                {
                    toReturn = F.data;
                }
            }

            return toReturn;
        }

        /// <summary>
        /// private class to act as the standard storage for the vector font
        /// </summary>
        private class FontStorage
        {
            public List<Vector2> data { get; private set; }
            public char Character { get; set; }

            public FontStorage(char ch, List<Vector2> points)
            {
                data = points;
                Character = ch;
            }
        }

        /// <summary>
        /// Stores the vector font in an organized manner
        /// </summary>
        /*private FontStorage[] alphabet = new FontStorage[] {
            new FontStorage('A', new int[,] { { 0, 2 }, { 0, 1 }, { 0, 1 }, { 1, 0 }, { 1, 0 }, { 2, 1 }, { 2, 1 }, { 2, 2 }, { 0, 1 }, { 2, 1 } }),
            new FontStorage('B', new int[,] { { 0, 0 }, { 2, 0 }, { 2, 0 }, { 1, 1 }, { 1, 1 }, { 2, 2 }, { 2, 2 }, { 0, 2 }, { 0, 2 }, { 0, 0 }, { 0, 1 }, { 1, 1 } }),
            new FontStorage('C', new int[,] { { 2, 2 }, { 0, 2 }, { 0, 2 }, { 0, 0 }, { 0, 0 }, { 2, 0 } }),
            new FontStorage('D', new int[,] { { 0, 2 }, { 0, 0 }, { 0, 0 }, { 1, 0 }, { 1, 0 }, { 2, 1 }, { 2, 1 }, { 2, 2 }, { 2, 2 }, { 0, 2 } }),
            new FontStorage('E', new int[,] { { 2, 2 }, { 0, 2 }, { 0, 2 }, { 0, 0 }, { 0, 0 }, { 2, 0 }, { 0, 1 }, { 2, 1 } }),
            new FontStorage('F', new int[,] { { 0, 2 }, { 0, 0 }, { 0, 0 }, { 2, 0 }, { 0, 1 }, { 1, 1 } }),
            new FontStorage('G', new int[,] { { 2, 2 }, { 0, 2 }, { 0, 2 }, { 0, 0 }, { 0, 0 }, { 2, 0 }, { 2, 2 }, { 2, 1 }, { 2, 1 }, { 1, 1 } }),
            new FontStorage('H', new int[,] { { 0, 2 }, { 0, 0 }, { 2, 0 }, { 2, 2 }, { 0, 1 }, { 2, 1 } }),
            new FontStorage('I', new int[,] { { 0, 2 }, { 2, 2 }, { 0, 0 }, { 2, 0 }, { 1, 0 }, { 1, 2 } }),
            new FontStorage('J', new int[,] { { 0, 0 }, { 2, 0 }, { 1, 0 }, { 1, 2 }, { 1, 2 }, { 0, 2 } }),
            new FontStorage('K', new int[,] { { 0, 2 }, { 0, 0 }, { 0, 1 }, { 2, 0 }, { 0, 1 }, { 2, 2 } }),
            new FontStorage('L', new int[,] { { 0, 0 }, { 0, 2 }, { 0, 2 }, { 2, 2 } }),
            new FontStorage('M', new int[,] { { 0, 2 }, { 0, 0 }, { 0, 0 }, { 1, 1 }, { 1, 1 }, { 2, 0 }, { 2, 0 }, { 2, 2 } }),
            new FontStorage('N', new int[,] { { 0, 2 }, { 0, 0 }, { 0, 0 }, { 2, 2 }, { 2, 2 }, { 2, 0 } }),
            new FontStorage('O', new int[,] { { 2, 2 }, { 0, 2 }, { 0, 2 }, { 0, 0 }, { 0, 0 }, { 2, 0 }, { 2, 0 }, { 2, 2 } }),
            new FontStorage('P', new int[,] { { 0, 2 }, { 0, 0 }, { 0, 0 }, { 2, 0 }, { 2, 0 }, { 2, 1 }, { 2, 1 }, { 0, 1 } }),
            new FontStorage('Q', new int[,] { { 2, 2 }, { 0, 2 }, { 0, 2 }, { 0, 0 }, { 0, 0 }, { 2, 0 }, { 2, 0 }, { 2, 2 }, { 1, 1 }, { 2, 2 } }),
            new FontStorage('R', new int[,] { { 0, 2 }, { 0, 0 }, { 0, 0 }, { 2, 0 }, { 2, 0 }, { 2, 1 }, { 2, 1 }, { 0, 1 }, { 0, 1 }, { 2, 2 } }),
            new FontStorage('S', new int[,] { { 2, 0 }, { 0, 0 }, { 0, 0 }, { 0, 1 }, { 0, 1 }, { 2, 1 }, { 2, 1 }, { 2, 2 }, { 2, 2 }, { 0, 2 } }),
            new FontStorage('T', new int[,] { { 0, 0 }, { 2, 0 }, { 1, 0 }, { 1, 2 } }),
            new FontStorage('U', new int[,] { { 0, 0 }, { 0, 2 }, { 0, 2 }, { 2, 2 }, { 2, 2 }, { 2, 0 } }),
            new FontStorage('V', new int[,] { { 0, 0 }, { 1, 2 }, { 1, 2 }, { 2, 0 } }),
            new FontStorage('W', new int[,] { { 0, 0 }, { 0, 2 }, { 0, 2 }, { 1, 1 }, { 1, 1 }, { 2, 2 }, { 2, 2 }, { 2, 0 } }),
            new FontStorage('X', new int[,] { { 0, 0 }, { 2, 2 }, { 2, 0 }, { 0, 2 } }),
            new FontStorage('Y', new int[,] { { 0, 0 }, { 1, 1 }, { 2, 0 }, { 1, 1 }, { 1, 1 }, { 1, 2 } }),
            new FontStorage('Z', new int[,] { { 0, 0 }, { 2, 0 }, { 2, 0 }, { 0, 2 }, { 0, 2 }, { 2, 2 } }),
            new FontStorage('1', new int[,] { { 0, 0 }, { 1, 0 }, { 1, 0 }, { 1, 2 }, { 0, 2 }, { 2, 2 } }),
            new FontStorage('2', new int[,] { { 0, 0 }, { 2, 0 }, { 2, 0 }, { 2, 1 }, { 2, 1 }, { 0, 2 }, { 0, 2 }, { 2, 2 } }),
            new FontStorage('3', new int[,] { { 0, 0 }, { 2, 0 }, { 2, 0 }, { 1, 1 }, { 1, 1 }, { 2, 2 }, { 2, 2 }, { 0, 2 } }),
            new FontStorage('4', new int[,] { { 0, 0 }, { 0, 1 }, { 0, 1 }, { 2, 1 }, { 2, 0 }, { 2, 2 } }),
            new FontStorage('5', new int[,] { { 2, 0 }, { 0, 0 }, { 0, 0 }, { 0, 1 }, { 0, 1 }, { 2, 1 }, { 2, 1 }, { 0, 2 } }),
            new FontStorage('6', new int[,] { { 2, 0 }, { 0, 0 }, { 0, 0 }, { 0, 2 }, { 0, 2 }, { 2, 2 }, { 2, 2 }, { 2, 1 }, { 2, 1 }, { 0, 1 } }),
            new FontStorage('7', new int[,] { { 0, 0 }, { 2, 0 }, { 2, 0 }, { 1, 2 }, { 1, 1 }, { 2, 1 } }),
            new FontStorage('8', new int[,] { { 2, 2 }, { 0, 2 }, { 0, 2 }, { 0, 0 }, { 0, 0 }, { 2, 0 }, { 2, 0 }, { 2, 2 }, { 0, 1 }, { 2, 1 } }),
            new FontStorage('9', new int[,] { { 2, 2 }, { 2, 0 }, { 2, 0 }, { 0, 0 }, { 0, 0 }, { 0, 1 }, { 0, 1 }, { 2, 1 } }),
            new FontStorage('0', new int[,] { { 2, 2 }, { 0, 2 }, { 0, 2 }, { 0, 0 }, { 0, 0 }, { 2, 0 }, { 2, 0 }, { 2, 2 }, { 0, 0 }, { 2, 2 } }),
            new FontStorage('.', new int[,] { { 1, 1 }, { 2, 2 }, { 2, 1 }, { 1, 2 } }),
            new FontStorage('?', new int[,] { { 0, 0 }, { 2, 0 }, { 2, 0 }, { 2, 1 }, { 2, 1 }, { 1, 1 }, { 1, 1 }, { 1, 2 } }),
            new FontStorage(' ', new int[,] { { 0, 0 }, { 0, 0 } })
        };*/

        
    }
}
