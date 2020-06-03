using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Renderer3D.Engine
{
    class Animator
    {
        SceneObject sceneObject;

        public Animator(SceneObject sceneObject)
        {
            this.sceneObject = sceneObject;
            
        }

        public void Animate(Func<SceneObject, SceneObject> func)
        {
            SceneObject s = func(sceneObject);
            this.sceneObject = s;
        }
    }
}
