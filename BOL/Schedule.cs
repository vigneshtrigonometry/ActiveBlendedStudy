using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class Schedule
    {
        [Key]
        public int Schedule_ID { get; set; }
        [Required]
        public String Schedule_Name { get; set; }
        
        private List<Course> courses { get; set; }

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

        private List<User> users = new List<User>();

        public virtual List<User> Users
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
            }
        }
    }
}
