using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            wireframe, solid, shaded
        }

        Mode viewportMode;
        public Viewport(int width, int height)
        {
            viewport = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);

            this.width = width;
            this.height = height;
            viewportMode = Mode.solid;
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

            //int x, y, dx, dy, dx1, dy1, px, py, xe, ye, i;
            //dx = x2 - x1; dy = y2 - y1;
            //dx1 = Math.Abs(dx); dy1 = Math.Abs(dy);
            //px = 2 * dy1 - dx1; py = 2 * dx1 - dy1;
            //if (dy1 <= dx1)
            //{
            //    if (dx >= 0)
            //    { x = x1; y = y1; xe = x2; }
            //    else
            //    { x = x2; y = y2; xe = x1; }

            //    DrawPixel(x, y, color);

            //    for (i = 0; x < xe; i++)
            //    {
            //        x = x + 1;
            //        if (px < 0)
            //            px = px + 2 * dy1;
            //        else
            //        {
            //            if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0)) y = y + 1; else y = y - 1;
            //            px = px + 2 * (dy1 - dx1);
            //        }
            //        DrawPixel(x, y, color);
            //    }
            //}
            //else
            //{
            //    if (dy >= 0)
            //    { x = x1; y = y1; ye = y2; }
            //    else
            //    { x = x2; y = y2; ye = y1; }

            //    DrawPixel(x, y, color);

            //    for (i = 0; y < ye; i++)
            //    {
            //        y = y + 1;
            //        if (py <= 0)
            //            py = py + 2 * dx1;
            //        else
            //        {
            //            if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0)) x = x + 1; else x = x - 1;
            //            py = py + 2 * (dx1 - dy1);
            //        }
            //        DrawPixel(x, y, color);
            //    }
            //}

        }

        public void DrawTriangle(Triangle t)
        {
            if(viewportMode == Mode.wireframe)
            {
                DrawTriangleWireframe(t);
            }
            else if( viewportMode == Mode.solid)
            {
                FillTriangle(t);
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
            //int originY = (int)Vector3.Max(t.p[0].x, t.p[1].x, t.p[2].x);
            //int originX = (int)Vector3.Min(t.p[0].y, t.p[1].y, t.p[2].y);
            //int dimX = (int)Vector3.Max(t.p[0].x, t.p[1].x, t.p[2].x) - (int)Vector3.Min(t.p[0].x, t.p[1].x, t.p[2].x);
            //int dimY = (int)Vector3.Max(t.p[0].y, t.p[1].y, t.p[2].y) - (int)Vector3.Min(t.p[0].y, t.p[1].y, t.p[2].y);

            //int[,] toFill = new int[dimX+1, dimY+1];
            DrawLine((int)t.p[0].x, (int)t.p[0].y, (int)t.p[1].x, (int)t.p[1].y, Color.FromRgb(255, 255, 255));
            DrawLine((int)t.p[1].x, (int)t.p[1].y, (int)t.p[2].x, (int)t.p[2].y, Color.FromRgb(255, 255, 255));


            List<Vector3> toFillPoints = new List<Vector3>();
            Vector3 fillOrigin = new Vector3(t.p[1].x, t.p[1].y, 0);

            //for (int i = 0; i < 3; i++)
            //{

            //    int j = i + 1;
            //    if (j > 2)
            //    {
            //        j = 0;
            //    }

            //int x1 = (int)t.p[i].x;
            //int y1 = (int)t.p[i].y;
            //int x2 = (int)t.p[j].x;
            //int y2 = (int)t.p[j].y;

            int x1 = (int)t.p[2].x;
            int y1 = (int)t.p[2].y;
            int x2 = (int)t.p[0].x;
            int y2 = (int)t.p[0].y;
            int lineNumber = 0;
            int lineToDrop = 2;

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
                    //toFill[-originX + x1, originY -y1] = 1;
                    //toFillPoints.Add(new Vector3(x1, y1,0));
                    DrawPixel(x1, y1, Color.FromRgb(255, 255, 255));
                    if (lineNumber % lineToDrop == 0)
                    {
                        DrawBoldLine((int)fillOrigin.x, (int)fillOrigin.y, x1, y1, Color.FromRgb(255, 255, 255),4);
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
                    //toFill[-originX +x1, originY - y1] = 1;

                    //toFillPoints.Add(new Vector3(x1, y1, 0));
                    DrawPixel(x1, y1, Color.FromRgb(255, 255, 255));
                    if (lineNumber % lineToDrop == 0)
                    {
                        DrawBoldLine((int)fillOrigin.x, (int)fillOrigin.y, x1, y1, Color.FromRgb(255, 255, 255),4);
                    }
                    lineNumber++;
                    c += 2 * deltax;
                    if (c > 0) { x1 += g; c -= 2 * deltay; }
                    y1 += h;
                }
            }

            //}

            //int i = 0;
            //foreach(Vector3 v in toFillPoints)
            //{
            //    if (i % 2 == 0)
            //    {
            //        DrawLine((int)fillOrigin.x, (int)fillOrigin.y, (int)v.x, (int)v.y, Color.FromRgb(255, 255, 255));
            //    }
            //    i++;
            //}
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
        }
        
    

        
    }
}
