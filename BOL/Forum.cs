using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class Forum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Forum_ID { get; set; }
        [Required]
        public string Topic { get; set; }

        public virtual Schedule Schedule { get; set; }

        public virtual Course Course { get; set; }

        public virtual User User { get; set; }

        private List<Forum_Answer> answers = new List<Forum_Answer>();

        public virtual List<Forum_Answer> Answers
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
