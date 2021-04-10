using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ApiServer.Model.Entity
{
    public partial class sys_dict
    {
        public long id { get; set; }
        public string group_name { get; set; }
        public string group_code { get; set; }
        public string item_name { get; set; }
        public string item_value { get; set; }
        public string item_desc { get; set; }
        public DateTime create_time { get; set; }
    }
}
