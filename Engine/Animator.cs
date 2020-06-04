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
        public List<bool> opts;
        public Animator(SceneObject sceneObject)
        {
            this.sceneObject = sceneObject;
            opts = new List<bool>();
        }

        public void Animate(Func<SceneObject, SceneObject> func)
        {
            SceneObject s = func(sceneObject);
            this.sceneObject = s;
        }
    }
}
