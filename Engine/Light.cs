﻿using System;
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
    }
}
