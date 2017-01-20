using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class Course
    {
        [Key]
        public int Course_ID { get; set; }
        [Required]
        public string Name { get; set; }

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

        private List<Schedule> schedules = new List<Schedule>();

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

        private List<Forum> forums = new List<Forum>();

        public virtual List<Forum> Forums
        {
            get
            {
                return forums;
            }
            set
            {
                forums = value;
            }
        }

        private List<Quiz> quizzes = new List<Quiz>();

        public virtual List<Quiz> Quizzes
        {
            get
            {
                return quizzes;
            }
            set
            {
                quizzes = value;
            }
        }

        private List<Material> materials = new List<Material>();

        public virtual List<Material> Materials
        {
            get
            {
                return materials;
            }
            set
            {
                materials = value;
            }
        }
    }
}
