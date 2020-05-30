using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer3D.Engine
{
   
    class Scene
    {
        List<SceneObject> objects;

        public Scene()
        {
            objects = new List<SceneObject>();
        }

        public void AddObjectToScene(SceneObject sceneObject)
        {
            objects.Add(sceneObject);
        }

        public List<Triangle> CollectTris()
        {
            List<Triangle> tris = new List<Triangle>();
            foreach (SceneObject so in objects)
            {
                if (so.mesh != null)
                {
                    foreach (Triangle t in so.mesh.Tris)
                    {

                        tris.Add(Triangle.TranslatedTriangle(t, so.position));

                        //translate with scene objects coordinate;
                    }
                }
            }

            return tris;
        }

    }
}
