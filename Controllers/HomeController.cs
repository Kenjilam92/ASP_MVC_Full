using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using wedding_planner.Models;

namespace wedding_planner.Controllers
{
    public class HomeController : Controller
    {   
        private Context databases {get;set;}
        private PasswordHasher<User> regHasher = new PasswordHasher<User>() ;
        private PasswordHasher<LoginUser> logHasher = new PasswordHasher<LoginUser>(); 
        public HomeController(Context context)
        {
            databases = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {   
            return View();
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(User newuser)
        {
            if(ModelState.IsValid)
            {   
                if(databases.Users.Any(u => u.Email == newuser.Email))
                {
                    ModelState.AddModelError("Email","This email is already registed. Please Login!");
                }
                string hash = regHasher.HashPassword(newuser,newuser.Password);
                newuser.Password = hash;
                databases.Users.Add(newuser);
                databases.SaveChanges();
                HttpContext.Session.SetInt32("userId", newuser.UserId);
                return Redirect("/");
            }
            return View("Index");
        }
        [Route("login")]
        public IActionResult LogIn(LoginUser user)
        {
            if(ModelState.IsValid)
            {
                User userInDB = databases.Users.FirstOrDefault(u => u.Email == user.LoginEmail);
                if( userInDB==null)
                {
                    ModelState.AddModelError("LoginEmail","Invalid Email or Password");
                    return View("Index");
                }
                var decode = logHasher.VerifyHashedPassword(user, userInDB.Password, user.LoginPassword);
                if( decode==0)
                {
                    ModelState.AddModelError("LoginPassword","Invalid Email or Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32 ("userId", userInDB.UserId);
                return Redirect("/");
            }
            return View("Index");
        }


    }
}
