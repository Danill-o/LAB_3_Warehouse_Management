
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WarehouseManagementSystem.Controllers;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Views
{
    public partial class StockForm : Form
    {
        private readonly StockController controller = new StockController();

        public StockForm()
        {
            InitializeComponent();

            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProduct.DisplayMember = "ProductName";
            cmbProduct.ValueMember = "ProductID";

            cmbTransactionType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTransactionType.Items.Clear();
            cmbTransactionType.Items.Add("Stock In");
            cmbTransactionType.Items.Add("Stock Out");

            cmbProduct.SelectedIndexChanged += cmbProduct_SelectedIndexChanged;
            btnRecord.Click += btnRecord_Click;

            LoadStockData();
        }

        private void LoadStockData()
        {
            try
            {
                cmbProduct.DataSource = controller.GetProducts();

                dgvTransactions.DataSource = null;
                dgvTransactions.DataSource = controller.GetTransactions();

                dgvTransactions.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;
                dgvTransactions.ReadOnly = true;
                dgvTransactions.AllowUserToAddRows = false;

                if (cmbProduct.Items.Count > 0)
                {
                    cmbProduct.SelectedIndex = 0;
                }
                else
                {
                    txtCurrentStock.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load stock data:\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedItem is Product product)
            {
                txtCurrentStock.Text = product.Quantity.ToString();
            }
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedValue == null ||
                cmbTransactionType.SelectedItem == null)
            {
                MessageBox.Show("Select a product and transaction type.");
                return;
            }

            int productID = Convert.ToInt32(cmbProduct.SelectedValue);
            string type = cmbTransactionType.SelectedItem.ToString();
            int quantity = Convert.ToInt32(numQuantity.Value);
            string notes = txtNotes.Text.Trim();

            string message;
            bool success = controller.RecordStock(
                productID, type, quantity, notes, out message);

            MessageBox.Show(message);

            if (success)
            {
                txtNotes.Clear();
                numQuantity.Value = 1;
                LoadStockData();
            }
        }



private void lblSubTitle_Click(object sender, EventArgs e)
        {
            // Subtitle label click event.
        }

        private void StockForm_Load(object sender, EventArgs e)
        {
            // Form loading event.
        }

    }
}

