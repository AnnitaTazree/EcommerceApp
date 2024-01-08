using EcommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly EcommerceDBContext _dbContext;

        public LoginController(EcommerceDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Login(AppUser appUser)
        {
            var userName = _dbContext.AppUsers.FirstOrDefault(x => x.UserName == appUser.UserName);
            var password = _dbContext.AppUsers.FirstOrDefault(x => x.Password == appUser.Password);
            if (userName != null && password != null)
            {
                HttpContext.Session.SetString("un", appUser.UserName);
                HttpContext.Session.SetString("id", appUser.Password);
                //return RedirectToAction(nameof(ConfirmOrder));
                return RedirectToAction("ConfirmOrder", "Shopping");
            }
            else
            {
                TempData["wronginfo"] = "Wrong Information";
                return View(appUser);
            }
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp([Bind("Name, UserName, Password, MobileNo, Address")] AppUser appUser)
        {
            string msg = "";
            if (ModelState.IsValid)
            {
                appUser.Role = 1;
                appUser.IsActive = true;

                var checkUsername = _dbContext.AppUsers.FirstOrDefault(x => x.UserName.ToLower() == appUser.UserName.ToLower());
                if (checkUsername != null)
                {
                    TempData["signup"] = "Username already exist!!";
                    return View(appUser);
                }
                _dbContext.AppUsers.Add(appUser);
                await _dbContext.SaveChangesAsync();

                HttpContext.Session.SetString("un", appUser.UserName);
                HttpContext.Session.SetString("id", appUser.Password);

                appUser.Name = String.Empty;
                appUser.UserName = String.Empty;
                appUser.Password = String.Empty;
                appUser.MobileNo = String.Empty;
                appUser.Address = String.Empty;
                TempData["signup"] = "Successfully create an Account!!";
                return RedirectToAction(nameof(Login));

            }
            else
            {
                msg = "Invalid Username or Password";
                return View(appUser);
            }
        }
    }
}
