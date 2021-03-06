﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fast.Entities
{
    public partial class Sys_Category
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(1)]
        public string IsMenu { get; set; }
        [Required]
        [StringLength(128)]
        public string UID { get; set; }
        [StringLength(128)]
        public string Code { get; set; }
        [StringLength(128)]
        public string FatherCode { get; set; }
        [StringLength(128)]
        public string RouteTemplate { get; set; }
        [StringLength(128)]
        public string RouteName { get; set; }
        [StringLength(50)]
        public string IconClass { get; set; }
        public int Sort { get; set; }
        [StringLength(2)]
        public string Target { get; set; }
        [StringLength(128)]
        public string Controller { get; set; }
        [StringLength(128)]
        public string Action { get; set; }
    }
}
