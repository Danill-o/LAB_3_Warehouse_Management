
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.DAO
{
    public class StockDAO
    {
        // Get products for the stock management screen.
        public List<Product> GetProducts(string search = "")
        {
            List<Product> products = new List<Product>();

            using (MySqlConnection conn =
                   DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT ProductID, ProductName, Quantity,
                           UnitPrice, ReorderLevel
                    FROM Products
                    WHERE ProductName LIKE @search
                    ORDER BY ProductName";

                using (MySqlCommand cmd =
                       new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue(
                        "@search", "%" + search + "%");

                    using (MySqlDataReader reader =
                           cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = Convert.ToInt32(
                                    reader["ProductID"]),
                                ProductName = reader["ProductName"].ToString(),
                                Quantity = Convert.ToInt32(
                                    reader["Quantity"]),
                                UnitPrice = Convert.ToDecimal(
                                    reader["UnitPrice"]),
                                ReorderLevel = Convert.ToInt32(
                                    reader["ReorderLevel"])
                            });
                        }
                    }
                }
            }

            return products;
        }

        // Record stock-in or stock-out atomically.
        public void RecordStockTransaction(
            int productID,
            string transactionType,
            int quantity,
            string notes)
        {
            if (quantity <= 0)
                throw new ArgumentException(
                    "Quantity must be greater than zero.");

            if (transactionType != "Stock In" &&
                transactionType != "Stock Out")
                throw new ArgumentException(
                    "Invalid stock transaction type.");

            using (MySqlConnection conn =
                   DatabaseConnection.GetConnection())
            {
                conn.Open();

                using (MySqlTransaction transaction =
                       conn.BeginTransaction())
                {
                    try
                    {
                        // Lock the product row while updating stock.
                        string lockSql =
                            "SELECT Quantity FROM Products " +
                            "WHERE ProductID = @id FOR UPDATE";

                        int currentQuantity;

                        using (MySqlCommand cmd =
                               new MySqlCommand(lockSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@id", productID);

                            object result = cmd.ExecuteScalar();

                            if (result == null)
                                throw new Exception(
                                    "Product was not found.");

                            currentQuantity = Convert.ToInt32(result);
                        }

                        int newQuantity;

                        if (transactionType == "Stock In")
                        {
                            newQuantity = checked(
                                currentQuantity + quantity);
                        }
                        else
                        {
                            if (quantity > currentQuantity)
                                throw new Exception(
                                    "Insufficient stock. Available: " +
                                    currentQuantity);

                            newQuantity = currentQuantity - quantity;
                        }

                        string updateSql = @"
                            UPDATE Products
                            SET Quantity = @quantity
                            WHERE ProductID = @id";

                        using (MySqlCommand cmd =
                               new MySqlCommand(
                                   updateSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue(
                                "@quantity", newQuantity);
                            cmd.Parameters.AddWithValue("@id", productID);

                            cmd.ExecuteNonQuery();
                        }

                        string insertSql = @"
                            INSERT INTO StockTransactions
                            (ProductID, TransactionType, Quantity, Notes)
                            VALUES
                            (@productID, @type, @quantity, @notes)";

                        using (MySqlCommand cmd =
                               new MySqlCommand(
                                   insertSql, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue(
                                "@productID", productID);
                            cmd.Parameters.AddWithValue(
                                "@type", transactionType);
                            cmd.Parameters.AddWithValue(
                                "@quantity", quantity);
                            cmd.Parameters.AddWithValue(
                                "@notes",
                                string.IsNullOrWhiteSpace(notes)
                                    ? (object)DBNull.Value
                                    : notes.Trim());

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // Get the history of stock movements.
        public List<StockTransaction> GetTransactions()
        {
            List<StockTransaction> transactions =
                new List<StockTransaction>();

            using (MySqlConnection conn =
                   DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT st.TransactionID,
                           st.ProductID,
                           p.ProductName,
                           st.TransactionType,
                           st.Quantity,
                           st.Notes,
                           st.TransactionDate
                    FROM StockTransactions st
                    INNER JOIN Products p
                        ON st.ProductID = p.ProductID
                    ORDER BY st.TransactionDate DESC,
                             st.TransactionID DESC";

                using (MySqlCommand cmd =
                       new MySqlCommand(sql, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        transactions.Add(new StockTransaction
                        {
                            TransactionID = Convert.ToInt32(
                                reader["TransactionID"]),
                            ProductID = Convert.ToInt32(
                                reader["ProductID"]),
                            ProductName = reader["ProductName"].ToString(),
                            TransactionType =
                                reader["TransactionType"].ToString(),
                            Quantity = Convert.ToInt32(
                                reader["Quantity"]),
                            Notes = reader["Notes"] == DBNull.Value
                                ? ""
                                : reader["Notes"].ToString(),
                            TransactionDate = Convert.ToDateTime(
                                reader["TransactionDate"])
                        });
                    }
                }
            }

            return transactions;
        }
    }
}
