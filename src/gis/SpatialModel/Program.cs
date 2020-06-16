using SpatialModel.GridCompression;
using System;
using System.Threading.Tasks.Sources;
using System.Drawing;
using System.Diagnostics;

namespace SpatialModel
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Color[,] c = GetColors();
            sw.Stop();
            Console.WriteLine($"get each pixel color, takes {(sw.ElapsedMilliseconds / 1000.0).ToString("0.00")} seconds.");

            Method1 m1 = new Method1();
            m1.DoCompress(c);

            Console.ReadKey();
        }

        static Color[,] GetColors()
        {
            //string fileName = AppDomain.CurrentDomain.BaseDirectory + @"Files/jugg.png";
            string fileName = AppDomain.CurrentDomain.BaseDirectory + @"Files/ndvi_sample.png";
            var bitmap = new Bitmap(fileName);

            Console.WriteLine($"file width: {bitmap.Width}, height: {bitmap.Height} .");

            Color[,] colors = new Color[bitmap.Width, bitmap.Height];

            for (int y = 0; y < bitmap.Height; y++)
                for (int x = 0; x < bitmap.Width; x++)
                    colors[x, y] = bitmap.GetPixel(x, y);


            bitmap.Dispose();
            return colors;
        }
    }
}
