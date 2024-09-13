using Microsoft.AspNetCore.Mvc;

[Route("main/home/[action]")]
public class HomeController : Controller
{
    public IActionResult Dashboard()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        if (userId.HasValue)
        {
            ViewBag.UserId = userId.Value;
        }
        else
        {
            return RedirectToAction("Login", "Login");
        }

        return View();
    }

    [HttpGet]
    public IActionResult GetUserId()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (userId.HasValue)
        {
            return Json(new { userId = userId.Value });
        }

        return Json(new { error = "User not logged in." });
    }

}