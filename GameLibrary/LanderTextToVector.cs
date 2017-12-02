using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameLibrary
{
    public static class LanderTextToVector
    {
        public static List<Vector2> GetXMirrorPoints(string fileName)
        {
            List<Vector2> list = GetPoints(fileName);
            List<Vector2> toReturn = new List<Vector2>();
            int maximum = 0;
            foreach (Vector2 v2 in list)
            {
                if (v2.X > maximum)
                {
                    maximum = (int)v2.X;
                }
            }

            foreach (Vector2 v2 in list)
            {
                toReturn.Add(new Vector2(((0 - v2.X) + maximum), v2.Y));
            }
            return toReturn;
        }

        public static List<Vector2> GetPoints(string fileName)
        {
            StreamReader inFile = new StreamReader(fileName);
            List<Vector2> pointList = new List<Vector2>();

            String line = inFile.ReadLine();
            String[] parts = line.Split(',');
            if (parts.Length > 4)
            {
                for (int i = 0; i < (parts.Length - 2); i += 3)
                {
                    if (parts[i + 2] == "->")
                    {
                        pointList.Add(new Vector2(int.Parse(parts[i]), int.Parse(parts[i + 1])));
                        pointList.Add(new Vector2(int.Parse(parts[(i + 3)]), int.Parse(parts[i + 4])));
                    } else //if (parts[i+2] == "||")
                    {

                    }
                }
            }
            inFile.Close();
            //inFile.Dispose();
            return pointList;
        }

        public static List<object[]> GetLetters(string fileName)
        {
            List<object[]> alphabetList = new List<object[]>();
            object[] container;
            List<Vector2> pointList;

            StreamReader inFile = new StreamReader(fileName);
            String line = inFile.ReadLine();

            while (line != null)
            {
                container = new object[2];
                pointList = new List<Vector2>();
                String[] parts = line.Split(',');
                if (parts.Length > 5)
                {
                    container[0] = parts[0][0];
                    for (int i = 1; i < (parts.Length - 2); i += 3)
                    {
                        if (parts[i + 2] == "->")
                        {
                            pointList.Add(new Vector2(int.Parse(parts[i]), int.Parse(parts[i + 1])));
                            pointList.Add(new Vector2(int.Parse(parts[(i + 3)]), int.Parse(parts[i + 4])));
                        }
                        else //if (parts[i+2] == "||")
                        {

                        }
                    }
                    container[1] = pointList;
                    alphabetList.Add(container);
                }
                line = inFile.ReadLine();
            }
            inFile.Close();
            return alphabetList;
        }
    }
}
