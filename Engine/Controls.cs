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
        public float Rotation = 0f;
        public float MoveSpeed = 0.1f;
        public float RotationSpeed = 0.1f;

        public Vector3 GetControls()
        {
            return new Vector3(Horizontal * MoveSpeed, 0, Vertical * MoveSpeed);
        }

        public float GetRotation()
        {
            return Rotation * RotationSpeed;
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

        public void SetRotation(float r)
        {
            Rotation = r;
        }

    }
}
