using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace encryptDecryptInterface
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            Form1 f = new Form1();
            f.ShowDialog();
            this.Close();
        }

        private void Welcome_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.a;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            Timer t = new Timer();
            t.Interval = 1200;
            t.Tick += new EventHandler(Change_Image);
            t.Start();
        }
        private void Change_Image(object sender, EventArgs e)
        {
            List<Bitmap> b1 = new List<Bitmap>();

            b1.Add(Properties.Resources.a);
            b1.Add(Properties.Resources.g);
            b1.Add(Properties.Resources.b);
            b1.Add(Properties.Resources.d);
            b1.Add(Properties.Resources.f);
            b1.Add(Properties.Resources.c);
            b1.Add(Properties.Resources.e);

            int index = DateTime.Now.Second % b1.Count;
            pictureBox1.Image = b1[index];
        }
    }
}
