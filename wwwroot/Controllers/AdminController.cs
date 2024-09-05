using BankManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankManagementSystem.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly UserService _userService;

        public AdminController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult CreateAdmin(string fullName, string username, string email, string password)
        {
            var currentAdmin = GetCurrentAdmin();
            _userService.CreateAdmin(currentAdmin, fullName, username, email, password);

            return Ok("Admin created successfully");
        }

        //[HttpPost]
        //public IActionResult CreateCustomer(string fullName, string username, string email, string password, string phoneNumber, string address, DateOnly dob)
        //{
        //    var currentAdmin = GetCurrentAdmin();
        //    _userService.CreateCustomer(currentAdmin, fullName, username, email, password, phoneNumber, address, dob);

        //    return Ok("Customer created successfully");
        //}

        private Administrator GetCurrentAdmin()
        {
            throw new NotImplementedException();
        }
    }
}