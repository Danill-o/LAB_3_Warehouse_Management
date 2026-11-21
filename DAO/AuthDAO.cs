using System;
using MySql.Data.MySqlClient;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.DAO
{
    public class AuthDAO
    {
        public bool RegisterUser(AuthUser user)
        {
            const string sql = @"
                INSERT INTO Users
                    (FullName, Username, Email, PasswordHash, Role)
                VALUES
                    (@FullName, @Username, @Email, @PasswordHash, 'Staff');";

            using (MySqlConnection conn = DatabaseConnection.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@FullName", user.FullName);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

                conn.Open();
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public AuthUser GetUserByUsername(string username)
        {
            const string sql = @"
                SELECT UserID, FullName, Username, Email, PasswordHash, Role
                FROM Users
                WHERE Username = @Username
                LIMIT 1;";

            using (MySqlConnection conn = DatabaseConnection.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);

                conn.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new AuthUser
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        FullName = reader["FullName"].ToString(),
                        Username = reader["Username"].ToString(),
                        Email = reader["Email"].ToString(),
                        PasswordHash = reader["PasswordHash"].ToString(),
                        Role = reader["Role"].ToString()
                    };
                }
            }
        }
    }
}