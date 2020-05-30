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
            p[0] = new Vector3(x1, y1, z3);
            p[1] = new Vector3(x2, y2, z2);
            p[2] = new Vector3(x3, y2, z3);
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


    }

    class Mesh
    {
        public Triangle[] Tris;


        public static Mesh CreateWall()
        {
            Mesh wall = new Mesh();
            wall.Tris = new Triangle[2];
            wall.Tris[0] = new Triangle(0, 0, 0, 0, 1, 0, 1, 1, 0);
            wall.Tris[1] = new Triangle(0, 0, 0, 1, 1, 0, 1, 0, 0);

            return wall;
        }

        public static Mesh CreateCube()
        {
            Mesh cube = new Mesh();
            cube.Tris = new Triangle[12];
            cube.Tris[0] = new Triangle(0, 0, 0, 0, 1, 0, 1, 1, 0);
            cube.Tris[1] = new Triangle(0, 0, 0, 1, 1, 0, 1, 0, 0);
            cube.Tris[2] = new Triangle(1, 0, 0, 1, 1, 0, 1, 1, 1);
            cube.Tris[3] = new Triangle(1, 0, 0, 1, 1, 1, 1, 0, 1);
            cube.Tris[4] = new Triangle(0, 1, 0, 0, 1, 1, 1, 1, 1);
            cube.Tris[5] = new Triangle(0, 1, 0, 1, 1, 1, 1, 1, 0);
            cube.Tris[6] = new Triangle(0, 0, 0, 0, 0, 1, 1, 0, 1);
            cube.Tris[7] = new Triangle(0, 0, 0, 1, 0, 1, 1, 0, 0);
            cube.Tris[8] = new Triangle(0, 0, 0, 0, 1, 0, 0, 1, 1);
            cube.Tris[9] = new Triangle(0, 0, 0, 0, 1, 1, 0, 0, 1);
            cube.Tris[10] = new Triangle(0, 0, 1, 0, 1, 1, 1, 1, 1);
            cube.Tris[11] = new Triangle(0, 0, 1, 1, 1, 1, 1, 0, 1);

            return cube;
        }


    }
}
