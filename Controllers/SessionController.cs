//using BankManagementSystem.DataProcessor;
//using Microsoft.AspNetCore.Mvc;

//namespace BankManagementSystem.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class SessionController : ControllerBase
//    {
//        [HttpPost("set")]
//        public IActionResult SetSession([FromBody] SessionData data)
//        {
//            HttpContext.Session.SetString("UserId", data.UserId);
//            return Ok();
//        }

//        [HttpGet("get")]
//        public IActionResult GetSession()
//        {
//            var userId = HttpContext.Session.GetString("UserId");
//            return Ok(new { UserId = userId });
//        }

//        [HttpPost("clear")]
//        public IActionResult ClearSession()
//        {
//            HttpContext.Session.Clear();
//            return Ok();
//        }
//    }
//}
