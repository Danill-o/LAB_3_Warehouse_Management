
using System;
using System.Windows.Forms;
using WarehouseManagementSystem.Controllers;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Views
{
    public partial class SupplierForm : Form
    {
        private readonly SupplierController controller =
            new SupplierController();

        private int selectedSupplierID = 0;

        public SupplierForm()
        {
            InitializeComponent();

            // Connect button events.
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            btnSearch.Click += btnSearch_Click;

            // Connect table selection.
            dgvSuppliers.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvSuppliers.MultiSelect = false;
            dgvSuppliers.ReadOnly = true;
            dgvSuppliers.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvSuppliers.SelectionChanged += dgvSuppliers_SelectionChanged;

            LoadSuppliers();
        }

        private void LoadSuppliers(string search = "")
        {
            try
            {
                dgvSuppliers.DataSource = controller.GetSuppliers(search);

                if (dgvSuppliers.Columns["SupplierID"] != null)
                    dgvSuppliers.Columns["SupplierID"].HeaderText =
                        "Supplier ID";

                if (dgvSuppliers.Columns["SupplierName"] != null)
                    dgvSuppliers.Columns["SupplierName"].HeaderText =
                        "Supplier Name";

                if (dgvSuppliers.Columns["ContactPerson"] != null)
                    dgvSuppliers.Columns["ContactPerson"].HeaderText =
                        "Contact Person";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load suppliers.\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private Supplier GetSupplierFromFields()
        {
            return new Supplier
            {
                SupplierID = selectedSupplierID,
                SupplierName = txtSupplierName.Text,
                ContactPerson = txtContactPerson.Text,
                Phone = txtPhone.Text,
                Email = txtEmail.Text,
                Address = txtAddress.Text
            };
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string message;

            if (controller.AddSupplier(
                GetSupplierFromFields(), out message))
            {
                MessageBox.Show(message, "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFields();
                LoadSuppliers();
            }
            else
            {
                MessageBox.Show(message, "Unable to Add",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string message;

            if (controller.UpdateSupplier(
                GetSupplierFromFields(), out message))
            {
                MessageBox.Show(message, "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearFields();
                LoadSuppliers();
            }
            else
            {
                MessageBox.Show(message, "Unable to Update",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedSupplierID <= 0)
            {
                MessageBox.Show("Please select a supplier first.");
                return;
            }

            DialogResult answer = MessageBox.Show(
                "Are you sure you want to delete this supplier?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (answer != DialogResult.Yes)
                return;

            string message;

            if (controller.DeleteSupplier(selectedSupplierID, out message))
            {
                MessageBox.Show(message, "Success");
                ClearFields();
                LoadSuppliers();
            }
            else
            {
                MessageBox.Show(message, "Unable to Delete",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadSuppliers(txtSearch.Text.Trim());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void dgvSuppliers_SelectionChanged(
            object sender, EventArgs e)
        {
            if (dgvSuppliers.CurrentRow == null ||
                dgvSuppliers.CurrentRow.IsNewRow ||
                dgvSuppliers.CurrentRow.DataBoundItem == null)
            {
                return;
            }

            Supplier supplier =
                dgvSuppliers.CurrentRow.DataBoundItem as Supplier;

            if (supplier == null)
                return;

            selectedSupplierID = supplier.SupplierID;
            txtSupplierName.Text = supplier.SupplierName;
            txtContactPerson.Text = supplier.ContactPerson;
            txtPhone.Text = supplier.Phone;
            txtEmail.Text = supplier.Email;
            txtAddress.Text = supplier.Address;
        }

        private void ClearFields()
        {
            selectedSupplierID = 0;

            txtSupplierName.Clear();
            txtContactPerson.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            txtSearch.Clear();

            dgvSuppliers.ClearSelection();

            txtSupplierName.Focus();
        }

        private void SupplierForm_Load(object sender, EventArgs e)
        {
            // Runs when the Supplier Management form opens.
        }

        private void lblContactPerson_Click(object sender, EventArgs e)
        {

        }
    }
}
