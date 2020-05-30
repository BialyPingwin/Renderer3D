using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer3D.Engine
{
    
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
            cube.Tris[0] = new Triangle(0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f);
            cube.Tris[1] = new Triangle(0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f, 0.0f);
            cube.Tris[2] = new Triangle(1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f);
            cube.Tris[3] = new Triangle(1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f);
            cube.Tris[4] = new Triangle(1.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 1.0f);
            cube.Tris[4] = new Triangle(1.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 1.0f);
            cube.Tris[5] = new Triangle(1.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 0.0f, 1.0f);
            cube.Tris[6] = new Triangle(0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f);
            cube.Tris[7] = new Triangle(0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f);
            cube.Tris[8] = new Triangle(0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
            cube.Tris[9] = new Triangle(0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f);
            cube.Tris[10] = new Triangle(1.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f);
            cube.Tris[11] = new Triangle(1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f);
            return cube;
        }


    }
}
