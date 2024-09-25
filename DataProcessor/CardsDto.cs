using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace BankManagementSystem.DataProcessor
{
    public class CardsDto
    {
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public string Status { get; set; }
        public int AccountId { get; set; }
    }
}
