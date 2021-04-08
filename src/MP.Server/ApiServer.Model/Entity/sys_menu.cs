using System;
using System.Collections.Generic;

namespace ApiServer.Model.Entity
{
    public partial class sys_menu
    {
        public long id { get; set; }
        public long menu_pid { get; set; }
        public string menu_pids { get; set; }
        public bool is_leaf { get; set; }
        public string menu_name { get; set; }
        public string url { get; set; }
        public string icon { get; set; }
        public int? sort { get; set; }
        public int level { get; set; }
        public bool status { get; set; }
    }
}
