using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ApiServer.Model.Entity
{
    public partial class sys_role_menu
    {
        public int id { get; set; }
        public int role_id { get; set; }
        public int menu_id { get; set; }
    }
}
