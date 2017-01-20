using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using BOL;

namespace ActiveBlendedStudy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
                if (user.User_ID.ToString().Length > 0 || user.Password.ToString().Length > 0)
                {
                    LoginManager loginManager = new LoginManager();
                    User loggedIn = loginManager.ValidateUserLogin(user.User_ID, user.Password);
                    if (loggedIn != null)
                    {
                        //set session variables
                        Session["User_ID"] = loggedIn.User_ID;
                        Session["Role"] = loggedIn.Role;
                        return RedirectToAction("Index", "Welcome");
                        //if (Session["Role"].ToString().ToUpper().Equals("STUDENT"))
                        //{
                        //    Schedule currentSchedule = new ScheduleManager().getCurrentSchedule();
                        //    Session["Schedule_ID"] = currentSchedule.Schedule_ID.ToString();
                        //    return RedirectToAction("StudentIndex", "Welcome");
                        //}
                        //else if (Session["Role"].ToString().ToUpper().Equals("STAFF"))
                        //{
                        //    return RedirectToAction("StaffIndex", "Welcome");
                        //}
                        //return RedirectToAction("Index", "Welcome");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "INVALID LOGIN ATTEMPT");
                    return RedirectToAction("Login", "Home");
                }
            // TODO: Show error message
            return RedirectToAction("Index", "Home");
        }

        public ActionResult logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Unauthorised()
        {
            return View();
        }


    }
}