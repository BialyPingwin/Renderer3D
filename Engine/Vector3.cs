﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Renderer3D.Engine
{
    class Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }

        public static Vector3 ones = new Vector3(1, 1, 1);
        public static Vector3 zeros = new Vector3(0, 0, 0);

        public void Add(Vector3 v)
        {
            this.x += v.x;
            this.y += v.y;
            this.z += v.z;
        }

        public void Mul(float f)
        {
            this.x *= f;
            this.y *= f;
            this.z *= f;
        }

        public void Sub(Vector3 v)
        {
            this.x -= v.x;
            this.y -= v.y;
            this.z -= v.z;
        }

        public static Vector3 Sub(Vector3 A, Vector3 B)
        {
            Vector3 ret = new Vector3(A.x, A.y, A.z);
            ret.Sub(B);
            return ret;
        }


        public static Vector3 CrossProduct(Vector3 A, Vector3 B)
        {
            Vector3 ret = new Vector3();
            ret.x = A.y * B.z - A.z * B.y;
            ret.y = A.z * B.x - A.x * B.z;
            ret.z = A.x * B.y - A.y * B.x;
            return ret;
        }

        public static float DotProduct(Vector3 A, Vector3 B)
        {
            Vector3 ret = new Vector3();
            ret.x = A.x * B.x;
            ret.y = A.y * B.y;
            ret.z = A.z * B.z;
            return ret.x + ret.y + ret.z;
        }


        public static Vector3 VectorFromPoints(Vector3 A, Vector3 B)
        {
            Vector3 ret = new Vector3(A.x, A.y, A.z);
            ret.Sub(B);
            return ret;
        }

        public void Normalize()
        {
            float l = (float)Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            this.x /= l;
            this.y /= l;
            this.z /= l;
        }

        public static float Max(float x, float y, float z)
        {
            if (x >= y && x >= z)
            {
                return x;
            }
            else if (y >=x && y>= z){
                return y;
            }
            else if (z >= x && z >= y)
            {
                return z;
            }
            else
            {
                return x;
            }
        }

        public static float Min(float x, float y, float z)
        {
            if (x <= y && x <= z)
            {
                return x;
            }
            else if (y <= x && y <= z)
            {
                return y;
            }
            else if (z <= x && z <= y)
            {
                return z;
            }
            else
            {
                return x;
            }
        }
    }
}
