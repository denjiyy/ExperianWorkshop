using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace BankManagementSystem.DataProcessor.Import
{
    public class CardsDto
    {
        public string CardType { get; set; }
        public string Password { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; }
    }
}
