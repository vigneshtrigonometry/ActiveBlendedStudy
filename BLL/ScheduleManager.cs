using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BOL;
using DAL;
namespace BLL
{
   public class ScheduleManager
    {
        ScheduleRepository schduleRepository = new ScheduleRepository();
        public IEnumerable<Schedule> GetAllSchedules(int courseId)
        {
            return schduleRepository.GetAllSchedule(courseId);
        }
        public Schedule GetScheduleById(int scheduleId)
        {
            return schduleRepository.getScheduleById(scheduleId);
        }

        public List<Schedule> GetScheduleForUserId(int userId)
        {
            return schduleRepository.getSchedulesforUser(userId);
        }
        public List<Schedule> GetScheduleForCourseId(int courseID)
        {
            return schduleRepository.getScheduleduleForCourse(courseID);
        }
        public Schedule GetCurrentSchedule()
        {
            //assume the last schedule is the current schedule
            return schduleRepository.getAllSchedules().Last();
        }

        /// <summary>
        /// Checks if the schedule id passed is the latest one in the system
        /// </summary>
        /// <param name="currentScheduleId">The schedule id that needs to be verified</param>
        /// <returns>Returns true if the schedule id is the latest one, else false.</returns>
        public bool IsScheduleTheLatest(int currentScheduleId)
        {
            // If the two match, then the passed schedule is the latest one.
            if (currentScheduleId == GetCurrentSchedule().Schedule_ID)
            {
                return true;
            }
            return false;
        }
    }
}
