using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Renderer3D.Engine
{
    class Viewport
    {
        static WriteableBitmap viewport;
        public float width;
        public float height;

        enum Mode
        {
            wireframe, solid, shaded,smoothshaded
        }

        Mode viewportMode;
        public Viewport(int width, int height)
        {
            viewport = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);

            this.width = width;
            this.height = height;
            viewportMode = Mode.smoothshaded;
        }

        public void DrawPixel(int column, int row, Color color)
        {
            if (column >= width)
            {
                return;
            }
            if (row >= height)
            {
                return;
            }
            if (column < 0)
            {
                return;
            }
            if (row < 0)
            {
                return;
            }
            if(CheckPixel(column, row, color))
            {
                
                return;
            }

            try
            {
                // Reserve the back buffer for updates.
                viewport.Lock();

                unsafe
                {
                    // Get a pointer to the back buffer.
                    IntPtr pBackBuffer = viewport.BackBuffer;

                    // Find the address of the pixel to draw.
                    pBackBuffer += row * viewport.BackBufferStride;
                    pBackBuffer += column * 4;

                    // Compute the pixel's color.
                    int color_data = color.R << 16; // R
                    color_data |= color.G << 8;   // G
                    color_data |= color.B << 0;   // B

                    // Assign the color data to the pixel.
                    //if(*((int*)pBackBuffer) == color_data)
                    //{
                    //    return;
                    //}
                    *((int*)pBackBuffer) = color_data;
                }

                // Specify the area of the bitmap that changed.
                viewport.AddDirtyRect(new Int32Rect(column, row, 1, 1));
            }
            finally
            {
                // Release the back buffer and make it available for display.
                viewport.Unlock();
            }


        }

        public void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {

            if (x2 <= x1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }
            if (y2 < y1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }

            if (y2 - y1 > x2 - x1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }

            int deltax, deltay, g, h, c;

            deltax = x2 - x1;
            if (deltax > 0) g = +1; else g = -1;
            deltax = Math.Abs(deltax);
            deltay = y2 - y1;
            if (deltay > 0) h = +1; else h = -1;
            deltay = Math.Abs(deltay);
            if (deltax > deltay)
            {
                c = -deltax;
                while (x1 != x2)
                {

                    DrawPixel(x1, y1, color);

                    c += 2 * deltay;
                    if (c > 0) { y1 += h; c -= 2 * deltax; }
                    x1 += g;
                }
            }
            else
            {
                c = -deltay;
                while (y1 != y2)
                {
                    DrawPixel(x1, y1, color);


                    c += 2 * deltax;
                    if (c > 0) { x1 += g; c -= 2 * deltay; }
                    y1 += h;
                }
            }

            
        }

        public void DrawSmoothLine(int x1, int y1, int x2, int y2, Color color, float lightfactor2, float lightfactor1)
        {

            if (x2 <= x1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
                float ftmp = lightfactor1;
                lightfactor1 = lightfactor2;
                lightfactor2 = tmp;
            }
            if (y2 < y1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
                float ftmp = lightfactor1;
                lightfactor1 = lightfactor2;
                lightfactor2 = tmp;
            }

            if (y2 - y1 > x2 - x1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;

                float ftmp = lightfactor1;
                lightfactor1 = lightfactor2;
                lightfactor2 = tmp;

            }

            int deltax, deltay, g, h, c;
            float baseDistance = Distance(x1, y1, x2, y2);
            deltax = x2 - x1;
            if (deltax > 0) g = +1; else g = -1;
            deltax = Math.Abs(deltax);
            deltay = y2 - y1;
            if (deltay > 0) h = +1; else h = -1;
            deltay = Math.Abs(deltay);
            if (deltax > deltay)
            {
                c = -deltax;
                while (x1 != x2)
                {
                    float actualDistanceFactor = Distance(x1, y1, x2, y2) / baseDistance;
                    float actualLightFactor = Light.LightFactorInterpolation(lightfactor1, lightfactor2, actualDistanceFactor);

                    DrawPixel(x1, y1, Light.LuminateColor(color,actualLightFactor));

                    c += 2 * deltay;
                    if (c > 0) { y1 += h; c -= 2 * deltax; }
                    x1 += g;
                }
            }
            else
            {
                c = -deltay;
                while (y1 != y2)
                {
                    float actualDistanceFactor = Distance(x1, y1, x2, y2) / baseDistance;
                    float actualLightFactor = Light.LightFactorInterpolation(lightfactor1, lightfactor2, actualDistanceFactor);

                    DrawPixel(x1, y1, Light.LuminateColor(color, actualLightFactor));


                    c += 2 * deltax;
                    if (c > 0) { x1 += g; c -= 2 * deltay; }
                    y1 += h;
                }
            }


        }

        public void DrawTriangle(Triangle t)
        {
            if(viewportMode == Mode.wireframe)
            {
                DrawTriangleWireframe(t);
            }
            else if( viewportMode == Mode.solid || viewportMode == Mode.shaded)
            {
                FillTriangle(t);
            }
            else if (viewportMode == Mode.smoothshaded)
            {
                FillSmoothTriangle(t);
            }
        }

        public void DrawTriangleWireframe(Triangle t)
        {
            DrawLine((int)t.p[0].x, (int)t.p[0].y, (int)t.p[1].x, (int)t.p[1].y, Color.FromRgb(255, 255, 255));
            DrawLine((int)t.p[1].x, (int)t.p[1].y, (int)t.p[2].x, (int)t.p[2].y, Color.FromRgb(255, 255, 255));
            DrawLine((int)t.p[2].x, (int)t.p[2].y, (int)t.p[0].x, (int)t.p[0].y, Color.FromRgb(255, 255, 255));
        }

        public void FillTriangle(Triangle t)
        {
            
            Color TriangleColor = Color.FromRgb(255,255,255);
            int lineToDrop = 1;
            if (viewportMode == Mode.shaded)
            {
                TriangleColor = t.GetColor(true);
                
            }
            else if (viewportMode == Mode.solid)
            {
                TriangleColor = t.GetColor(false);
                
            }
            DrawLine((int)t.p[0].x, (int)t.p[0].y, (int)t.p[1].x, (int)t.p[1].y, TriangleColor);
            DrawLine((int)t.p[1].x, (int)t.p[1].y, (int)t.p[2].x, (int)t.p[2].y, TriangleColor);

            Vector3 fillOrigin = new Vector3(t.p[1].x, t.p[1].y, 0);
                       

            int x1 = (int)t.p[2].x;
            int y1 = (int)t.p[2].y;
            int x2 = (int)t.p[0].x;
            int y2 = (int)t.p[0].y;
            int lineNumber = 0;
            

            if (x2 <= x1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }
            if (y2 < y1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }

            if (y2 - y1 > x2 - x1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }

            int deltax, deltay, g, h, c;

            deltax = x2 - x1;
            if (deltax > 0) g = +1; else g = -1;
            deltax = Math.Abs(deltax);
            deltay = y2 - y1;
            if (deltay > 0) h = +1; else h = -1;
            deltay = Math.Abs(deltay);
            if (deltax > deltay)
            {
                c = -deltax;
                while (x1 != x2)
                {
                    DrawPixel(x1, y1, TriangleColor);
                    if (lineNumber % lineToDrop == 0)
                    {
                        DrawBoldLine((int)fillOrigin.x, (int)fillOrigin.y, x1, y1, TriangleColor, 4);
                    }
                    lineNumber++;

                    c += 2 * deltay;
                    if (c > 0) { y1 += h; c -= 2 * deltax; }
                    x1 += g;
                }
            }
            else
            {
                c = -deltay;
                while (y1 != y2)
                {
                    DrawPixel(x1, y1, TriangleColor);
                    if (lineNumber % lineToDrop == 0)
                    {
                        DrawBoldLine((int)fillOrigin.x, (int)fillOrigin.y, x1, y1, TriangleColor, 4);
                    }
                    lineNumber++;
                    c += 2 * deltax;
                    if (c > 0) { x1 += g; c -= 2 * deltay; }
                    y1 += h;
                }
            }

        }

        public void FillSmoothTriangle(Triangle t)
        {

            Color TriangleColor = t.GetColor(false);
            
            
            
            //DrawSmoothLine((int)t.p[1].x, (int)t.p[1].y, (int)t.p[2].x, (int)t.p[2].y, TriangleColor, t.lightFactorPerPoint[1], t.lightFactorPerPoint[2]);

            Vector3 fillOrigin = new Vector3(t.p[1].x, t.p[1].y, 0);


            int x1 = (int)t.p[2].x;
            int y1 = (int)t.p[2].y;
            int x2 = (int)t.p[0].x;
            int y2 = (int)t.p[0].y;

            float lineNumber = 0;
            float lineToDrop = 1;

            float lightfactor1 = t.lightFactorPerPoint[2];
            float lightfactor2 = t.lightFactorPerPoint[0];
            float originLightFactor = t.lightFactorPerPoint[1];

            if (x2 <= x1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }
            if (y2 < y1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }

            if (y2 - y1 > x2 - x1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }

            int deltax, deltay, g, h, c;
            float baseDistance = Distance(x1, y1, x2, y2);
            deltax = x2 - x1;
            if (deltax > 0) g = +1; else g = -1;
            deltax = Math.Abs(deltax);
            deltay = y2 - y1;
            if (deltay > 0) h = +1; else h = -1;
            deltay = Math.Abs(deltay);
            if (deltax > deltay)
            {
                c = -deltax;
                while (x1 != x2)
                {
                    float actualDistanceFactor =Distance(x1, y1, x2, y2) /baseDistance;
                    float actualLightFactor = Light.LightFactorInterpolation(lightfactor1, lightfactor2, actualDistanceFactor);

                    DrawPixel(x1, y1, Light.LuminateColor(TriangleColor, actualLightFactor));
                    
                    if (lineNumber % lineToDrop == 0)
                    {
                        DrawSmoothBoldLine((int)fillOrigin.x, (int)fillOrigin.y, x1, y1, TriangleColor, 4, originLightFactor, actualLightFactor);
                        
                    }
                    lineNumber++;
                    c += 2 * deltay;
                    if (c > 0) { y1 += h; c -= 2 * deltax; }
                    x1 += g;
                }
            }
            else
            {
                c = -deltay;
                while (y1 != y2)
                {
                    float actualDistanceFactor = Distance(x1, y1, x2, y2) / baseDistance;
                    float actualLightFactor = Light.LightFactorInterpolation(lightfactor1, lightfactor2, actualDistanceFactor);

                    DrawPixel(x1, y1, Light.LuminateColor(TriangleColor, actualLightFactor));
                    
                    if (lineNumber % lineToDrop == 0)
                    {
                        DrawSmoothBoldLine((int)fillOrigin.x, (int)fillOrigin.y, x1, y1, TriangleColor, 4, originLightFactor, actualLightFactor);
                        
                    }
                    lineNumber++;
                    c += 2 * deltax;
                    if (c > 0) { x1 += g; c -= 2 * deltay; }
                    y1 += h;
                }
            }
            //DrawSmoothBoldLine((int)t.p[0].x, (int)t.p[0].y, (int)t.p[1].x, (int)t.p[1].y, TriangleColor,4, t.lightFactorPerPoint[0], t.lightFactorPerPoint[1]);

        }

        public WriteableBitmap ShowViewport()
        {
            return viewport;
        }

        private bool CheckPixel(int x, int y, Color color)
        {
            int stride = (viewport.Format.BitsPerPixel / 8);
            Int32Rect pixelCheck = new Int32Rect(x, y, 1, 1);
            byte[] data = new byte[stride];
            viewport.CopyPixels(pixelCheck, data, stride, 0);
            if (data[0] == color.B && data[1] == color.G && data[2] == color.R)
            {
                return true;
            }
            else return false;
            
        }

        public void DrawBoldLine(int x1, int y1, int x2, int y2, Color color, int bold)
        {
            if (x2 <= x1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }
            if (y2 < y1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }

            if (y2 - y1 > x2 - x1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }

            int deltax, deltay, g, h, c;

            deltax = x2 - x1;
            if (deltax > 0) g = +1; else g = -1;
            deltax = Math.Abs(deltax);
            deltay = y2 - y1;
            if (deltay > 0) h = +1; else h = -1;
            deltay = Math.Abs(deltay);
            if (deltax > deltay)
            {
                c = -deltax;
                while (x1 != x2)
                {
                    for (int i = -(int)bold / 2; i < (int)bold / 2; i++)
                    {
                        DrawPixel(x1, y1+i, color);
                    }
                    

                    c += 2 * deltay;
                    if (c > 0) { y1 += h; c -= 2 * deltax; }
                    x1 += g;
                }
            }
            else
            {
                c = -deltay;
                while (y1 != y2)
                {
                    for (int i =  -(int)bold/2 ; i < (int)bold /2; i++)
                    {
                        DrawPixel(x1 + i, y1, color);
                    }


                    c += 2 * deltax;
                    if (c > 0) { x1 += g; c -= 2 * deltay; }
                    y1 += h;
                }
            }
        }

        public void DrawSmoothBoldLine(int x1, int y1, int x2, int y2, Color color, int bold, float lightfactor2, float lightfactor1)
        {
            if (x2 <= x1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }
            if (y2 < y1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }

            if (y2 - y1 > x2 - x1)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }

            int deltax, deltay, g, h, c;
            float baseDistance = Distance(x1, y1, x2, y2);
            deltax = x2 - x1;
            if (deltax > 0) g = +1; else g = -1;
            deltax = Math.Abs(deltax);
            deltay = y2 - y1;
            if (deltay > 0) h = +1; else h = -1;
            deltay = Math.Abs(deltay);
            if (deltax > deltay)
            {
                c = -deltax;
                while (x1 != x2)
                {
                    float actualDistanceFactor = Distance(x1, y1, x2, y2) / baseDistance;
                    float actualLightFactor = Light.LightFactorInterpolation(lightfactor1, lightfactor2, actualDistanceFactor);

                    

                    for (int i = -(int)bold / 2; i < (int)bold / 2; i++)
                    {
                        DrawPixel(x1, y1 + i, Light.LuminateColor(color, actualLightFactor));
                    }


                    c += 2 * deltay;
                    if (c > 0) { y1 += h; c -= 2 * deltax; }
                    x1 += g;
                }
            }
            else
            {
                c = -deltay;
                while (y1 != y2)
                {
                    float actualDistanceFactor = Distance(x1, y1, x2, y2) / baseDistance;
                    float actualLightFactor = Light.LightFactorInterpolation(lightfactor1, lightfactor2, actualDistanceFactor);

                    

                    for (int i = -(int)bold / 2; i < (int)bold / 2; i++)
                    {
                        DrawPixel(x1 + i, y1, Light.LuminateColor(color, actualLightFactor));
                    }


                    c += 2 * deltax;
                    if (c > 0) { x1 += g; c -= 2 * deltay; }
                    y1 += h;
                }
            }
        }



        public void ClearViewport()
        {
            
            Int32Rect rect = new Int32Rect(0, 0, viewport.PixelWidth, viewport.PixelHeight);
            int bytesPerPixel = viewport.Format.BitsPerPixel / 8; // typically 4 (BGR32)
            byte[] empty = new byte[rect.Width * rect.Height * bytesPerPixel]; // cache this one
            int emptyStride = rect.Width * bytesPerPixel;

            viewport.WritePixels(rect, empty, emptyStride, 0);
        }
            
        public void RenderMode(int i)
        {
            if (i == 1)
            {
                viewportMode = Mode.wireframe;
            }
            else if( i== 2)
            {
                viewportMode = Mode.solid;
            }
            else if (i == 3)
            {
                viewportMode = Mode.shaded;
            }
            else if (i == 4)
            {
                viewportMode = Mode.smoothshaded;
            }
        }
        
    
        private float Distance(int x1, int y1, int x2, int y2)
        {
            return (float)Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }
        
    }
}
