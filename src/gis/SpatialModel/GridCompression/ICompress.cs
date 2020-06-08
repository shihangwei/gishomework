using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SpatialModel.GridCompression
{
    interface ICompress
    {
        void DoCompress(Color[,] colors);
    }
}
