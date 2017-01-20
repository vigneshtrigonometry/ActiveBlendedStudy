using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOL;

namespace DAL
{
    public class MaterialRepository : DashboardRepository
    {
        /**********************************************************************/
        // Instance Variables
        /**********************************************************************/

        private static MaterialRepository instance;

        /**********************************************************************/
        // Constructors
        /**********************************************************************/

        /// <summary>
        /// Disable creating the initialization of this class. Users need to use the singleton instance.
        /// </summary>
        private MaterialRepository() : base()
        {
        }

        /**********************************************************************/
        // Public Methods
        /**********************************************************************/
        /// <summary>
        /// Get Instance of the Repository
        /// </summary>
        public static MaterialRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MaterialRepository();
                }
                return instance;
            }
        }
        /// <summary>
        /// Get the list of materials attached to a particular schedule.
        /// </summary>
        /// <param name="scheduleId">The schedule for which the materials need to be queried.</param>
        /// <returns>The list of materials associated with the schedule.</returns>
        public IEnumerable<Material> GetAllMaterialsForSchedule(int scheduleId, int courseId)
        {
            return from material in dB.Materials
                   where material.Schedule.Schedule_ID == scheduleId && material.Course.Course_ID == courseId
                   select material;
        }

        /// <summary>
        /// Get the material using the material Id.
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        public Material GetMaterial(int materialId)
        {
            try
            {
                return (from material in dB.Materials
                        where material.Material_ID == materialId
                        select material).SingleOrDefault();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Method to add a new material to the dB.
        /// </summary>
        /// <param name="material"> The material that needs to be added to the dB.</param>
        public void AddMaterial(Material material)
        {
            dB.Materials.Add(material);
            dB.SaveChanges();
        }

        /// <summary>
        /// Method to update a material present in dB.
        /// If the material is not present in dB, then it adds a new entry.
        /// </summary>
        /// <param name="material">The material that needs to be updated</param>
        public void UpdateMaterial(Material material)
        {
            // First check if the object exists.
            Material existingMaterial = (from m in dB.Materials
                                        where m.Material_ID == material.Material_ID
                                        select m).SingleOrDefault();

            // Make sure the object exists in dB.
            if(existingMaterial != null)
            {
                existingMaterial.Material_ID = material.Material_ID;
                existingMaterial.Schedule = material.Schedule;
                existingMaterial.Link = material.Link;
                existingMaterial.Type = material.Type;
            }
            else
            {
                // Make sure the identity is used for updating the values.
                material.Material_ID = 0;
                // Add the material since it does not exist in dB.
                AddMaterial(material);
            }
            // Save the changes in dB.
            dB.SaveChanges();
        }

        /// <summary>
        /// Method to delete an existing material from the dB.
        /// </summary>
        /// <param name="materialId">The material that needs to be deleted.</param>
        public void DeleteMaterial(int materialId)
        {
            // Retrieve the material that needs to be deleted.
            Material materialToDelete = (from m in dB.Materials
            where m.Material_ID == materialId
            select m).SingleOrDefault();

            // Make sure the item exists before performing the delete operation.
            if(materialToDelete != null)
            {
                dB.Materials.Remove(materialToDelete);
            }
            dB.SaveChanges();
        }
    }
}
