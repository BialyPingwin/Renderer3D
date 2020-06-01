﻿using System;
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
        float sceneRotation = 0f;
        public Scene()
        {
            objects = new List<SceneObject>();
            lights = new List<Light>();
            moveSceneVector = Vector3.zeros;
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
                        

                        Triangle inScene = Triangle.TranslatedTriangle(t, so.position);
                        foreach (Light l in translatedLights)
                        {
                            inScene.lightFactor = l.GetLightFactor(inScene.GetNormal());
                            //optymalizacja by się przydała, żeby sprawdzało sie tylko gdy vieport ma włączone smooth shading
                            for (int i = 0; i < 3; i++)
                            {
                                inScene.lightFactorPerPoint[i] = l.GetLightFactor(inScene.GetNormalPoint(i));
                            }
                        }

                        Triangle newT = Triangle.RotateTriangle(Triangle.TranslatedTriangle(inScene, moveSceneVector),sceneRotation);

                        if (newT.p[0].z > 0.5f && newT.p[1].z > 0.5f && newT.p[2].z > 0.5f)
                        {
                            Vector3 normal = newT.GetNormal();
                            Vector3 newP = Vector3.Sub(newT.p[0], new Vector3(0.4f, 0f, 0.4f));
                            if (Vector3.DotProduct(normal, newP) < 0)
                            {
                                tris.Add(newT);
                            }
                        }
                        //translate with scene objects coordinate;
                    }
                }
            }

            return tris;
        }

        public void MoveScene(Vector3 move)
        {
            Vector3 m = move;
            m.RotateY(-sceneRotation);
            moveSceneVector.Add(m);
        }

        public void RotateScene(float fi)
        {
            sceneRotation += fi;
        }

    }
}
