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
        Vector3 moveSceneVector;
        public Scene()
        {
            objects = new List<SceneObject>();
            moveSceneVector = Vector3.zeros;
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


                        Triangle newT = Triangle.TranslatedTriangle(Triangle.TranslatedTriangle(t, so.position), moveSceneVector);
                        
                        if(newT.p[0].z > 0.5f && newT.p[1].z > 0.5f && newT.p[2].z > 0.5f)
                        {
                            Vector3 normal = Vector3.CrossProduct(Vector3.VectorFromPoints(newT.p[1], newT.p[0]), Vector3.VectorFromPoints(newT.p[2], newT.p[0]));
                            normal.Normalize();
                            //if (normal.z <= 0)
                            //{
                                tris.Add(newT);
                            //}
                        }
                        //translate with scene objects coordinate;
                    }
                }
            }

            return tris;
        }

        public void MoveScene(Vector3 move)
        {
            moveSceneVector.Add(move);
        }

    }
}
