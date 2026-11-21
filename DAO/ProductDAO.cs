
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.DAO
{
    public class ProductDAO
    {
        public List<Category> GetCategories()
        {
            var categories = new List<Category>();

            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql =
                    "SELECT CategoryID, CategoryName " +
                    "FROM Categories ORDER BY CategoryName";

                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            CategoryID = reader.GetInt32("CategoryID"),
                            CategoryName = reader.GetString("CategoryName")
                        });
                    }
                }
            }

            return categories;
        }

        public List<Product> GetProducts(string search = "")
        {
            var products = new List<Product>();

            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT p.ProductID, p.ProductName,
                           p.CategoryID, c.CategoryName,
                           p.Quantity, p.UnitPrice, p.ReorderLevel
                    FROM Products p
                    LEFT JOIN Categories c
                        ON p.CategoryID = c.CategoryID
                    WHERE p.ProductName LIKE @search
                    ORDER BY p.ProductID DESC";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@search", "%" + search + "%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = reader.GetInt32("ProductID"),
                                ProductName = reader.GetString("ProductName"),

                                CategoryID = reader.IsDBNull(
                                    reader.GetOrdinal("CategoryID"))
                                    ? (int?)null
                                    : reader.GetInt32("CategoryID"),

                                CategoryName = reader.IsDBNull(
                                    reader.GetOrdinal("CategoryName"))
                                    ? ""
                                    : reader.GetString("CategoryName"),

                                Quantity = reader.GetInt32("Quantity"),
                                UnitPrice = reader.GetDecimal("UnitPrice"),
                                ReorderLevel = reader.GetInt32("ReorderLevel")
                            });
                        }
                    }
                }
            }

            return products;
        }

        public void AddProduct(Product product)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql = @"
                    INSERT INTO Products
                    (ProductName, CategoryID, Quantity,
                     UnitPrice, ReorderLevel)
                    VALUES
                    (@name, @category, @quantity,
                     @price, @reorder)";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@name", product.ProductName);

                    cmd.Parameters.AddWithValue(
                        "@category",
                        product.CategoryID.HasValue
                            ? (object)product.CategoryID.Value
                            : DBNull.Value);

                    cmd.Parameters.AddWithValue(
                        "@quantity", product.Quantity);

                    cmd.Parameters.AddWithValue(
                        "@price", product.UnitPrice);

                    cmd.Parameters.AddWithValue(
                        "@reorder", product.ReorderLevel);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql = @"
                    UPDATE Products
                    SET ProductName = @name,
                        CategoryID = @category,
                        Quantity = @quantity,
                        UnitPrice = @price,
                        ReorderLevel = @reorder
                    WHERE ProductID = @id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@name", product.ProductName);

                    cmd.Parameters.AddWithValue(
                        "@category",
                        product.CategoryID.HasValue
                            ? (object)product.CategoryID.Value
                            : DBNull.Value);

                    cmd.Parameters.AddWithValue(
                        "@quantity", product.Quantity);

                    cmd.Parameters.AddWithValue(
                        "@price", product.UnitPrice);

                    cmd.Parameters.AddWithValue(
                        "@reorder", product.ReorderLevel);

                    cmd.Parameters.AddWithValue(
                        "@id", product.ProductID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteProduct(int productID)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql =
                    "DELETE FROM Products WHERE ProductID = @id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", productID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
