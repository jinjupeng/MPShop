using System;
using System.Collections.Generic;

namespace ApiServer.Model.Entity
{
    public partial class sys_api
    {
        public long id { get; set; }
        public long api_pid { get; set; }
        public string api_pids { get; set; }
        public bool is_leaf { get; set; }
        public string api_name { get; set; }
        public string url { get; set; }
        public int? sort { get; set; }
        public int level { get; set; }
        public bool status { get; set; }
    }
}
