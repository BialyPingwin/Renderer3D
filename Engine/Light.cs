using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Renderer3D.Engine
{
    class Light : SceneObject
    {
        Vector3 lightDirection;
        public Light(float x, float y, float z, Vector3 lightDirection ) : base(x,y,z)
        {
            this.lightDirection = lightDirection;
            this.lightDirection.Normalize();
        }


        public float GetLightFactor(Vector3 normal)
        {
            float factor = Vector3.DotProduct(this.lightDirection, normal);

            factor += 0.5f;
            if (factor > 1f) factor = 1f;
            if (factor > 0)
            {
                return factor;
            }
            else
            {
                return 0 ;
            }
        }

        public static Color LuminateColor(Color color, float lightFactor)
        {
            Color c = Color.FromRgb((byte)(color.R * lightFactor), (byte)(color.G * lightFactor), (byte)(color.B * lightFactor));
            return c;
        }

        public static float LightFactorInterpolation(float a, float b, float c)
        {
            if (c > 1f)
            {
                c = 1f;
            }

            if (c < 0)
            {
                c = 0f;
            }

            //if (a > b)
            //{
            //    float temp = b;
            //    b = a;
            //    a = temp;
            //}

            float dif = b - a;

            return a + dif * c;

        }

        public static Color ColorInterpolation(Color a, Color b, float c)
        {

            if (c > 1f)
            {
                c = 1f;
            }

            if (c < 0)
            {
                c = 0f;
            }

            Vector3 aRgb = new Vector3(a.R, a.G, a.B);
            Vector3 bRgb = new Vector3(b.R, b.G, b.B);
            Vector3 dif = Vector3.Sub(aRgb, bRgb);

            dif.Mul(c);

            return Color.FromRgb((byte)dif.x, (byte)dif.y, (byte)dif.z);

        }
        
    }
}
