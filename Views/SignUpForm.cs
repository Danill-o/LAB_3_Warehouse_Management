using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseManagementSystem.Controllers;

namespace WarehouseManagementSystem.Views
{
    public partial class SignUpForm : Form
    {
        public SignUpForm()
        {
            InitializeComponent();
        }

        private readonly AuthController authController = new AuthController();

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void SignUpForm_Load(object sender, EventArgs e)
        {

        }

        private void lnkBackToLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            this.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string message;

            bool success = authController.Register(
                txtFullName.Text,
                txtUsername.Text,
                txtEmail.Text,
                txtPassword.Text,
                txtConfirmPassword.Text,
                out message
            );

            if (success)
            {
                MessageBox.Show(
                    message,
                    "Registration Successful",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.Close();
            }
            else
            {
                MessageBox.Show(
                    message,
                    "Registration Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}
        
    

