using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CourseRepository: DashboardRepository
    {
        /**********************************************************************/
        // Instance Variables
        /**********************************************************************/

        private static CourseRepository instance;

        //private ActiveBlendedStudyContext dB;

        /**********************************************************************/
        // Constructors
        /**********************************************************************/

        /// <summary>
        /// Disable creating the initialization of this class. Users need to use the singleton instance.
        /// </summary>
        private CourseRepository()
        {
            dB = ActiveBlendedStudyContext.Instance;
        }

        /**********************************************************************/
        // Public Methods
        /**********************************************************************/
        /// <summary>
        /// Get Instance of the Repository
        /// </summary>
        public static CourseRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CourseRepository();
                }
                return instance;
            }
        }

        //private ActiveBlendedStudyContext dB = new ActiveBlendedStudyContext();
        /// <summary>
        /// Get the courses related to a particular user.
        /// </summary>
        /// <param name="userId">The userId for which the courses need to be queried.</param>
        /// <returns>The list of courses that the user is related to.</returns>
        public IEnumerable<Course> GetCoursesForUser(int userId)
        {
            return from course in dB.Courses
                   where course.Users.Any(c => c.User_ID == userId)
                   select course;
        }

        /// <summary>
        /// Get the list of schedules attached to a particular course.
        /// </summary>
        /// <param name="courseId">The course for which the scedules need to be queried.</param>
        /// <returns>The list of schedules associated with the course.</returns>
        public IEnumerable<Schedule> GetAllSchedulesForCourse(int courseId)
        {
            // TODO: Incomplete
            return null;
        }

        public Course getCourseById(int course_id)
        {
            return dB.Courses.Single(c => c.Course_ID.Equals(course_id));
        }

        public List<Course> GetAllCoursesforScheduleId(int schedule_id)
        {
            return dB.Courses.Where(c => c.Schedules.Any(s => s.Schedule_ID == schedule_id)).ToList();
        }


        public List<Course> FilterCoursesByUser(List<Course> allcourses, int user_id)
        {
            List<Course> filteredCourses = new List<Course>();
            foreach (var item in allcourses)
            {
                if (item.Users.Any(u => u.User_ID == user_id))
                {
                    filteredCourses.Add(item);
                }
            }
            return filteredCourses;
        }
    }
}
