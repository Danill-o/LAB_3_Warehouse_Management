
using System;
using System.Collections.Generic;
using WarehouseManagementSystem.DAO;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Controllers
{
    public class StockController
    {
        private readonly StockDAO stockDAO = new StockDAO();

        public List<Product> GetProducts(string search = "")
        {
            return stockDAO.GetProducts(search);
        }

        public List<StockTransaction> GetTransactions()
        {
            return stockDAO.GetTransactions();
        }

        public bool RecordStock(
            int productID,
            string transactionType,
            int quantity,
            string notes,
            out string message)
        {
            if (productID <= 0)
            {
                message = "Please select a product.";
                return false;
            }

            if (transactionType != "Stock In" &&
                transactionType != "Stock Out")
            {
                message = "Please select Stock In or Stock Out.";
                return false;
            }

            if (quantity <= 0)
            {
                message = "Quantity must be greater than zero.";
                return false;
            }

            try
            {
                stockDAO.RecordStockTransaction(
                    productID,
                    transactionType,
                    quantity,
                    notes);

                message = transactionType +
                    " recorded successfully.";
                return true;
            }
            catch (Exception ex)
            {
                message = "Could not record stock: " + ex.Message;
                return false;
            }
        }
    }
}
