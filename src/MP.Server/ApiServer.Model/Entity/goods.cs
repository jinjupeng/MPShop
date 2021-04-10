using System;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ApiServer.Model.Entity
{
    public partial class goods
    {
        public int id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public string spu_no { get; set; }
        public string goods_name { get; set; }
        public decimal start_price { get; set; }
        public long category_id { get; set; }
        public long brand_id { get; set; }
        public string goods_desc { get; set; }
    }
}
