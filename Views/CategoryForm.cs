
using System;
using System.Windows.Forms;
using WarehouseManagementSystem.Controllers;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Views
{
    public partial class CategoryForm : Form
    {
        private readonly CategoryController controller =
            new CategoryController();

        private int selectedCategoryID = 0;

        public CategoryForm()
        {
            InitializeComponent();

            Load += CategoryForm_Load;

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            btnSearch.Click += btnSearch_Click;

            dgvCategories.CellClick += dgvCategories_CellClick;
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories(string search = "")
        {
            try
            {
                dgvCategories.DataSource = null;
                dgvCategories.DataSource =
                    controller.GetCategories(search);

                if (dgvCategories.Columns["CategoryID"] != null)
                    dgvCategories.Columns["CategoryID"].HeaderText =
                        "Category ID";

                if (dgvCategories.Columns["CategoryName"] != null)
                    dgvCategories.Columns["CategoryName"].HeaderText =
                        "Category Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load categories:\n" + ex.Message);
            }
        }

        private Category ReadInputs()
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
                throw new ArgumentException("Enter a category name.");

            return new Category
            {
                CategoryID = selectedCategoryID,
                CategoryName = txtCategoryName.Text.Trim()
            };
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                controller.AddCategory(ReadInputs());

                MessageBox.Show("Category added successfully.");

                LoadCategories(txtSearch.Text.Trim());
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to add category:\n" + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedCategoryID <= 0)
                {
                    MessageBox.Show("Select a category first.");
                    return;
                }

                controller.UpdateCategory(ReadInputs());

                MessageBox.Show("Category updated successfully.");

                LoadCategories(txtSearch.Text.Trim());
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to update category:\n" + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedCategoryID <= 0)
            {
                MessageBox.Show("Select a category first.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Delete this category? Products assigned to it will remain, but their category will be cleared.",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                controller.DeleteCategory(selectedCategoryID);

                MessageBox.Show("Category deleted successfully.");

                LoadCategories(txtSearch.Text.Trim());
                ClearInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to delete category:\n" + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LoadCategories(txtSearch.Text.Trim());
            ClearInputs();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void dgvCategories_CellClick(
            object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvCategories.Rows[e.RowIndex];

            selectedCategoryID = Convert.ToInt32(
                row.Cells["CategoryID"].Value);

            txtCategoryName.Text = Convert.ToString(
                row.Cells["CategoryName"].Value);
        }

        private void ClearInputs()
        {
            selectedCategoryID = 0;
            txtCategoryName.Clear();
            dgvCategories.ClearSelection();
        }


        private void lblCategoryName_Click(object sender, EventArgs e)
        {
            // No action is needed when clicking this label.
        }

    }
}
