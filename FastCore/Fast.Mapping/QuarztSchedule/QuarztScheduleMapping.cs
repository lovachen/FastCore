using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fast.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class QuarztScheduleMapping
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(150)]
        public string JobGroup { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(150)]
        public string JobName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RunStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(150)]
        public string CronExpress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? StarRunTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? EndRunTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? NextRunTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [StringLength(500)]
        public string TaskDescription { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? DataStatus { get; set; }

        /// <summary>
        /// 运行时，每次运行都记录
        /// </summary>
        public DateTime? JobRunTime { get; set; }
    }
}
