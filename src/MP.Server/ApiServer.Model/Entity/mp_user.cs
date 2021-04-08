using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ApiServer.Model.Entity
{
    public partial class mp_user
    {
        public mp_user()
        {
            session_key = new HashSet<session_key>();
        }

        public int id { get; set; }
        public string nickName { get; set; }
        public string openId { get; set; }
        public string avatarUrl { get; set; }
        public int? gender { get; set; }
        public string language { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public virtual ICollection<session_key> session_key { get; set; }
    }
}
