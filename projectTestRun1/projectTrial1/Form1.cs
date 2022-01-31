using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace projectTrial1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
        Bitmap bmap;
        String sText;

        private void button1_Click(object sender, EventArgs e)
        {
            // File Open Dialog box details
            OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            OpenFileDialog1.Filter = "Image Files( *.bmp; *.jpeg; *.png *.jpg;) |*.bmp; *.jpeg; *.png; *.jpg";
            OpenFileDialog1.Title = "Choose Cover Image";
            if(OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(OpenFileDialog1.FileName); //opening image as Bitmap
                int he = bmp.Height, wei = 0;
                if (bmp.Width % 2 == 0) wei = bmp.Width + 1;
                bmap = new Bitmap(bmp, wei, he);
                pictureBox1.Image = bmap;
                pictureBox1.Show();
                textBox1.Text = OpenFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == String.Empty || richTextBox1.Text == String.Empty || textBox2.Text == String.Empty) // If image is not selected or text not input, show error
            {
                MessageBox.Show("Fill all the neccessary fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(textBox2.Text.Length < 6)
            {
                MessageBox.Show("Secret Key must be 6 characters at least", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Text = String.Empty;
                return;
            }

            sText = Crypto.EncryptStringAES(richTextBox1.Text,textBox2.Text.ToString());
            Bitmap bmpNew = lWrite(bmap, sText.Length);
            bmpNew = encryption(bmpNew, sText); // call encryption function
            string saveFile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFile += "\\Output.bmp";
            bmpNew.Save(saveFile,ImageFormat.Bmp); //save the new image file as output.png in Desktop
            MessageBox.Show("Encryption Completed. Image saved to Desktop", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private  Bitmap lWrite(Bitmap bmp,int h)
        {
            int r = 0, g = 0, b = 0;
            int colorIndex = 0;

            for (int j = 0; j < 3; j++)
            {
                Color pix = bmp.GetPixel(j, 0);

                r = pix.R - pix.R % 2; //MessageBox.Show(pix.R.ToString());
                g = pix.G - pix.G % 2; //MessageBox.Show(pix.G.ToString());
                b = pix.B - pix.B % 2; //MessageBox.Show(pix.B.ToString());

                for (int n = 0; n < 3; n++)
                {
                    switch (colorIndex % 3)
                    {
                        case 0:
                            r = r + h % 2;
                            h = h / 2;
                            // MessageBox.Show(r.ToString());
                            break;

                        case 1:
                            g = g + h % 2;
                            h = h / 2;
                            // MessageBox.Show(g.ToString());
                            break;

                        case 2:
                            b = b + h % 2;
                            h = h / 2;
                            bmp.SetPixel(j, 0, Color.FromArgb(r, g, b));
                            // MessageBox.Show(b.ToString());
                            break;
                    }
                    colorIndex++;
                }
            }
            return bmp;
        }

        private Bitmap encryption(Bitmap bmp, String str)
        {
            int charIndex = 0, charValue = 0;
            int R = 0, G = 0, B = 0; // to hold pixel element
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 3; j < bmp.Width; j = j + 2)
                {
                    if (charIndex == str.Length)
                    {
                        return bmp;
                    }
                    if (charIndex < str.Length)
                    {
                        charValue = str[charIndex++];
                    }
                   
                    Color pix = bmp.GetPixel(j, i);

                    R = pix.R - pix.R % 4; R = R + charValue % 4; charValue /= 4;
                    G = pix.G - pix.G % 2; G = G + charValue % 2; charValue /= 2;
                    B = pix.B - pix.B % 2; B = B + charValue % 2; charValue /= 2;
                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));

                    pix = bmp.GetPixel(j + 1, i);

                    R = pix.R - pix.R % 4; R = R + charValue % 4; charValue /= 4;
                    G = pix.G - pix.G % 2; G = G + charValue % 2; charValue /= 2;
                    B = pix.B - pix.B % 2; B = B + charValue % 2; charValue /= 2;
                    bmp.SetPixel(j + 1, i, Color.FromArgb(R, G, B));

                }
            }
            return bmp;
        }
    }
}
