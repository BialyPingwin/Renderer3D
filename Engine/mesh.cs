using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Globalization;

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

        public static Mesh LoadFromObj(string file)
        {

            List<Vector3> points = new List<Vector3>();
            List<Triangle> triangles = new List<Triangle>();
            try
            {
                
                using (StreamReader sr = new StreamReader(file))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line[0] == 'v')
                        {

                            string[] dim = line.Split(' ');                                                       
                            points.Add(new Vector3((float.Parse(dim[1], CultureInfo.InvariantCulture.NumberFormat)), (float.Parse(dim[2], CultureInfo.InvariantCulture.NumberFormat)), (float.Parse(dim[3], CultureInfo.InvariantCulture.NumberFormat))));
                        }
                        if (line[0] == 'f')
                        {
                            string[] pointNumber = line.Split(' ');
                            triangles.Add(new Triangle(points[Convert.ToInt32(pointNumber[1]) -1], points[Convert.ToInt32(pointNumber[2]) - 1], points[Convert.ToInt32(pointNumber[3]) - 1]));
                        }
                        

                    }
                }

                Mesh mesh = new Mesh();
                mesh.Tris = triangles.ToArray();
                return mesh;
                
                
            }
            catch(Exception e)
            {
                MessageBox.Show("Error with obj " + e.Message);
                return null;
            }
        }

    }
}
