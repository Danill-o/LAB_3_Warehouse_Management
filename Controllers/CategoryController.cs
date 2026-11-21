
using System;
using System.Collections.Generic;
using WarehouseManagementSystem.DAO;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Controllers
{
    public class CategoryController
    {
        private readonly CategoryDAO categoryDAO = new CategoryDAO();

        public List<Category> GetCategories(string search = "")
        {
            return categoryDAO.GetCategories(search);
        }

        public void AddCategory(Category category)
        {
            ValidateCategory(category);
            categoryDAO.AddCategory(category);
        }

        public void UpdateCategory(Category category)
        {
            if (category.CategoryID <= 0)
                throw new ArgumentException("Select a category first.");

            ValidateCategory(category);
            categoryDAO.UpdateCategory(category);
        }

        public void DeleteCategory(int categoryID)
        {
            if (categoryID <= 0)
                throw new ArgumentException("Select a category first.");

            categoryDAO.DeleteCategory(categoryID);
        }

        private void ValidateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("Category name is required.");
        }
    }
}
