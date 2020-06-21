using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            if(HttpContext.Session.GetInt32("userId") == null)
            {
                return View();
            }
            return View("AddWedding");
        }
        
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
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
                    return View("Index");
                }
                string hash = regHasher.HashPassword(newuser,newuser.Password);
                newuser.Password = hash;
                databases.Users.Add(newuser);
                databases.SaveChanges();
                HttpContext.Session.SetInt32("userId", newuser.UserId);
                return Redirect("/dashboard");
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
                return Redirect("/dashboard");
            }
            return View("Index");
        }
        ////////////////////////////// Dashboard
        [HttpGet]
        [Route("dashboard")]

        public IActionResult Dashboard()
        {   
            if(HttpContext.Session.GetInt32("userId") == null)
            {
                return Redirect("/");
            }
            ViewBag.UserId = HttpContext.Session.GetInt32("userId");
            ViewBag.WeddingPlans = databases.Weddings
                .Include(w => w.AllGuests)
                .ThenInclude( a => a.User)
                .OrderBy(w => w.Date)
                .ToList();
            return View();
        }
        [Route("wedding/{id}")]
        public IActionResult WeddingDetails(int id)
        {
            if(HttpContext.Session.GetInt32("userId") == null)
            {
                return Redirect("/");
            }
            ViewBag.Wedding = databases.Weddings
                .Include(w => w.AllGuests)
                .ThenInclude(a => a.User)
                .FirstOrDefault(w => w.WeddingId == id);
            return View("WeddingDetails");
        }
        [Route("wedding/{id}/{cmd}")]
        public IActionResult WeddingJoin(int id, string cmd)
        {   
            if(HttpContext.Session.GetInt32("userId") == null)
            {
                return Redirect("/");
            }
            Wedding selected = databases.Weddings.FirstOrDefault(w => w.WeddingId == id);
            int? userId = HttpContext.Session.GetInt32("userId");
            if (cmd =="cancel")
            {
                databases.Weddings.Remove(selected);
                databases.SaveChanges();
            }
            else if(cmd == "RSVP")
            {
                Associate join = new Associate();
                join.UserId = (int) userId;
                join.WeddingId = id;
                databases.Associates.Add(join);
                databases.SaveChanges();
            }
            else if (cmd == "UnRSVP")
            {
                Associate remove = databases.Associates
                    .FirstOrDefault(a => a.UserId == userId && a.WeddingId == id);
                databases.Associates.Remove(remove);
                databases.SaveChanges();

            }
            return Redirect("/dashboard");

        }


        [HttpPost("wedding/create")]
        public IActionResult CreateWedding(Wedding Plan)
        {   
            if(HttpContext.Session.GetInt32("userId") == null)
            {
                return Redirect("/");
            }
            if(ModelState.IsValid)
            {    
                Plan.UserId = (int) HttpContext.Session.GetInt32("userId");
                databases.Weddings.Add(Plan);
                databases.SaveChanges();
                return Redirect("/dashboard");
            }   
            return View("AddWedding");
        }
    }
}
