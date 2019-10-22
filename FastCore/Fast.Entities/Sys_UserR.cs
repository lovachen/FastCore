using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fast.Entities
{
    public partial class Sys_UserR
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Required]
        [StringLength(64)]
        public string R { get; set; }
        public int Platform { get; set; }
    }
}
