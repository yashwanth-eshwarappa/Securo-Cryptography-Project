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

namespace encryptDecryptInterface
{
    public partial class imgdecy : Form
    {
        public imgdecy()
        {
            InitializeComponent();
        }

        OpenFileDialog op1 = new OpenFileDialog();
        Bitmap bmp1;
        int hei = 0, wid = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            op1.Title = "Select Image";
            op1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            op1.Filter = "Image Files(*.jpeg; *.bmp; *.jpg; *.png;) | *.jpeg; *.bmp; *.jpg; *.png";
            if (op1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = op1.FileName;
                bmp1 = new Bitmap(op1.FileName);
                pictureBox1.Image = bmp1;
            }
        }


        private int hRead(Bitmap bmp)
        {
            int colorIndex = 0;
            int value = 0;
            for (int j = 0; j < 3; j++)
            {
                Color p = bmp.GetPixel(j, 0);

                for (int n = 0; n < 3; n++)
                {
                    switch (colorIndex % 3)
                    {
                        case 0:
                            {
                                value = value * 2 + p.R % 2;
                            }
                            break;

                        case 1:
                            {
                                value = value * 2 + p.G % 2;
                            }
                            break;

                        case 2:
                            {
                                value = value * 2 + p.B % 2;
                            }
                            break;
                    }
                    colorIndex++;
                    if (colorIndex % 8 == 0)
                    {
                        int val = revBits(value);
                        return val;
                    }
                }
            }
            return 0;
        }

        private int wRead(Bitmap bmp)
        {
            int colorIndex = 0;
            int value = 0;
            for (int j = 3; j < 6; j++)
            {
                Color p = bmp.GetPixel(j, 0);

                for (int n = 0; n < 3; n++)
                {
                    switch (colorIndex % 3)
                    {
                        case 0:
                            {
                                value = (value * 2) + (p.R % 2);
                            }
                            break;

                        case 1:
                            {
                                value = (value * 2) + (p.G % 2);
                            }
                            break;

                        case 2:
                            {
                                value = (value * 2) + (p.B % 2);
                            }
                            break;
                    }
                    colorIndex++;
                    if (colorIndex % 8 == 0)
                    {
                        int val = revBits(value);
                        return val;
                    }
                }
            }
            return 0;
        }

        private string decrypt(Bitmap bmp)
        {
            int colorUnitIndex = 0, charValue = 0;
            string eText = String.Empty; // to store secret ext from ing
            //int kkk = 6;


            for (int i = 0; i < bmp.Height; i++)  // iterate rows
            {
                for (int j = 6; j < bmp.Width; j++)  // iterate in each row
                {
                    Color pixel = bmp.GetPixel(j, i);

                    for (int n = 0; n < 3; n++)
                    {
                        switch (colorUnitIndex % 3)
                        {
                            case 0:
                                {
                                    charValue = charValue * 2 + pixel.R % 2; // append the bit value to charValue
                                }
                                break;
                            case 1:
                                {
                                    charValue = charValue * 2 + pixel.G % 2;
                                }
                                break;
                            case 2:
                                {
                                    charValue = charValue * 2 + pixel.B % 2;
                                }
                                break;
                        }
                        colorUnitIndex++;

                        if (colorUnitIndex % 8 == 0)
                        {
                            charValue = revBits(charValue);

                            if (charValue == 0)
                            {
                                return eText;
                            }

                            char c = (char)charValue; // conversion from int to char

                            eText += c.ToString();
                        }
                    }
                }

            }
            return eText;
        }

        private int revBits(int n)
        {
            int result = 0;

            for (int i = 0; i < 8; i++)
            {
                result = result * 2 + n % 2;

                n /= 2;
            }

            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            if (textBox1.Text == String.Empty || textBox2.Text == String.Empty)
            {
                MessageBox.Show("All fields are mandatory", "Incomplete information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox2.Text.Length < 6)
            {
                MessageBox.Show("Secret Key must be 6 characters in length", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            hei = hRead(bmp1);
            wid = wRead(bmp1);

            int flag1 = 1;
            String secImg = decrypt(bmp1);
            try
            {
                secImg = Crypto.DecryptStringAES(secImg, textBox2.Text.ToString());
            }
            catch
            {
                MessageBox.Show("Wrong Password");
                flag1 = 0;
            }
            if (flag1 == 1)
            {
                Bitmap bmpSec = new Bitmap(wid, hei);
                bmpSec = secImgRet(bmpSec, secImg, wid, hei);
                pictureBox2.Image = bmpSec;
                String secsave = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                secsave += "\\secImage.bmp";
                bmpSec.Save(secsave, ImageFormat.Bmp);

                label1.Text = "Decryption successful!\nThe secret image is:";
                label1.Visible = true;

                MessageBox.Show("Secret Image Stored to Desktop", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            encry en = new encry();
            en.ShowDialog();
            this.Close();
        }

        private Bitmap secImgRet(Bitmap bmp, string stext, int wid, int hei)
        {
            int m = 0;
            for (int i = 0; i < hei; i++)
            {
                for (int j = 0; j < wid; j++)
                {
                    int r = 0, g = 0, b = 0;
                    if (m < stext.Length)
                    {
                        for (int buf = 0; buf < 3; buf++)
                        {
                            r = r * 10 + stext[m++] - '0';
                        }
                        for (int buf = 0; buf < 3; buf++)
                        {
                            g = g * 10 + stext[m++] - '0';
                        }
                        for (int buf = 0; buf < 3; buf++)
                        {
                            b = b * 10 + stext[m++] - '0';
                        }
                    }
                    bmp.SetPixel(j, i, Color.FromArgb(r, g, b));
                }
            }
            return bmp;
        }

    }
}
