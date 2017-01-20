using BOL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CourseManager
    {
        //private CourseRepository courseRepository = new CourseRepository();
        public IEnumerable<Course> GetAllCourses(int userId)
        {
            return CourseRepository.Instance.GetCoursesForUser(userId);
        }

        public List<Course> GetAllCoursesforScheduleId(int schedule_id)
        {
            return CourseRepository.Instance.GetAllCoursesforScheduleId(schedule_id);
        }

        public List<Course> FilterCoursesByUser(List<Course> allcourses, int userid)
        {
            return CourseRepository.Instance.FilterCoursesByUser(allcourses, userid);
        }

        /// <summary>
        /// Get the list of courses for a particular user and schedule.
        /// </summary>
        /// <param name="scheduleId">The schedule ID that is attached to the course</param>
        /// <param name="userId">The user for which the courses need to retrieved. </param>
        /// <returns></returns>
        public List<Course> GetCourses(int scheduleId, int userId)
        {
            List<Course> allCourses = GetAllCoursesforScheduleId(scheduleId);
            List<Course> courses = FilterCoursesByUser(allCourses, userId);
            return courses;
        }

        /// <summary>
        /// Get the course object based on its id.
        /// </summary>
        /// <param name="courseId">The courseId for which the course object needs to be queried.</param>
        /// <returns>The course object from the dB.</returns>
        public Course GetCourse(int courseId)
        {
            return CourseRepository.Instance.getCourseById(courseId);
        }
    }
}
