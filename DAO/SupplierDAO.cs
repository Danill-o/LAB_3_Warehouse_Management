
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.DAO
{
    public class SupplierDAO
    {
        public List<Supplier> GetSuppliers(string search = "")
        {
            List<Supplier> suppliers = new List<Supplier>();

            using (MySqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql = @"
                    SELECT SupplierID, SupplierName,
                           ContactPerson, Phone, Email, Address
                    FROM Suppliers
                    WHERE SupplierName LIKE @search
                       OR ContactPerson LIKE @search
                       OR Phone LIKE @search
                    ORDER BY SupplierID DESC";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suppliers.Add(new Supplier
                            {
                                SupplierID = Convert.ToInt32(reader["SupplierID"]),
                                SupplierName = reader["SupplierName"].ToString(),
                                ContactPerson = reader["ContactPerson"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Email = reader["Email"].ToString(),
                                Address = reader["Address"].ToString()
                            });
                        }
                    }
                }
            }

            return suppliers;
        }

        public bool AddSupplier(Supplier supplier)
        {
            using (MySqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql = @"
                    INSERT INTO Suppliers
                    (SupplierName, ContactPerson, Phone, Email, Address)
                    VALUES
                    (@name, @contact, @phone, @email, @address)";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    AddParameters(cmd, supplier);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateSupplier(Supplier supplier)
        {
            using (MySqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql = @"
                    UPDATE Suppliers
                    SET SupplierName = @name,
                        ContactPerson = @contact,
                        Phone = @phone,
                        Email = @email,
                        Address = @address
                    WHERE SupplierID = @id";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    AddParameters(cmd, supplier);
                    cmd.Parameters.AddWithValue("@id", supplier.SupplierID);

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteSupplier(int supplierID)
        {
            using (MySqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();

                string sql =
                    "DELETE FROM Suppliers WHERE SupplierID = @id";

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", supplierID);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private void AddParameters(MySqlCommand cmd, Supplier supplier)
        {
            cmd.Parameters.AddWithValue("@name", supplier.SupplierName);
            cmd.Parameters.AddWithValue("@contact",
                string.IsNullOrWhiteSpace(supplier.ContactPerson)
                    ? (object)DBNull.Value : supplier.ContactPerson);
            cmd.Parameters.AddWithValue("@phone",
                string.IsNullOrWhiteSpace(supplier.Phone)
                    ? (object)DBNull.Value : supplier.Phone);
            cmd.Parameters.AddWithValue("@email",
                string.IsNullOrWhiteSpace(supplier.Email)
                    ? (object)DBNull.Value : supplier.Email);
            cmd.Parameters.AddWithValue("@address",
                string.IsNullOrWhiteSpace(supplier.Address)
                    ? (object)DBNull.Value : supplier.Address);
        }
    }
}
