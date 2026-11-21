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
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Views
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private readonly AuthController authController = new AuthController();
        private void lnkCreateAccount_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (SignUpForm signUp = new SignUpForm())
            {
                this.Hide();
                signUp.ShowDialog();
                this.Show();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Enter your username and password.");
                return;
            }

            AuthUser user = authController.Login(username, password);

            if (user == null)
            {
                MessageBox.Show(
                    "Invalid username or password, or the database is unavailable.",
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            MessageBox.Show(
                "Welcome, " + user.FullName + "!",
                "Login Successful",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            this.Hide();

            using (MainDashboard dashboard = new MainDashboard())
            {
                dashboard.ShowDialog();
            }

            this.Close();
        }
        

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
