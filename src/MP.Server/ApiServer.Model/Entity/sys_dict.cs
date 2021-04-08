using System;
using System.Collections.Generic;

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
