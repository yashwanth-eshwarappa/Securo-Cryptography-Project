using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projectTrial2
{
    public partial class Form1 : Form
    {
        public Form1()
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
            OpenFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            OpenFileDialog1.Title = "Choose Image to Decrypt";
            OpenFileDialog1.Filter = "Image Files(*.jpeg; *.bmp; *.jpg; *.png;) | *.jpeg; *.bmp; *.jpg; *.png";
            if(OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bmap = new Bitmap(OpenFileDialog1.FileName);
                textBox1.Text = OpenFileDialog1.FileName;
                pictureBox1.Image = bmap;
                pictureBox1.Show();
            }
        }
        int test = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == String.Empty)
            {
                MessageBox.Show("Select an Image to Decrypt", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            int len = lRead(bmap);
            sText = decrypt(bmap,len);
            try
            {
                sText = Crypto.DecryptStringAES(sText, textBox2.Text.ToString());
            }
            catch
            {
                test += 1;
                if (test < 3)
                {
                    richTextBox1.Text = String.Empty;

                    MessageBox.Show("Wrong Passowrd" +"\n" + (3 - test) + " tries left!!", "Error" , MessageBoxButtons.OK, MessageBoxIcon.Error );
                    
                    textBox2.Text = String.Empty;

                    
                    return;
                }
                else
                {
                    test = 0;
                    this.Close();
                }
            }
            richTextBox1.Text = sText;
            richTextBox1.Show();
            MessageBox.Show("Decryption Completed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            textBox2.Text = string.Empty;
        }

        private int lRead(Bitmap bmp)
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

        private string decrypt(Bitmap bmp, int len)
        {
            int charValue = 0;
            string eText = String.Empty; // to store secret ext from ing
            int chcount = 0;

            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 3; j < bmp.Width; j = j + 2)
                {
                    
                    if (chcount < len)
                    {

                        Color pix = bmp.GetPixel(j, i);
                        int R = revB(pix.R % 4);
                        charValue = charValue * 4 + R;
                        charValue = charValue * 2 + pix.G % 2;
                        charValue = charValue * 2 + pix.B % 2;


                        pix = bmp.GetPixel(j + 1, i);
                        R = revB(pix.R % 4);
                        charValue = charValue * 4 + R;
                        charValue = charValue * 2 + pix.G % 2;
                        charValue = charValue * 2 + pix.B % 2;

                        charValue = revBits(charValue);
                        char c = (char)charValue;
                        //MessageBox.Show(c.ToString());
                        eText += c.ToString();
                        chcount++;
                    }
                    else
                    {
                        return eText;
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

        private int revB(int n)
        {
            int r = 0;
            for(int i = 0; i < 2; i++)
            {
                r = r * 2 + n % 2;
                n = n / 2;
            }
            return r;
        }
    }
}
