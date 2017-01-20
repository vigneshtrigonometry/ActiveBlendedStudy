using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BOL;
using BLL;

namespace ActiveBlendedStudy.Controllers
{
    public class MaterialController : Controller
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
        /// The material manager object that will allow the controller to communicate with dB.
        /// </summary>
        private MaterialManager materialManager;

        /**********************************************************************/
        // Constructor
        /**********************************************************************/
        public MaterialController()
        {
            materialManager = new MaterialManager();
        }

        /**********************************************************************/
        // Public Methods
        /**********************************************************************/

        // GET: Material
        public ActionResult Index()
        {
            GetValuesFromSession();
            ViewBag.IsCurrentSchedule = false;
            if (new ScheduleManager().IsScheduleTheLatest(scheduleId))
            {
                ViewBag.IsCurrentSchedule = true;
            }
            return View(materialManager.GetAllMaterials(scheduleId, courseId).ToList());
        }

        // GET: Material/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Material/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Material/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                GetValuesFromSession();
                Material material = GetMaterialFromCollection(collection);
                material.Schedule = new ScheduleManager().GetScheduleById(scheduleId);
                material.User = new LoginManager().GetUser(userId);
                material.Course = new CourseManager().GetCourse(courseId);
                materialManager.AddMaterial(material);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Material/Edit/5
        public ActionResult Edit(int id)
        {
            Material materialToEdit = materialManager.GetMaterial(id);
            return View(materialToEdit);
        }

        // POST: Material/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                GetValuesFromSession();
                // Create the material object to update
                Material updatedMaterial = GetMaterialFromCollection(collection);
                updatedMaterial.Material_ID = id;
                updatedMaterial.Schedule = new ScheduleManager().GetScheduleById(scheduleId);
                updatedMaterial.User = new LoginManager().GetUser(userId);
                updatedMaterial.Course = new CourseManager().GetCourse(courseId);

                // Now save the same
                materialManager.UpdateMaterial(updatedMaterial);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Material/Delete/5
        public ActionResult Delete(int id)
        {
            Material materialToDelete = materialManager.GetMaterial(id);
            return View(materialToDelete);
        }

        // POST: Material/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                materialManager.DeleteMaterial(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Repost(int id)
        {
            materialManager.RepostMaterialToCurrentSchedule(id, userId);
            return RedirectToAction("Index");
        }
        /**********************************************************************/
        // Private Methods
        /**********************************************************************/
        /// <summary>
        /// Method to get the material object from the FormCollection
        /// </summary>
        /// <param name="collection">The collection from which the material object needs to be retrieved.</param>
        /// <returns></returns>
        private Material GetMaterialFromCollection(FormCollection collection)
        {
            Material retrievedMaterial = new Material();
            retrievedMaterial.Type = collection["Type"];
            retrievedMaterial.Link = collection["Link"];
            return retrievedMaterial;
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
    }
}
