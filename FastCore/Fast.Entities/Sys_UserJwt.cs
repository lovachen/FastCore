using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fast.Entities
{
    public partial class Sys_UserJwt
    {
        [Key]
        [StringLength(64)]
        public string Jti { get; set; }
        [Required]
        [StringLength(64)]
        public string RefreshToken { get; set; }
        public Guid UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Expiration { get; set; }
        public int Platform { get; set; }
    }
}
