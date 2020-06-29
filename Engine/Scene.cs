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
        List<Light> lights;
        Vector3 moveSceneVector;

        Vector3 sceneRotation;
        
        public Scene()
        {
            objects = new List<SceneObject>();
            lights = new List<Light>();
            moveSceneVector = Vector3.Zeros();
            sceneRotation = Vector3.Zeros();            
        }

        public void AddObjectToScene(SceneObject sceneObject)
        {
            
            objects.Add(sceneObject);
            if (sceneObject is Light)
            {
                lights.Add(sceneObject as Light);
            }
        }

        public List<Triangle> CollectTris()
        {
            List<Triangle> tris = new List<Triangle>();
            List<Light> translatedLights = new List<Light>();
            foreach(Light l in lights)
            {
                l.position.Add(moveSceneVector);
                translatedLights.Add(l);
            }

            
            foreach (SceneObject so in objects)
            {
                
                if (so.mesh != null)
                {
                    foreach (Triangle t in so.mesh.Tris)
                    {
                        if (so.material != null)
                        {
                            t.color = so.material.GetColor();
                        }
                        Triangle inScene = Triangle.Copy(t);
                        inScene.ScaleTriangle(so.scale);
                        inScene.RotateTriangle(so.rotation);
                        
                        inScene = Triangle.TranslatedTriangle(inScene, so.position);
                        foreach (Light l in translatedLights)
                        {
                            inScene.lightFactor = l.GetLightFactor(inScene.GetNormal());
                            for (int i = 0; i < 3; i++)
                            {
                                inScene.lightFactorPerPoint[i] = l.GetLightFactor(inScene.GetNormalPoint(i));
                            }
                        }

                        Triangle newT = Triangle.RotateTriangle(Triangle.TranslatedTriangle(inScene, moveSceneVector),sceneRotation.y);

                        if (newT.p[0].z > 0.5f && newT.p[1].z > 0.5f && newT.p[2].z > 0.5f)
                        {
                            Vector3 normal = newT.GetNormal();
                            Vector3 newP = Vector3.Sub(newT.p[0], new Vector3(0.5f, 0.5f, 0.5f));
                            if (Vector3.DotProduct(normal, newP) <= 0)
                            {
                                tris.Add(newT);
                            }
                        }
                    }
                }
            }

            return tris;
        }

        public void MoveScene(Vector3 move)
        {
            Vector3 m = move;
            m.RotateY(-1f * sceneRotation.y);
            moveSceneVector.Add(m);
        }

        public void RotateScene(float fi)
        {
            sceneRotation.y += fi;
            

        }

    }
}
