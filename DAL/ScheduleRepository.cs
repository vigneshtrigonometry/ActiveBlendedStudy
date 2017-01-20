using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOL;
namespace DAL
{
   public class ScheduleRepository : DashboardRepository
    {
        //CourseRepository courseRepository = new CourseRepository();
        public Schedule getScheduleById(int id)
        {
            return dB.Schedules.Single(m => m.Schedule_ID.Equals(id));
        }

        public List<Schedule> getSchedulesforUser(int userId)
        {
            //get courses the user is enrolled in 
            List<Course> courses = new List<Course>();
            courses = CourseRepository.Instance.GetCoursesForUser(userId).ToList();

            //get schedules of the particular courses
            List<Schedule> schedules = new List<Schedule>();
            foreach (Course c in courses)
            {
                foreach (Schedule s in c.Schedules)
                {
                    schedules.Add(s);
                }
            }

            //return unique list of schedules. We can filter courses based on schedules later
            return schedules.Distinct().ToList();
        }

        public List<Schedule> getScheduleduleForCourse(int course_id)
        {
            return CourseRepository.Instance.getCourseById(course_id).Schedules;
        }

        public List<Schedule> getAllSchedules()
        {
            return dB.Schedules.ToList();
        }
    }
}
