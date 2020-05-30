using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer3D.Engine
{
    class Controls
    {
        public float Horizontal = 0f;
        public float Vertical = 0f;
        public float Speed = 0.1f;

        public Vector3 GetControls()
        {
            return new Vector3(Horizontal * Speed, 0, Vertical * Speed);
        }

        public void SetVertical(float input)
        {
            Vertical = input;
        }

        public void SetHorizontal(float input)
        {
            Horizontal = input;
        }

        public void ResetControls()
        {
            Horizontal = 0f;
            Vertical = 0f;
        }


    }
}
