using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseManagementSystem.Views;

namespace WarehouseManagementSystem.Views
{
    public partial class MainDashboard : Form
    {
        public MainDashboard()
        {
            InitializeComponent();
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
        // Add this method inside the MainDashboard class.
        private void btnProducts_Click(object sender, EventArgs e)
        {
            using (ProductForm productForm = new ProductForm())
            {
                using (ProductForm form = new ProductForm())
                {
                    form.ShowDialog();
                }
            }
        }
            

        private void btnCategories_Click(object sender, EventArgs e)
        {
            using (CategoryForm form = new CategoryForm())
            {
                form.ShowDialog();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            { 
            this.Close();
        }
        }


        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            SupplierForm supplierForm = new SupplierForm();
            supplierForm.ShowDialog();
        }


        private void btnStock_Click(object sender, EventArgs e)
        {
            StockForm stockForm = new StockForm();
            stockForm.ShowDialog();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reports will be added next.");
        }

        private void MainDashboard_Load(object sender, EventArgs e)
        {

        }
    }
}
