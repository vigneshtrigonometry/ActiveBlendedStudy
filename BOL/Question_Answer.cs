using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class Question_Answer
    {
        [Key]
        public int Question_Answer_ID { get; set; }
        [Required]
        public string Answer { get; set; }
        [Required]
        public virtual Quiz_Question Quiz_Question { get; set; }

        public virtual User User { get; set; }
    }
}
