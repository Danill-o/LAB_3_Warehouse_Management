using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseManagementSystem.Data;

namespace WarehouseManagementSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void TestDatabaseConnection()
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    MessageBox.Show(
                        "Database connected successfully!",
                        "Connection Test",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database connection failed:\n" + ex.Message,
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            TestDatabaseConnection();
        }
        private void btnOpenProducts_Click(object sender, EventArgs e)
        {
            using (var form = new WarehouseManagementSystem.Views.ProductForm())
            {
                form.ShowDialog();
            }
        }
    }
}
