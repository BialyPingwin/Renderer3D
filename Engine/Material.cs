using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Renderer3D.Engine
{
    class Material
    {
        Color baseColor;
        float roughness;

        public Material(int R, int G, int B)
        {
            baseColor = Color.FromRgb((byte)R, (byte)G, (byte)B);
        }

        public Color GetColor()
        {
            return baseColor;
        }
    }
}
