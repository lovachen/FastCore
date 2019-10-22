using System;
using System.Collections.Generic;
using System.Text;

namespace Fast.Mapping
{
    [Serializable]
    public class Sys_PermissionMapping
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
