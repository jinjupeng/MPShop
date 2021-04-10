using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ApiServer.Model.Entity
{
    public partial class session_key
    {
        public int id { get; set; }
        public int uid { get; set; }
        public string sessionKey { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public virtual mp_user u { get; set; }
    }
}
