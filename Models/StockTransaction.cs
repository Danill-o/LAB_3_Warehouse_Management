
using System;

namespace WarehouseManagementSystem.Models
{
    public class StockTransaction
    {
        public int TransactionID { get; set; }

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string TransactionType { get; set; }

        public int Quantity { get; set; }

        public string Notes { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}
