using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Renderer3D.Engine
{
    class Camera 
    {
        Viewport viewport;
        Scene scene;

        float nearClippingPlane = 0.1f;
        float farClippingPlane = 1000f;
        float Fov = 90.0f;
        float AspectRatio; ///from viewport h/w
        float FovRad;// = 1.0f / Math.Tan(Fov * 0.5f / 180.0f * Math.PI);
        
        public Camera(Viewport viewport, Scene scene)
        {
            this.viewport = viewport;
            this.scene = scene;

            AspectRatio = viewport.height / viewport.width;
            FovRad = (float)(1.0f / Math.Tan(Fov * 0.5f / 180.0f * Math.PI));


        }

        public Vector3 Project(Vector3 vin)
        {
            Vector3 vout = new Vector3();
            vout.x = vin.x * AspectRatio * FovRad;
            vout.y = vin.y * FovRad;
            vout.z = vin.z * farClippingPlane / (farClippingPlane - nearClippingPlane) + (-farClippingPlane * nearClippingPlane)/(farClippingPlane - nearClippingPlane);
            float w = vin.z * 1.0f + 0;

            if (w != 0.0f)
            {
                vout.x /= w;
                vout.y /= w;
                vout.z /= w;
            }

            return vout;
        }

        public void Render() 
        {
            List<Triangle> trisToRender = scene.CollectTris();

            viewport.ClearViewport();
            foreach(Triangle t in trisToRender)
            {
                //t.p[0].z += 3f;
                //t.p[1].z += 3f;
                //t.p[2].z += 3f;

                t.p[0].x += -0.5f;
                t.p[1].x += -0.5f;
                t.p[2].x += -0.5f;

                t.p[0].y += -0.5f;
                t.p[1].y += -0.5f;
                t.p[2].y += -0.5f;

                ///System.Windows.MessageBox.Show(t.p[0].x +" " + t.p[0].y+" " +t.p[0].z) ;

                t.p[0] = Project(t.p[0]);
                t.p[1] = Project(t.p[1]);
                t.p[2] = Project(t.p[2]);

                t.p[0].x += 1.0f; t.p[0].y += 1.0f;
                t.p[1].x += 1.0f; t.p[1].y += 1.0f;
                t.p[2].x += 1.0f; t.p[2].y += 1.0f;

                t.p[0].x *= 0.5f * viewport.width;
                t.p[0].y *= 0.5f * viewport.height;
                t.p[1].x *= 0.5f * viewport.width;
                t.p[1].y *= 0.5f * viewport.height;
                t.p[2].x *= 0.5f * viewport.width;
                t.p[2].y *= 0.5f * viewport.height;

                viewport.FillTriangle(t);
            }

            
        }
        
    }
}
