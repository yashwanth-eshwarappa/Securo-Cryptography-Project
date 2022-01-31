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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool pass = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (Application.UserAppDataRegistry.GetValue("username") != null && Application.UserAppDataRegistry.GetValue("passwd") != null)
                {
                    pass = true;
                    label1.Text = "Enter the Saved Credentials";
                    button1.Text = "Login";
                }
                else
                {
                    label1.Visible = true;
                    pass = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (pass)
            {
                if (Application.UserAppDataRegistry.GetValue("username").ToString() == textBox1.Text && Application.UserAppDataRegistry.GetValue("passwd").ToString() == textBox2.Text)
                {
                    Hide();
                    encry enc = new encry();
                    enc.ShowDialog();
                    Close();
                }
                else
                {
                    MessageBox.Show("Wrong UserName or Password");
                    textBox2.Text = String.Empty;
                }
            }
            else if (textBox1.Text == String.Empty || textBox2.Text == String.Empty)
            {
                MessageBox.Show("Fill All The Fields", "Incomplete Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Application.UserAppDataRegistry.SetValue("username", textBox1.Text);
                Application.UserAppDataRegistry.SetValue("passwd", textBox2.Text);
                MessageBox.Show("Login Credentials Set, use this Credentials from now to Login\nRe-Open the applicaion to use", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
