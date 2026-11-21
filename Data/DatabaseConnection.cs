
using MySql.Data.MySqlClient;

namespace WarehouseManagementSystem.Data
{
    public static class DatabaseConnection
    {
        private static readonly string connectionString =
            "Server=localhost;" +
            "Port=3306;" +
            "Database=warehouse_db;" +
            "Uid=root;" +
            "Pwd=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
