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

        public Viewport(int width, int height)
        {
            viewport = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);

            this.width = width;
            this.height = height;
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

        public void DrawTriangle(Triangle t)
        {
            DrawLine((int)t.p[0].x, (int)t.p[0].y, (int)t.p[1].x, (int)t.p[1].y, Color.FromRgb(255, 255, 255));
            DrawLine((int)t.p[1].x, (int)t.p[1].y, (int)t.p[2].x, (int)t.p[2].y, Color.FromRgb(255, 255, 255));
            DrawLine((int)t.p[2].x, (int)t.p[2].y, (int)t.p[0].x, (int)t.p[0].y, Color.FromRgb(255, 255, 255));
        }

        public WriteableBitmap ShowViewport()
        {
            return viewport;
        }

        


        
        public void ClearViewport()
        {
            
            Int32Rect rect = new Int32Rect(0, 0, viewport.PixelWidth, viewport.PixelHeight);
            int bytesPerPixel = viewport.Format.BitsPerPixel / 8; // typically 4 (BGR32)
            byte[] empty = new byte[rect.Width * rect.Height * bytesPerPixel]; // cache this one
            int emptyStride = rect.Width * bytesPerPixel;

            viewport.WritePixels(rect, empty, emptyStride, 0);
        }
            

        
    

        
    }
}
