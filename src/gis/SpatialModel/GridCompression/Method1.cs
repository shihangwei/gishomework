using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SpatialModel.GridCompression
{
    /// <summary>
    /// 压缩编码-链式编码
    /// </summary>
    class Method1 : ICompress
    {
        Color tColor = Color.Empty;
        StringBuilder tSb = new StringBuilder();
        Dictionary<Color, List<string>> dic = new Dictionary<Color, List<string>>();
        List<XY> addedXY = new List<XY>();
        Color[,] tColors;
        public void DoCompress(Color[,] colors)
        {
            tColors = colors;

            ConsoleWrite();

            int x = 0;
            int y = 0;

            while (x < colors.GetLength(0) && y < colors.GetLength(1))
            {
                for (y = 0; y < colors.GetLength(1); y++)
                {

                    for (x = 0; x < colors.GetLength(0); x++)
                    {
                        if (colors[x, y] != Color.Empty &&
                            !(colors[x, y].R == Color.White.R && colors[x, y].G == Color.White.G && colors[x, y].B == Color.White.B) &&
                            !addedXY.Any(t => t.Equals(x, y)))
                        {
                            tColor = colors[x, y];
                            if (!dic.ContainsKey(tColor))
                                dic.Add(tColor, new List<string>());

                            tSb.Append(y);
                            tSb.Append(",");
                            tSb.Append(x);
                            tSb.Append(",");

                            XY next = new XY { X = x, Y = y };
                            addedXY.Add(next.Clone());

                            while (next != null)
                            {
                                next = GetNext(tColor, x, y, tSb);
                                if (next != null) addedXY.Add(next.Clone());
                            }

                            var s = tSb.ToString();
                            if (s.EndsWith(","))
                                s = s.Substring(0, s.Length - 1);

                            dic[tColor].Add(s);
                            tSb.Clear();
                        }
                    }
                }

            }

            SaveFile();
            Clear();

            Console.WriteLine("finished.");
        }

        private void ConsoleWrite()
        {

            for (int y = 0; y < tColors.GetLength(1); y++)
            {
                List<Color> rowColor = new List<Color>(tColors.GetLength(0));

                for (int x = 0; x < tColors.GetLength(0); x++)
                {
                    rowColor.Add(tColors[x, y]);
                }

                Console.WriteLine($"rows {y + 1} date: {string.Join(" ", rowColor)}");
            }
        }

        #region get next
        private XY GetNext(Color c, int x, int y, StringBuilder sb)
        {
            XY nextXY = Add0(c, x, y, sb);
            if (nextXY != null) return nextXY;
            nextXY = Add1(c, x, y, sb);
            if (nextXY != null) return nextXY;
            nextXY = Add2(c, x, y, sb);
            if (nextXY != null) return nextXY;
            nextXY = Add3(c, x, y, sb);
            if (nextXY != null) return nextXY;
            nextXY = Add4(c, x, y, sb);
            if (nextXY != null) return nextXY;
            nextXY = Add5(c, x, y, sb);
            if (nextXY != null) return nextXY;
            nextXY = Add6(c, x, y, sb);
            if (nextXY != null) return nextXY;
            nextXY = Add7(c, x, y, sb);
            if (nextXY != null) return nextXY;

            return null;
        }
        /// <summary>
        /// 左上
        /// </summary>
        private XY Add0(Color c, int x, int y, StringBuilder sb)
        {
            if ((x - 1) >= 0 && (y - 1) >= 0)
                return AddBase(c, x - 1, y - 1, sb, 0);

            return null;
        }
        /// <summary>
        /// 上
        /// </summary>
        private XY Add1(Color c, int x, int y, StringBuilder sb)
        {
            if ((y - 1) >= 0)
                return AddBase(c, x, y - 1, sb, 1);

            return null;
        }
        /// <summary>
        /// 右上
        /// </summary>
        private XY Add2(Color c, int x, int y, StringBuilder sb)
        {
            if ((x + 1) < tColors.GetLength(0) && (y - 1) >= 0)
                return AddBase(c, x + 1, y - 1, sb, 2);

            return null;
        }
        /// <summary>
        /// 右
        /// </summary>
        private XY Add3(Color c, int x, int y, StringBuilder sb)
        {
            if ((x + 1) < tColors.GetLength(0))
                return AddBase(c, x + 1, y, sb, 3);

            return null;
        }
        /// <summary>
        /// 右下
        /// </summary>
        private XY Add4(Color c, int x, int y, StringBuilder sb)
        {
            if ((x + 1) < tColors.GetLength(0) && (y + 1) < tColors.GetLength(1))
                return AddBase(c, x + 1, y + 1, sb, 4);

            return null;
        }
        /// <summary>
        /// 下
        /// </summary>
        private XY Add5(Color c, int x, int y, StringBuilder sb)
        {
            if ((y + 1) < tColors.GetLength(1))
                return AddBase(c, x, y + 1, sb, 5);

            return null;
        }
        /// <summary>
        /// 左下
        /// </summary>
        private XY Add6(Color c, int x, int y, StringBuilder sb)
        {
            if ((x - 1) >= 0 && (y + 1) < tColors.GetLength(1))
                return AddBase(c, x - 1, y + 1, sb, 6);

            return null;
        }
        /// <summary>
        /// 左
        /// </summary>
        private XY Add7(Color c, int x, int y, StringBuilder sb)
        {
            if ((x - 1) >= 0)
                return AddBase(c, x - 1, y, sb, 7);

            return null;
        }
        private XY AddBase(Color c, int x, int y, StringBuilder sb, int actionNum)
        {
            if (!c.Equals(tColors[x, y]) || addedXY.Any(t => t.Equals(x, y)))
                return null;

            sb.Append(actionNum);
            sb.Append(",");
            return new XY { X = x, Y = y }; ;
        }
        #endregion

        #region savefile
        private void SaveFile()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + $"method1_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (var kp in dic)
                    {
                        foreach (var v in kp.Value)
                        {
                            sw.Write($"{kp.Key.ToArgb()}:{v};");
                        }
                    }
                    sw.WriteLine();
                }
            }
        }
        #endregion

        #region clear
        private void Clear()
        {
            tSb.Clear();
            tColors = null;
            addedXY.Clear();
            dic.Clear();
        }

        #endregion
    }

    class XY
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool Equals(int x, int y)
        {
            if (x == X && y == Y) return true;
            return false;
        }
        public XY Clone()
        {
            return new XY { X = this.X, Y = this.Y };
        }
    }
}
