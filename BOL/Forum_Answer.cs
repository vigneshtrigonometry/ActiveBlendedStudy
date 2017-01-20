using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class Forum_Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Forum_Answer_ID { get; set; }
        [Required]
        public string Answer { get; set; }

        public virtual User User { get; set; }

        public virtual Forum Forum { get; set; }
    }

}
