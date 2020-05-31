using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer3D.Engine
{
    class Triangle
    {
        public Vector3[] p;
        public Triangle(float x1, float y1, float z1, float x2, float y2, float z2, float x3, float y3, float z3)
        {
            p = new Vector3[3];
            p[0] = new Vector3(x1, y1, z1);
            p[1] = new Vector3(x2, y2, z2);
            p[2] = new Vector3(x3, y3, z3);
        }

        public void Translate(Vector3 translate)
        {
            foreach (Vector3 v in p)
            {
                v.x += translate.x;
                v.y += translate.y;
                v.z += translate.z;
            }
        }

        public static Triangle TranslatedTriangle(Triangle t, Vector3 translate)
        {
            Triangle n = new Triangle(t.p[0].x, t.p[0].y, t.p[0].z, t.p[1].x, t.p[1].y, t.p[1].z, t.p[2].x, t.p[2].y, t.p[2].z);

            n.Translate(translate);

            return n;
        }

        public static Triangle RotateTriangle(Triangle t, float angle)
        {
            Triangle n = new Triangle(t.p[0].x, t.p[0].y, t.p[0].z, t.p[1].x, t.p[1].y, t.p[1].z, t.p[2].x, t.p[2].y, t.p[2].z);
            n.p[0].RotateY(angle);
            n.p[1].RotateY(angle);
            n.p[2].RotateY(angle);

            return n;
        }


    }
}
