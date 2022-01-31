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
    public partial class txtency : Form
    {
        public txtency()
        {
            InitializeComponent();
        }

        public enum State
        {
            Hiding,         //  Defines what state the 
            Zero_Filling    //  application is currently in.
        };

        OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
        Bitmap bmap;
        String sText;

        private void button1_Click(object sender, EventArgs e)
        {
            // File Open Dialog box details
            OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            OpenFileDialog1.Filter = "Image Files( *.bmp; *.jpeg; *.png *.jpg;) |*.bmp; *.jpeg; *.png; *.jpg";
            OpenFileDialog1.Title = "Choose Cover Image";
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmap = new Bitmap(OpenFileDialog1.FileName); //opening image as Bitmap
                pictureBox1.Image = bmap;
                pictureBox1.Show();
                textBox1.Text = OpenFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == String.Empty || richTextBox1.Text == String.Empty || textBox2.Text == String.Empty) // If image is not selected or text not input, show error
            {
                MessageBox.Show("Fill all the neccessary fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox2.Text.Length < 6)
            {
                MessageBox.Show("Secret Key must be 6 characters at least", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Text = String.Empty;
                return;
            }
            progressBar1.Visible = true;
            progressBar1.Minimum = 0;

            sText = Crypto.EncryptStringAES(richTextBox1.Text, textBox2.Text.ToString());
            progressBar1.Maximum = sText.Length;
            Bitmap bmpNew = encryption(bmap, sText); // call encryption function
            string saveFile = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFile += "\\Output.bmp";
            bmpNew.Save(saveFile, ImageFormat.Bmp); //save the new image file as output.png in Desktop
            MessageBox.Show("Encryption Completed. Image saved to Desktop", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private Bitmap encryption(Bitmap bmp, String str)
        {
            State st = State.Hiding; // Hiding chars in image
            int charIndex = 0, charValue = 0;
            long pixelElementIndex = 0; //index of pixel being processed
            int zeros = 0; // no of zeros at the finishing
            int R = 0, G = 0, B = 0; // to hold pixel element

            for (int i = 0; i < bmp.Height; i++) // iterate through rows
            {
                for (int j = 0; j < bmp.Width; j++) // iterate in each row
                {
                    Color pixel = bmp.GetPixel(j, i);  //read pixel at position j,i

                    // progreess bar incrementing
                    progressBar1.Value = charIndex;

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
            }
            return bmp;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            encry en = new encry();
            en.ShowDialog();
            this.Close();
        }
    }
}
