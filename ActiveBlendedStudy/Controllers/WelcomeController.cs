using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using BOL;

namespace ActiveBlendedStudy.Controllers
{
    public class WelcomeController : Controller
    {
        /**********************************************************************/
        // Instance Variables
        /**********************************************************************/

        /// <summary>
        /// Hold the current user Id. Will be read from the session.
        /// </summary>
        private int userId;

        private ScheduleManager scheduleManager;

        private CourseManager courseManager;

        /**********************************************************************/
        // Constructor
        /**********************************************************************/
        public WelcomeController()
        {
            scheduleManager = new ScheduleManager();
            courseManager = new CourseManager();
        }

        /**********************************************************************/
        // Public Methods
        /**********************************************************************/
        public ActionResult Index()
        {
            GetValuesFromSession();
            if (Session["Role"].ToString().ToUpper().Equals("STUDENT"))
            {
                // get the current schedule
                Schedule currentSchedule = new ScheduleManager().GetCurrentSchedule();
                // set the session for schedule
                SetScheduleInSession(currentSchedule.Schedule_ID.ToString()); 

                List<Course> courses = courseManager.GetCourses(currentSchedule.Schedule_ID, userId);
                return View(courses);
            }
            else if (Session["Role"].ToString().ToUpper().Equals("STAFF"))
            {
                List<Schedule> schedules = scheduleManager.GetScheduleForUserId(userId); 
                SetScheduleInSession(schedules.First().Schedule_ID.ToString());

                List<SelectListItem> schedule_items = new List<SelectListItem>();
                foreach (var s in schedules)
                {
                    schedule_items.Add(new SelectListItem { Text = s.Schedule_Name, Value = s.Schedule_ID.ToString() });
                }
                ViewBag.Schedules = new SelectList(schedule_items, "Value", "Text");

                List<Course> courses = courseManager.GetCourses(schedules.First().Schedule_ID, userId);
                return View(courses);
            }
            return Content("INVALID ENTRY");
        }


        [HttpPost]
        public ActionResult Index(String Schedule_ID)
        {
            
            List<Schedule> schedules = scheduleManager.GetScheduleForUserId(int.Parse(Session["User_ID"].ToString()));
            Session["Schedule_ID"] = Schedule_ID;
            List<SelectListItem> schedule_items = new List<SelectListItem>();
            foreach (var s in schedules)
            {
                schedule_items.Add(new SelectListItem { Text = s.Schedule_ID + " : " + s.Schedule_Name, Value = s.Schedule_ID.ToString() });
            }
            ViewBag.Schedules = new SelectList(schedule_items, "Value", "Text");
            List<Course> allCourses = courseManager.GetAllCoursesforScheduleId(int.Parse(Session["Schedule_ID"].ToString()));
            List<Course> courses_thisuser = courseManager.FilterCoursesByUser(allCourses, int.Parse(Session["User_ID"].ToString()));
            
            return PartialView("_IndexPartial", courses_thisuser);
        }

        public ActionResult CourseSelected(int id)
        {
            Session["Course_ID"] = id.ToString();
            // Navigate to the next page.
            return RedirectToAction("Index", "Forum");
        }

        /**********************************************************************/
        // Private Methods
        /**********************************************************************/

        /// <summary>
        /// Method to get the various values from Session.
        /// </summary>
        private void GetValuesFromSession()
        {
            userId = int.Parse(Session["User_ID"].ToString());
        }

        /// <summary>
        /// Sets the schedule id to the session.
        /// </summary>
        /// <param name="scheduleId"></param>
        private void SetScheduleInSession(string scheduleId)
        {
            Session["Schedule_ID"] = scheduleId;
        }
    }
}