using System;
using System.Collections.Generic;

namespace ApiServer.Model.Entity
{
    public partial class sys_user_role
    {
        public long role_id { get; set; }
        public long user_id { get; set; }
    }
}
