using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fast.Entities
{
    public partial class QuarztSchedule
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(150)]
        public string JobGroup { get; set; }
        [Required]
        [StringLength(150)]
        public string JobName { get; set; }
        public int RunStatus { get; set; }
        [Required]
        [StringLength(150)]
        public string CronExpress { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StarRunTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EndRunTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? NextRunTime { get; set; }
        [StringLength(500)]
        public string TaskDescription { get; set; }
        public int? DataStatus { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? JobRunTime { get; set; }
    }
}
