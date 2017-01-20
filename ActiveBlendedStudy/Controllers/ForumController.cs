using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOL;
using BLL;
using ActiveBlendedStudy.Filters;

namespace ActiveBlendedStudy.Controllers
{
    [SessionAuthorize]
    public class ForumController : Controller
    {
        /**********************************************************************/
        // Instance Variables
        /**********************************************************************/

        /// <summary>
        /// Hold the current course Id. Will be read from the session.
        /// </summary>
        private int courseId;

        /// <summary>
        /// Hold the current schedule Id. Will be read from the session.
        /// </summary>
        private int scheduleId;

        /// <summary>
        /// Hold the current user Id. Will be read from the session.
        /// </summary>
        private int userId;

        /// <summary>
        /// The forum manager object that will allow the controller to communicate with dB.
        /// </summary>
        private ForumManager forumManager;

        /**********************************************************************/
        // Constructors
        /**********************************************************************/
        public ForumController()
        {
            forumManager = new ForumManager();
        }

        /**********************************************************************/
        // Public Methods
        /**********************************************************************/
        // GET: Forum

        public ActionResult Index()
        {
            GetValuesFromSession();
            ViewBag.IsCurrentSchedule = false;
            if (new ScheduleManager().IsScheduleTheLatest(scheduleId))
            {
                ViewBag.IsCurrentSchedule = true;
            }
            return View(forumManager.GetAllForums(scheduleId, courseId).ToList());
        }

        // GET: Forum/Details/5
        public ActionResult Details(int id)
        {
            return RedirectToAction("Index", "ForumPosts");
        }

        // GET: Forum/Create
        [SessionAuthorize(role = "staff")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Forum/Create
        [SessionAuthorize(role = "staff")]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                GetValuesFromSession();
                Forum forum = GetForumFromCollection(collection);
                //var context = new EntityContext();
                

                forum.Schedule = new ScheduleManager().GetScheduleById(scheduleId);
                forum.User = new LoginManager().GetUser(userId);
                forum.Course = new CourseManager().GetCourse(courseId);
                forumManager.AddForum(forum);
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                Console.Write(e.ToString());
                return View();
            }
        }
        [SessionAuthorize(role = "staff")]
        // GET: Forum/Edit/5
        public ActionResult Edit(int id)
        {
            Forum forumToEdit = forumManager.GetForum(id);
            return View(forumToEdit);
        }

        // POST: Forum/Edit/5
        [HttpPost]
        [SessionAuthorize(role = "staff")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                GetValuesFromSession();
                // Create the material object to update
                Forum updatedForum = GetForumFromCollection(collection);
                updatedForum.Forum_ID = id;
                updatedForum.Schedule = new ScheduleManager().GetScheduleById(scheduleId);
                updatedForum.User = new LoginManager().GetUser(userId);
                updatedForum.Course = new CourseManager().GetCourse(courseId);

                // Now save the same
                forumManager.UpdateForum(updatedForum);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Forum/Delete/5
        [SessionAuthorize(role = "staff")]
        public ActionResult Delete(int id)
        {
            Forum forumToDelete = forumManager.GetForum(id);
            return View(forumToDelete);
        }

        // POST: Forum/Delete/5
        [SessionAuthorize(role = "staff")]
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                forumManager.DeleteForum(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// This will be display all the answers related to a particular forum topic.
        /// </summary>
        /// <returns>View displaying all the forum answers for a particular forum topic</returns>
        public ActionResult DisplayForumPosts(int id)
        {
            ViewBag.Title = forumManager.GetForum(id).Topic;
            ViewBag.Id = id;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PostForumAnswer(int id, string newPost)
        {
            if (newPost.Length > 0)
            {
                GetValuesFromSession();
                LoginManager loginManager = new LoginManager();

                Forum_Answer forumAnswer = new Forum_Answer();
                forumAnswer.Forum = forumManager.GetForum(id);
                forumAnswer.User = loginManager.GetUser(userId);
                forumAnswer.Answer = newPost;
                // We need to insert here
                forumManager.AddForumPost(forumAnswer);
            }
            List<Forum_Answer> forum_Answers = forumManager.GetAllForumAnswers(id).ToList();
            return PartialView(viewName: "PostForumAnswer", model: forum_Answers);
        }

        /**********************************************************************/
        // Private Methods
        /**********************************************************************/
        /// <summary>
        /// Method to get the forum object from the FormCollection
        /// </summary>
        /// <param name="collection">The collection from which the material object needs to be retrieved.</param>
        /// <returns></returns>
        private Forum GetForumFromCollection(FormCollection collection)
        {
            Forum retrievedForum = new Forum();
            retrievedForum.Topic = collection["Topic"];
            return retrievedForum;
        }

        /// <summary>
        /// Method to get the various values from Session.
        /// </summary>
        private void GetValuesFromSession()
        {
            scheduleId = int.Parse(Session["Schedule_ID"].ToString());
            courseId = int.Parse(Session["Course_ID"].ToString());
            userId = int.Parse(Session["User_ID"].ToString());
        }



        ///Get machine learning data from string passed
        ///returns a string as well
        public ActionResult GetMachineLearningData(string inputText)
        {
            TextAnalyticsSample.Program analytics = new TextAnalyticsSample.Program();
            return Content(analytics.GetSentiment(inputText));
        }

           
}
}
