using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class casbin_rule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(10)]
        public string ptype { get; set; }
        [MaxLength(500)]
        public string v0 { get; set; }
        [MaxLength(500)]
        public string v1 { get; set; }
        [MaxLength(500)]
        public string v2 { get; set; }
        [MaxLength(500)]
        public string v3 { get; set; }
        [MaxLength(500)]
        public string v4 { get; set; }
        [MaxLength(500)]
        public string v5 { get; set; }

    }
}
