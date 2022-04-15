using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RayCasting
{
    static class RayCasting
    {
        static float xc, yc;
        public static void Do(Graphics g, Section[] sections, Camera camera)
        {
            var a = 0f;
            while (a < camera.view_angle)
            {
                var rx = camera.depth * (float)Math.Cos((camera.angle + a) * Math.PI / 180) + camera.x;
                var ry = camera.depth * (float)Math.Sin((camera.angle + a) * Math.PI / 180) + camera.y;

                float[,] cp = new float[sections.Length, 3];
                bool[] c = new bool[sections.Length]; 

                for (int i = 0; i < sections.Length; i++)
                {
                    xc = 0;
                    yc = 0;
                    if(Crossing(camera.x, camera.y, rx, ry, sections[i].x1, sections[i].y1, sections[i].x2, sections[i].y2, xc, yc))
                    {
                        c[i] = true;
                        cp[i, 0] = xc;
                        cp[i, 1] = yc;
                        cp[i, 2] = (float)Math.Sqrt((camera.x - xc) * (camera.x - xc) + (camera.y - yc) * (camera.y - yc));
                    }
                    else
                    {
                        c[i] = false;
                        cp[i, 2] = float.PositiveInfinity;
                        g.FillRectangle(new SolidBrush(Color.Black), 200 + (3 * a / camera.step), 10, 3, camera.screen_height);
                        //for (int j = 0; j < (camera.screen_height / 2) / 8; j++)
                        //{
                        //    var floor_brightness = 255 - (int)(255 * Math.Sqrt(((a / camera.step) - ((camera.view_angle / 2) / camera.step)) * ((a / camera.step) - ((camera.view_angle / 2) / camera.step)) * 0.5 + ((camera.screen_height / 8) - j) * ((camera.screen_height / 8) - j) * 1.5) / camera.depth);
                        //    if (floor_brightness < 0)
                        //        floor_brightness = 0;
                        //    g.FillRectangle(new SolidBrush(Color.FromArgb(255, floor_brightness, floor_brightness, floor_brightness)), 200 + (2 * a / camera.step), 10 + camera.screen_height / 2 + 8 * j, 2, 8);
                        //}
                    }
                    g.DrawLine(new Pen(Color.Black), camera.x, camera.y, rx, ry);
                    g.DrawLine(new Pen(Color.Black), sections[i].x1, sections[i].y1, sections[i].x2, sections[i].y2);
                }
                float rMin = camera.depth + 1;
                int nMin = sections.Length;
                for (int i = 0; i < sections.Length; i++)
                {
                    if(c[i])
                    {
                        if (cp[i, 2] < rMin)
                        {
                            rMin = cp[i, 2];
                            nMin = i;
                        }
                    }
                }
                if(nMin < sections.Length)
                    if(c[nMin])
                    {
                        var aDiff = 0f;
                        if (a <= camera.view_angle / 2)
                            aDiff = camera.view_angle / 2 - a;
                        else
                            aDiff = a - camera.view_angle / 2;

                        var h = camera.screen_distance * 100f / (cp[nMin, 2] /* * (float)Math.Cos(aDiff * Math.PI / 180)*/);
                        if (h > camera.screen_height)
                            h = camera.screen_height;
                        var brightness = (int)(255 - (255 * cp[nMin, 2] / camera.depth));
                        if (brightness > 255)
                            brightness = 255;
                        g.FillRectangle(new SolidBrush(Color.Black), 200 + (3 * a / camera.step), 10, 3, camera.screen_height / 2 - h / 2);
                        //for (int j = 0; j < (camera.screen_height / 2) / 8; j++)
                        //{
                        //    var floor_brightness = 255 - (int)(255 * Math.Sqrt(((a / camera.step) - ((camera.view_angle / 2) / camera.step)) * ((a / camera.step) - ((camera.view_angle / 2) / camera.step)) * 0.5 + ((camera.screen_height / 8) - j) * ((camera.screen_height / 8) - j) * 1.5) / camera.depth);
                        //    if (floor_brightness < 0)
                        //        floor_brightness = 0;
                        //    g.FillRectangle(new SolidBrush(Color.FromArgb(255, floor_brightness, floor_brightness, floor_brightness)), 200 + (2 * a / camera.step), 10 + camera.screen_height / 2 + 8 * j, 2, 8);
                        //}

                            g.FillRectangle(new SolidBrush(Color.FromArgb(255, brightness, brightness, brightness)), 200 + (3 * a / camera.step), 10 + camera.screen_height / 2 - h / 2, 3, h);
                        g.FillRectangle(new SolidBrush(Color.Red), cp[nMin, 0] - 2, cp[nMin, 1] - 2, 4, 4);
                    }
                a += camera.step;
            }
        }

        private static void Swap(double a1, double a2)
        {
            var a3 = a1;
            a1 = a2;
            a2 = a3;
        }

        private static void Swap(bool a1, bool a2)
        {
            var a3 = a1;
            a1 = a2;
            a2 = a3;
        }

        private static bool Crossing(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, float cx, float cy)
        {
            float k1, k2;

            if (x1 >= x2)
            {
                Swap(x1, x2);
                Swap(y1, y2);
            }

            if (x3 >= x4)
            {
                Swap(x3, x4);
                Swap(y3, y4);
            }

            if (y1 == y2)
            {
                k1 = 0;
            }
            else
            {
                k1 = (y2 - y1) / (x2 - x1);
            }

            if (y3 == y4)
            {
                k2 = 0;
            }
            else
            {
                k2 = (y4 - y3) / (x4 - x3);
            }

            if (k1 == k2)
            {
                return false;
            }
            else
            {
                var b1 = y1 - k1 * x1;
                var b2 = y3 - k2 * x3;

                var x = 0f;
                var y = 0f;

                if(float.IsInfinity(k2))
                {
                    x = x3;
                    y = k1 * x + b1;
                    if(y3 < y4)
                    {
                        if (y < y3 || y > y4)
                            return false;
                    }
                    else if(y3 > y4)
                    {
                        if (y < y4 || y > y3)
                            return false;
                    }
                }
                else
                {
                    x = (b2 - b1) / (k1 - k2);
                    y = k1 * x + b1;
                }

                if (Math.Round((x - x1) * (y2 - y1) - (y - y1) * (x2 - x1)) == 0 && ((x1 <= x && x <= x2) || (x2 <= x && x <= x1)) && Math.Round((x - x3) * (y4 - y3) - (y - y3) * (x4 - x3)) == 0 && ((x3 <= x && x <= x4) || (x4 <= x && x <= x3)))
                {
                    xc = x;
                    yc = y;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
