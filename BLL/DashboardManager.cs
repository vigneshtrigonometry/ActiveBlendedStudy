using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOL;
using DAL;
namespace BLL
{
    public class DashboardManager
    {
        public IEnumerable<Schedule> GetAllScheduleForCourse(int courseId)
        {
            DashboardRepository dashboardRepository = new DashboardRepository();
            return dashboardRepository.GetAllSchedule(courseId);
        }
    }
}
