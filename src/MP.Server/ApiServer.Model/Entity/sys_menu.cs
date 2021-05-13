using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ApiServer.Model.Entity
{
    public partial class sys_menu
    {
        public int id { get; set; }
        public int menu_pid { get; set; }
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
