using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class Quiz
    {
        [Key]
        public int Quiz_ID { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual Schedule Schedule { get; set; }

        public virtual Course Course { get; set; }

        public virtual User User { get; set; }

        private List<Quiz_Question> questions = new List<Quiz_Question>();

        public virtual List<Quiz_Question> Questions
        {
            get
            {
                return questions;
            }
            set
            {
                questions = value;
            }
        }
    }
}
