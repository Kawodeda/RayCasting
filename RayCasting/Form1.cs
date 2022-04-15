using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace RayCasting
{
    public partial class Form1 : Form
    {
        Section o1 = new Section(0, 0, 100, 0);
        Section o2 = new Section(100, 0, 100, 50);
        Section o3 = new Section(100, 50, 150, 50);
        Section o4 = new Section(150, 50, 150, 150);
        Section o5 = new Section(150, 150, 50, 150);
        Section o6 = new Section(50, 150, 50, 100);
        Section o7 = new Section(50, 100, 0, 100);
        Section o8 = new Section(0, 100, 0, 0);
        Section i1 = new Section(25, 25, 25, 100);
        Section i2 = new Section(50, 0, 50, 50);
        Section i3 = new Section(50, 50, 75, 50);
        Section i4 = new Section(75, 50, 75, 25);
        Section i5 = new Section(50, 75, 125, 75);
        Section i6 = new Section(125, 75, 125, 125);
        Section i7 = new Section(125, 125, 75, 125);
        Section i8 = new Section(75, 125, 75, 100);
        Section i9 = new Section(75, 100, 100, 100);
        Camera c = new Camera(80, 140, 90, 315f, 0.4f, 10, 300, 100);
        Bitmap bmp;
        Graphics g;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Clip = new Rectangle(0, 1, this.Width, this.Height);
            System.Windows.Forms.Cursor.Hide();
            this.SetBounds(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (bmp != null)
                bmp.Dispose();
            if (g != null)
                g.Dispose();

            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);

            c.w_key = Keyboard.IsKeyDown(Key.W);
            c.d_key = Keyboard.IsKeyDown(Key.D);
            c.s_key = Keyboard.IsKeyDown(Key.S);
            c.a_key = Keyboard.IsKeyDown(Key.A);

            if (System.Windows.Forms.Cursor.Position.X >= Size.Width - 1)
            {
                Point p = new Point(1, System.Windows.Forms.Cursor.Position.Y);
                System.Windows.Forms.Cursor.Position = p;
                c.mx = System.Windows.Forms.Cursor.Position.X;
                c.mxp = System.Windows.Forms.Cursor.Position.X;
            }
            if (System.Windows.Forms.Cursor.Position.X <= 0)
            {
                Point p = new Point(Size.Width - 1, System.Windows.Forms.Cursor.Position.Y);
                System.Windows.Forms.Cursor.Position = p;
                c.mx = System.Windows.Forms.Cursor.Position.X;
                c.mxp = System.Windows.Forms.Cursor.Position.X;
            }

            c.mx = System.Windows.Forms.Cursor.Position.X;
            c.my = System.Windows.Forms.Cursor.Position.Y;

            c.m_range = pictureBox1.Width;
            c.m_vert_range = pictureBox1.Height;
            c.Move();

            RayCasting.Do(g, new Section[17] { o1, o2, o3, o4, o5, o6, o7, o8, i1, i2, i3, i4, i5, i6, i7, i8, i9 }, c);

            pictureBox1.Image = bmp;
        }

        private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

        }

        private void Form1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {

        }
    }
}
