using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BOL;

namespace DAL
{
    public class DashboardRepository
    {
        /**********************************************************************/
        // Instance Variables
        /**********************************************************************/
        protected ActiveBlendedStudyContext dB;

        /**********************************************************************/
        // Constructor
        /**********************************************************************/
        public DashboardRepository()
        {
            dB = ActiveBlendedStudyContext.Instance;
        }

        /**********************************************************************/
        // Public Methods
        /**********************************************************************/
        /**********************************************************************/
        // Public Methods
        /**********************************************************************/
        public IEnumerable<Schedule> GetAllSchedule(int courseId)
        {
            return dB.Schedules.Where(c => c.Courses.Any(s => s.Course_ID == courseId));
        }
    }
}
