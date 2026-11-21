
using System;
using System.Windows.Forms;
using WarehouseManagementSystem.Controllers;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Views
{
    public partial class ProductForm : Form
    {
        private readonly ProductController controller =
            new ProductController();

        private int selectedProductID = 0;

        public ProductForm()
        {
            InitializeComponent();

            Load += ProductForm_Load;

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            btnSearch.Click += btnSearch_Click;

            dgvProducts.CellClick += dgvProducts_CellClick;
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            try
            {
                cmbCategory.DataSource = controller.GetCategories();
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryID";
                cmbCategory.SelectedIndex = -1;

                LoadProducts();
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load products or categories:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadProducts(string search = "")
        {
            dgvProducts.DataSource = null;
            dgvProducts.DataSource = controller.GetProducts(search);

            if (dgvProducts.Columns["TotalValue"] != null)
            {
                dgvProducts.Columns["TotalValue"].HeaderText =
                    "Stock Value";
            }

            if (dgvProducts.Columns["ProductID"] != null)
                dgvProducts.Columns["ProductID"].HeaderText = "Product ID";

            if (dgvProducts.Columns["ProductName"] != null)
                dgvProducts.Columns["ProductName"].HeaderText = "Product Name";

            if (dgvProducts.Columns["CategoryName"] != null)
                dgvProducts.Columns["CategoryName"].HeaderText = "Category";

            if (dgvProducts.Columns["UnitPrice"] != null)
                dgvProducts.Columns["UnitPrice"].DefaultCellStyle.Format = "N2";
        }

        private Product ReadInputs()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
                throw new ArgumentException("Enter a product name.");

            return new Product
            {
                ProductID = selectedProductID,
                ProductName = txtProductName.Text.Trim(),

                CategoryID = cmbCategory.SelectedValue == null
                    ? (int?)null
                    : Convert.ToInt32(cmbCategory.SelectedValue),

                Quantity = Convert.ToInt32(numQuantity.Value),
                UnitPrice = numUnitPrice.Value,
                ReorderLevel = Convert.ToInt32(numReorderLevel.Value)
            };
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Product product = ReadInputs();
                controller.AddProduct(product);

                MessageBox.Show("Product added successfully!");

                LoadProducts(txtSearch.Text.Trim());
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Cannot Add Product",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedProductID <= 0)
                {
                    MessageBox.Show("Select a product from the table first.");
                    return;
                }

                Product product = ReadInputs();
                controller.UpdateProduct(product);

                MessageBox.Show("Product updated successfully!");

                LoadProducts(txtSearch.Text.Trim());
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Cannot Update Product",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedProductID <= 0)
            {
                MessageBox.Show("Select a product to delete.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this product?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                controller.DeleteProduct(selectedProductID);

                MessageBox.Show("Product deleted successfully!");

                LoadProducts(txtSearch.Text.Trim());
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Cannot Delete Product",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadProducts(txtSearch.Text.Trim());
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed:\n" + ex.Message);
            }
        }

        private void dgvProducts_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvProducts.Rows[e.RowIndex];

            selectedProductID = Convert.ToInt32(
                row.Cells["ProductID"].Value);

            txtProductName.Text =
                Convert.ToString(row.Cells["ProductName"].Value);

            numQuantity.Value = Convert.ToDecimal(
                row.Cells["Quantity"].Value);

            numUnitPrice.Value = Convert.ToDecimal(
                row.Cells["UnitPrice"].Value);

            numReorderLevel.Value = Convert.ToDecimal(
                row.Cells["ReorderLevel"].Value);

            object categoryID = row.Cells["CategoryID"].Value;

            if (categoryID == null || categoryID == DBNull.Value)
                cmbCategory.SelectedIndex = -1;
            else
                cmbCategory.SelectedValue = Convert.ToInt32(categoryID);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void ClearInputs()
        {
            selectedProductID = 0;
            txtProductName.Clear();
            cmbCategory.SelectedIndex = -1;
            numQuantity.Value = 0;
            numUnitPrice.Value = 0;
            numReorderLevel.Value = 10;
            dgvProducts.ClearSelection();
    
        
        }

        private void label5_Click(object sender, EventArgs e)
        {
            // No action required for this label.
        }

        private void label1_Click_1(object sender, EventArgs e)
        {
            // No action required for this label.
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {

        }
    }

}
