using SpatialModel.GridCompression;
using System;
using System.Threading.Tasks.Sources;
using System.Drawing;

namespace SpatialModel
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string fileName = AppDomain.CurrentDomain.BaseDirectory + @"Files/jugg.png";
            using Image image = Image.FromFile(fileName);

            Console.WriteLine($"file width: {image.Width}, height: {image.Height} .");

            Color[,] colors = new Color[image.Width, image.Height];
            var bitmap = image.Clone() as Bitmap;

            for (int y = 0; y < bitmap.Height; y++)
                for (int x = 0; x < bitmap.Width; x++)
                    colors[x, y] = bitmap.GetPixel(x, y);

            Method1 m1 = new Method1();
            m1.DoCompress(colors);

            Console.ReadKey();
        }
    }
}
