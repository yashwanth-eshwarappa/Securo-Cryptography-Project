using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace encryptDecryptInterface
{
    public partial class imgency : Form
    {
        public imgency()
        {
            InitializeComponent();
        }

        public enum State
        {
            Hiding,
            Zero_Filling
        };

        OpenFileDialog open1 = new OpenFileDialog();
        OpenFileDialog open2 = new OpenFileDialog();
        Bitmap bmp1, bmp2;
        String iText;

        private void button2_Click(object sender, EventArgs e)
        {
            open2.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            open2.Filter = "Image Files( *.bmp; *.jpeg; *.png *.jpg;) |*.bmp; *.jpeg; *.png; *.jpg";
            open2.Title = "Choose Secret Image";
            if (open2.ShowDialog() == DialogResult.OK)
            {
                bmp2 = new Bitmap(open2.FileName);
                textBox2.Text = open2.FileName;
                pictureBox2.Image = bmp2;
                pictureBox2.Show();
            }
            FileInfo fi = new FileInfo(open2.FileName);
            if (bmp1.Width * bmp1.Height / 4 < fi.Length)
            {
                MessageBox.Show("Secret Image size is large", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pictureBox2.Visible = false;
                textBox2.Text = String.Empty;
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == String.Empty || textBox2.Text == String.Empty || textBox3.Text == String.Empty)
            {
                MessageBox.Show("Fill all the Fields properly", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox3.Text.Length < 6)
            {
                MessageBox.Show("Secret Key must be 6 character in length", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            progressBar1.Visible = true;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = bmp2.Width * bmp2.Height;

            // reading image begins here.
            for (int i = 0; i < bmp2.Height; i++)
            {
                for (int j = 0; j < bmp2.Width; j++)
                {
                    Color pix = bmp2.GetPixel(j, i);
                    String abc = pix.R.ToString("000") + pix.G.ToString("000") + pix.B.ToString("000");
                    iText += abc;
                    progressBar1.Value = i * bmp2.Height + j;
                }
                progressBar1.Value = progressBar1.Maximum;
            }

            iText = Crypto.EncryptStringAES(iText, textBox3.Text.ToString());
            bmp1 = hwwrite(bmp1, bmp2.Height, bmp2.Width);
            Bitmap bmp3 = encryption(bmp1, iText);
            string savef = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            savef += "\\output.bmp";
            bmp3.Save(savef, ImageFormat.Bmp);
            MessageBox.Show("Encryption Complete", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            open1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            open1.Filter = "Image Files( *.bmp; *.jpeg; *.png *.jpg;) |*.bmp; *.jpeg; *.png; *.jpg";
            open1.Title = "Choose Cover Image";
            if (open1.ShowDialog() == DialogResult.OK)
            {
                bmp1 = new Bitmap(open1.FileName);
                textBox1.Text = open1.FileName;
                pictureBox1.Image = bmp1;
                pictureBox1.Show();
            }
        }

        private Bitmap hwwrite(Bitmap bmp, int h, int w)
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

            r = 0; g = 0; b = 0;
            colorIndex = 0;
            for (int j = 3; j < 6; j++)
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
                            r = r + w % 2;
                            w = w / 2;
                            // MessageBox.Show(r.ToString());
                            break;

                        case 1:
                            g = g + w % 2;
                            w = w / 2;
                            // MessageBox.Show(g.ToString());
                            break;

                        case 2:
                            b = b + w % 2;
                            w = w / 2;
                            bmp.SetPixel(j, 0, Color.FromArgb(r, g, b));
                            //MessageBox.Show(b.ToString());
                            break;
                    }
                    colorIndex++;
                }
            }
            return bmp;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            encry en = new encry();
            en.ShowDialog();
            this.Close();
        }

        private Bitmap encryption(Bitmap bmp, String str)
        {
            State st = State.Hiding; // Hiding chars in image
            int charIndex = 0, charValue = 0;
            long pixelElementIndex = 0; //index of pixel being processed
            int zeros = 0; // no of zeros at the finishing
            int R = 0, G = 0, B = 0; // to hold pixel element
            //int kkk = 6;
            for (int i = 0; i < bmp.Height; i++) // iterate through rows
            {
                for (int j = 6; j < bmp.Width; j++) // iterate in each row
                {
                    Color pixel = bmp.GetPixel(j, i);  //read pixel at position j,i

                    R = pixel.R - pixel.R % 2;  // clearing least 
                    G = pixel.G - pixel.G % 2;  // significant bit in
                    B = pixel.B - pixel.B % 2;  // each pixel

                    for (int n = 0; n < 3; n++)
                    {
                        if (pixelElementIndex % 8 == 0)  // if 8 bits are processed.
                        {
                            if (st == State.Zero_Filling && zeros == 8) // check if the everything is done
                            {
                                if ((pixelElementIndex - 1) % 3 < 2)  // apply the last pixel ( if only a part of it is affected )
                                {
                                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                return bmp;
                            }
                            if (charIndex >= str.Length)
                                st = State.Zero_Filling;
                            else
                                charValue = str[charIndex++];

                        }

                        switch (pixelElementIndex % 3)  // check which pixel element has to hide a bit in its LSB
                        {
                            case 0:
                                {
                                    if (st == State.Hiding)
                                    {
                                        // adding the rightmost bit.
                                        R += charValue % 2; //since lsb of each byte is cleared , adding it will work
                                        charValue /= 2; // since rightmost bit is added, remove it and move to next one
                                    }
                                }
                                break;
                            case 1:
                                {
                                    if (st == State.Hiding)
                                    {
                                        G += charValue % 2;
                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (st == State.Hiding)
                                    {
                                        B += charValue % 2;
                                        charValue /= 2;
                                    }
                                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                break;
                        }

                        pixelElementIndex++;
                        if (st == State.Zero_Filling)  // increment the value of zeros until it is 8
                            zeros++;
                    }
                }
                //kkk = 0;
            }
            return bmp;
        }

    }
}
