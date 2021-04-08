using System;
using System.Collections.Generic;

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
