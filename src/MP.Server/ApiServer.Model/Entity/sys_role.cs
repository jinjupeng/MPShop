using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ApiServer.Model.Entity
{
    public partial class sys_role
    {
        public long id { get; set; }
        public string role_name { get; set; }
        public string role_desc { get; set; }
        public string role_code { get; set; }
        public int sort { get; set; }
        public bool? status { get; set; }
    }
}
