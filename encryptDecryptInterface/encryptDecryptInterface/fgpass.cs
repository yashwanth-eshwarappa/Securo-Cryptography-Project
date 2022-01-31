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
    public partial class fgpass : Form
    {
        public fgpass()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            bool pass = false;
            if (Application.UserAppDataRegistry.GetValue("passwd").ToString() == textBox1.Text)
            {
                pass = true;
            }

            if (textBox1.Text == String.Empty || textBox2.Text == String.Empty || textBox3.Text == String.Empty)
            {
                MessageBox.Show("Fill All Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (pass)
            {

                if (textBox2.Text == textBox1.Text)
                {
                    MessageBox.Show("Old and New Password CANNOT be same", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Text = String.Empty; textBox2.Text = String.Empty; textBox3.Text = String.Empty;
                }

                if (textBox2.Text != textBox3.Text)
                {
                    textBox2.Text = String.Empty;
                    textBox3.Text = String.Empty;
                    MessageBox.Show("New Password fields do not match\nRe-Enter New Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (textBox2.Text == textBox3.Text && textBox2.Text != String.Empty)
                {
                    Application.UserAppDataRegistry.SetValue("passwd", textBox2.Text.ToString());
                    MessageBox.Show("Password Changed");
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Input the proper Old Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = String.Empty; textBox2.Text = String.Empty; textBox3.Text = String.Empty;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void fgpass_Load(object sender, EventArgs e)
        {
            label1.Text = Application.UserAppDataRegistry.GetValue("username").ToString();
        }

    }
}
