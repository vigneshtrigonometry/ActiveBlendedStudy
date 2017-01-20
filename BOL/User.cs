using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class User
    {

        [Key]
        public int User_ID { get; set; }
       
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }

        private List<Course> courses = new List<Course>();

        private List<Schedule> schedules = new List<Schedule>();

        public virtual List<Course> Courses
        {
            get
            {
                return courses;
            }
            set
            {
                courses = value;
            }
        }

        public virtual List<Schedule> Schedules
        {
            get
            {
                return schedules;
            }
            set
            {
                schedules = value;
            }
        }
    }
}
