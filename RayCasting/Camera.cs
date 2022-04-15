using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCasting
{
    class Camera
    {
        public float x, y, v, vx, vy, vr, spd = 1.5f, view_angle, angle, vert_angle = 90.0f, step, screen_distance, screen_height, depth;
        public bool[] keys = new bool[4];
        public bool w_key, d_key, s_key, a_key;
        public int mx, my, mxp, myp, m_range, m_vert_range;

        public Camera(float x, float y, float view_angle, float angle, float step, float screen_distance, float screen_height, float depth)
        {
            this.x = x;
            this.y = y;
            this.view_angle = view_angle;
            this.angle = angle;
            this.step = step;
            this.screen_distance = screen_distance;
            this.screen_height = screen_height;
            this.depth = depth;
        }

        public Camera(float x, float y, float view_angle, float step, float screen_distance, float screen_height, float depth)
        {
            this.x = x;
            this.y = y;
            this.view_angle = view_angle;
            angle = 0.0f;
            this.step = step;
            this.screen_distance = screen_distance;
            this.screen_height = screen_height;
            this.depth = depth;
        }

        public void Move()
        {
            GetInput();

            //angle += vr;
            float f_angle = 0;

            if (w_key)
            {
                f_angle = (angle + view_angle / 2) * (float)Math.PI / 180;
            }
            if(d_key)
            {
                f_angle = (angle + 90 + view_angle / 2) * (float)Math.PI / 180;
            }
            if (s_key)
            {
                f_angle = (angle + 180 + view_angle / 2) * (float)Math.PI / 180;
            }
            if (a_key)
            {
                f_angle = (angle + 270 + view_angle / 2) * (float)Math.PI / 180;
            }
            if (w_key && d_key)
            {
                f_angle = (angle + 45 + view_angle / 2) * (float)Math.PI / 180;
            }
            if (d_key && s_key)
            {
                f_angle = (angle + 135 + view_angle / 2) * (float)Math.PI / 180;
            }
            if (s_key && a_key)
            {
                f_angle = (angle + 225 + view_angle / 2) * (float)Math.PI / 180;
            }
            if (a_key && w_key)
            {
                f_angle = (angle + 315 + view_angle / 2) * (float)Math.PI / 180;
            }

            vx = v * (float)Math.Cos(f_angle);
            vy = v * (float)Math.Sin(f_angle);

            x += vx;
            y += vy;
        }

        private void GetInput()
        {
            if (!w_key && !d_key && !s_key && !a_key)
            {
                vr = 0;
                v = 0;
            }
            else
            {
                v = spd;
            }
            if (w_key && s_key)
                v = 0;
            if (a_key && d_key)
                v = 0;

            if(mx > mxp)
            {
                angle += (mx - mxp) / (m_range / 360) * 1f;
            }
            else if(mx < mxp)
            {
                angle -= (mxp - mx) / (m_range / 360) * 1f;
            }

            if (my > myp)
            {
                vert_angle -= (my - myp) / (m_vert_range / 180) * 1f;
            }
            else if (my < myp)
            {
                vert_angle += (myp - my) / (m_vert_range / 180) * 1f;
            }

            mxp = mx;
            myp = my;
        }
    }
}
