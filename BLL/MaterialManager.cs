using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOL;
using DAL;
namespace BLL
{
    public class MaterialManager : DashboardManager
    {
        //private MaterialRepository materialRespository = new MaterialRepository();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        public IEnumerable<Material> GetAllMaterials(int scheduleId, int courseId)
        {
            return MaterialRepository.Instance.GetAllMaterialsForSchedule(scheduleId, courseId);
        }

        /// <summary>
        /// Get the material object from the dB for a particular ID.
        /// Sends null if the object is not found.
        /// </summary>
        /// <param name="materialId">The ID for which the material needs to be found.</param>
        /// <returns></returns>
        public Material GetMaterial(int materialId)
        {
            return MaterialRepository.Instance.GetMaterial(materialId);
        }

        /// <summary>
        /// Methot to add a material to the dB. 
        /// This checks if the material is null reference or not. 
        /// If yes, then it thrown a NullReferenceException.
        /// The new ID of the material will be auto generated.
        /// </summary>
        /// <param name="material">The material that needs to be added to the dB. Must not be null.</param>
        public void AddMaterial(Material material)
        {
            if(material != null)
            {
                MaterialRepository.Instance.AddMaterial(material);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        /// <summary>
        /// Method to update a material present in dB.
        /// If the material is not present in dB, then it adds a new entry.
        /// </summary>
        /// <param name="material">The material that needs to be updated</param>
        public void UpdateMaterial(Material material)
        {
            if (material != null)
            {
                MaterialRepository.Instance.UpdateMaterial(material);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        /// <summary>
        /// Method to delete an existing material from the dB.
        /// </summary>
        /// <param name="materialId">The material that needs to be deleted.</param>
        public void DeleteMaterial(int materialId)
        {
            MaterialRepository.Instance.DeleteMaterial(materialId);
        }

        /// <summary>
        /// Allows users to repost a material from a previous schedule to the current schedule.
        /// </summary>
        /// <param name="materialId">The material Id that needs to be reposted.</param>
        /// <returns>Returns true if successfully reposted, else false.</returns>
        public bool RepostMaterialToCurrentSchedule(int materialId, int userId)
        {
            // Get the material
            Material currentMaterial = GetMaterial(materialId);
            Schedule currentSchedule = new ScheduleManager().GetCurrentSchedule();
            if (currentMaterial != null)
            {
                // Check if the course is available in the current schedule
                if (IsCourseAvailableForCurrentSchedule(currentMaterial.Course))
                {
                    // Check if the material already exists in the current schedule
                    List<Material> allMaterials = GetAllMaterials(currentSchedule.Schedule_ID, currentMaterial.Course.Course_ID).ToList();

                    // Now check if the material already is mapped to the current schedule
                    foreach (Material material in allMaterials)
                    {
                        if (material.Type == currentMaterial.Type && material.Link == currentMaterial.Link)
                        {
                            return false;
                        }
                    }

                    // Since the loop executed and we did not find the material in the current schedule, we add now.
                    MaterialRepository.Instance.AddMaterial(GetNewMaterialToRepost(currentMaterial, userId));
                    // Since it is successful, return true.
                    return true;
                }
            }
            return false;
        }

        /**********************************************************************/
        // Private Methods
        /**********************************************************************/
        /// <summary>
        /// Creates a new material object that will be reposted as a new entry.
        /// </summary>
        /// <param name="materialToRepost">The current material that will be replicated</param>
        /// <returns>The replicated object with the ID as null.</returns>
        private Material GetNewMaterialToRepost(Material materialToRepost, int userId)
        {
            Material newMaterial = new Material();
            newMaterial.Course = materialToRepost.Course;
            newMaterial.Schedule = new ScheduleManager().GetCurrentSchedule();
            newMaterial.Link = materialToRepost.Link;
            newMaterial.Type = materialToRepost.Type;
            newMaterial.User = new LoginManager().GetUser(userId);
            return newMaterial;
        }

        /// <summary>
        /// Checks if the course is available for the current schedule.
        /// </summary>
        /// <param name="course">The course that needs to be checked for the current schedule.</param>
        /// <returns>True if the course is avaialble, else False.</returns>
        private bool IsCourseAvailableForCurrentSchedule(Course course)
        {
            bool courseAvailable = false;
            Schedule currentSchedule = new ScheduleManager().GetCurrentSchedule();
            foreach (Schedule schedule in course.Schedules)
            {
                // Successful
                if (schedule.Schedule_ID == currentSchedule.Schedule_ID)
                {
                    courseAvailable = true;
                }
            }
            return courseAvailable;
        }
    }
}
