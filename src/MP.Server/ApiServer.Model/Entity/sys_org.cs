using System;
using System.Collections.Generic;

namespace ApiServer.Model.Entity
{
    public partial class sys_org
    {
        public long id { get; set; }
        public long org_pid { get; set; }
        public string org_pids { get; set; }
        public bool is_leaf { get; set; }
        public string org_name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public int? sort { get; set; }
        public int level { get; set; }
        public bool status { get; set; }
    }
}
