
using System;
using System.Collections.Generic;
using WarehouseManagementSystem.DAO;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Controllers
{
    public class ProductController
    {
        private readonly ProductDAO productDAO = new ProductDAO();

        public List<Category> GetCategories()
        {
            return productDAO.GetCategories();
        }

        public List<Product> GetProducts(string search = "")
        {
            return productDAO.GetProducts(search);
        }

        public void AddProduct(Product product)
        {
            ValidateProduct(product);
            productDAO.AddProduct(product);
        }

        public void UpdateProduct(Product product)
        {
            if (product.ProductID <= 0)
                throw new ArgumentException("Select a valid product.");

            ValidateProduct(product);
            productDAO.UpdateProduct(product);
        }

        public void DeleteProduct(int productID)
        {
            if (productID <= 0)
                throw new ArgumentException("Select a product to delete.");

            productDAO.DeleteProduct(productID);
        }

        private void ValidateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrWhiteSpace(product.ProductName))
                throw new ArgumentException("Product name is required.");

            if (product.Quantity < 0)
                throw new ArgumentException(
                    "Quantity cannot be negative.");

            if (product.UnitPrice < 0)
                throw new ArgumentException(
                    "Unit price cannot be negative.");

            if (product.ReorderLevel < 0)
                throw new ArgumentException(
                    "Reorder level cannot be negative.");
        }
    }
}
