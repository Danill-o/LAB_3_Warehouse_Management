
using System;
using System.Collections.Generic;
using System.Net.Mail;
using WarehouseManagementSystem.DAO;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Controllers
{
    public class SupplierController
    {
        private readonly SupplierDAO supplierDAO = new SupplierDAO();

        public List<Supplier> GetSuppliers(string search = "")
        {
            return supplierDAO.GetSuppliers(search);
        }

        public bool AddSupplier(Supplier supplier, out string message)
        {
            if (!ValidateSupplier(supplier, out message))
                return false;

            try
            {
                bool result = supplierDAO.AddSupplier(supplier);
                message = result
                    ? "Supplier added successfully."
                    : "Supplier was not added.";

                return result;
            }
            catch (Exception ex)
            {
                message = "Error adding supplier: " + ex.Message;
                return false;
            }
        }

        public bool UpdateSupplier(Supplier supplier, out string message)
        {
            if (supplier == null || supplier.SupplierID <= 0)
            {
                message = "Please select a supplier to update.";
                return false;
            }

            if (!ValidateSupplier(supplier, out message))
                return false;

            try
            {
                bool result = supplierDAO.UpdateSupplier(supplier);
                message = result
                    ? "Supplier updated successfully."
                    : "Supplier was not found or no changes were made.";

                return result;
            }
            catch (Exception ex)
            {
                message = "Error updating supplier: " + ex.Message;
                return false;
            }
        }

        public bool DeleteSupplier(int supplierID, out string message)
        {
            if (supplierID <= 0)
            {
                message = "Please select a supplier to delete.";
                return false;
            }

            try
            {
                bool result = supplierDAO.DeleteSupplier(supplierID);
                message = result
                    ? "Supplier deleted successfully."
                    : "Supplier was not found.";

                return result;
            }
            catch (Exception ex)
            {
                message = "Error deleting supplier: " + ex.Message;
                return false;
            }
        }

        private bool ValidateSupplier(
            Supplier supplier, out string message)
        {
            if (supplier == null ||
                string.IsNullOrWhiteSpace(supplier.SupplierName))
            {
                message = "Supplier name is required.";
                return false;
            }

            if (supplier.SupplierName.Trim().Length > 150)
            {
                message = "Supplier name cannot exceed 150 characters.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(supplier.Email))
            {
                try
                {
                    MailAddress email = new MailAddress(supplier.Email.Trim());

                    if (email.Address != supplier.Email.Trim())
                    {
                        message = "Please enter a valid email address.";
                        return false;
                    }
                }
                catch
                {
                    message = "Please enter a valid email address.";
                    return false;
                }
            }

            supplier.SupplierName = supplier.SupplierName.Trim();
            supplier.ContactPerson = supplier.ContactPerson?.Trim();
            supplier.Phone = supplier.Phone?.Trim();
            supplier.Email = supplier.Email?.Trim();
            supplier.Address = supplier.Address?.Trim();

            message = "";
            return true;
        }
    }
}
