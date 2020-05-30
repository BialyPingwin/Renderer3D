using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer3D.Engine
{
    class SceneObject
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;


        public Mesh mesh;
        public SceneObject(float x, float y, float z)
        {
            position = new Vector3(x, y, z);
            rotation = Vector3.ones;
            scale = Vector3.ones;
           
        }


    }
}
