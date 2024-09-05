using BankManagementSystem.Models;
using System;
using System.Linq;

namespace BankManagementSystem.Services
{
    public class UserService
    {
        private readonly BankingSystemContext bankingContext;

        public UserService(BankingSystemContext context)
        {
            bankingContext = context;
        }

        public Administrator CreateAdmin(Administrator currentAdmin, string fullName, string username, string email, string password)
        {
            if (!currentAdmin.IsAdministrator)
                throw new UnauthorizedAccessException("Only administrators can create other admins.");

            var newAdmin = new Administrator
            {
                FullName = fullName,
                Username = username,
                Email = email,
                Password = password,
                IsAdministrator = true
            };

            bankingContext.Administrators.Add(newAdmin);
            bankingContext.SaveChanges();

            return newAdmin;
        }

        public void DeleteCustomer(Administrator admin, int customerId)
        {
            if (!admin.IsAdministrator)
                throw new UnauthorizedAccessException("Only administrators can delete customers.");

            var customer = bankingContext.Customers.FirstOrDefault(c => c.Id == customerId);
            if (customer == null)
                throw new Exception("Customer not found.");

            bankingContext.Customers.Remove(customer);
            bankingContext.SaveChanges();
        }
    }
}