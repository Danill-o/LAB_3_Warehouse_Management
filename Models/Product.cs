using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WarehouseManagementSystem.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; } = "";

        public int? CategoryID { get; set; }

        public string CategoryName { get; set; } = "";

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public int ReorderLevel { get; set; }

        public decimal TotalValue
        {
            get { return Quantity * UnitPrice; }
        }
    }
}


