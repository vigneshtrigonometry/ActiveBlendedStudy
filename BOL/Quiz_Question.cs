using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class Quiz_Question
    {
        [Key]
        public int Quiz_Question_ID { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        public string Option_1 { get; set; }
        [Required]
        public string Option_2 { get; set; }
        [Required]
        public string Option_3 { get; set; }
        [Required]
        public string Option_4 { get; set; }
        [Required]
        public string Answer { get; set; }

        public virtual Quiz Quiz { get; set; }

        public virtual User User { get; set; }

        private List<Question_Answer> answers = new List<Question_Answer>();

        public virtual List<Question_Answer> Answers
        {
            get
            {
                return answers;
            }
            set
            {
                answers = value;
            }
        }
    }
}
