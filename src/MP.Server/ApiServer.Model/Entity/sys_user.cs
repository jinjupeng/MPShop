using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ApiServer.Model.Entity
{
    public partial class sys_user
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string nickname { get; set; }
        public string portrait { get; set; }
        public int org_id { get; set; }
        public bool enabled { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public DateTime create_time { get; set; }
    }
}
