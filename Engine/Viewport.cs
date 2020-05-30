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
            int z_x = 0, z_y = 0;
            if (x1 > x2)
            {
                int tmp = x1;
                x1 = x2;
                x2 = tmp;
                tmp = y1;
                y1 = y2;
                y2 = tmp;
            }

            if (x1 < 0)
            {
                z_x = Math.Abs(x1);
                x2 += Math.Abs(x1);
                x1 = 0;
            }
            if (y1 < 0)
            {
                z_y = Math.Abs(y1);
                y2 += Math.Abs(y1);
                y1 = 0;
            }


            float dy, dx, x, y, m;
            dy = y2 - y1;
            dx = x2 - x1;
            m = dy / dx;
            y = y1;
            x = x1;

            if (Math.Abs(m) < 1)
            {
                for (x = x1; x <= x2; x++)
                {
                    DrawPixel((int)(int)Math.Floor(x), (int)Math.Floor(y + 0.5f), color);
                    y += m;
                                       
                }
            }
            else
            {
                dx = 1 / m;
                for (y = y1; y <= y2; y++)
                {
                    DrawPixel((int)Math.Floor(x + 0.5f), (int)Math.Floor(y), color);
                    x += dx;               
                                       
                }
            }
        }

        public void DrawTriangle(Triangle t)
        {
            DrawLine((int)t.p[0].x, (int)t.p[0].y, (int)t.p[1].x, (int)t.p[1].y, Color.FromRgb(255,255,255));
            DrawLine((int)t.p[1].x, (int)t.p[1].y, (int)t.p[2].x, (int)t.p[2].y, Color.FromRgb(255, 255, 255));
            DrawLine((int)t.p[2].x, (int)t.p[2].y, (int)t.p[0].x, (int)t.p[0].y, Color.FromRgb(255, 255, 255));
        }

        public WriteableBitmap ShowViewport()
        {
            return viewport;
        }
    }
}
