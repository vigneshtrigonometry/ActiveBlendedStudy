using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOL
{
    public class Material
    {
        [Key]
        public int Material_ID { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Link { get; set; }

        public virtual Schedule Schedule { get; set; }

        public virtual Course Course { get; set; }

        public virtual User User { get; set; }
    }
}
