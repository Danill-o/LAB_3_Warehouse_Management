
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.DAO
{
    public class CategoryDAO
    {
        public List<Category> GetCategories(string search = "")
        {
            var categories = new List<Category>();

            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT CategoryID, CategoryName
                    FROM Categories
                    WHERE CategoryName LIKE @search
                    ORDER BY CategoryID DESC";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@search", "%" + search + "%");

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
            }

            return categories;
        }

        public void AddCategory(Category category)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql =
                    "INSERT INTO Categories (CategoryName) VALUES (@name)";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@name", category.CategoryName);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateCategory(Category category)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql = @"
                    UPDATE Categories
                    SET CategoryName = @name
                    WHERE CategoryID = @id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@name", category.CategoryName);

                    cmd.Parameters.AddWithValue(
                        "@id", category.CategoryID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCategory(int categoryID)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql =
                    "DELETE FROM Categories WHERE CategoryID = @id";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", categoryID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
