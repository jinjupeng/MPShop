using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ApiServer.Model.Entity
{
    public partial class goods_carts
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int goods_id { get; set; }
        public int goods_sku_id { get; set; }
        public int num { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string goods_sku_desc { get; set; }
    }
}
