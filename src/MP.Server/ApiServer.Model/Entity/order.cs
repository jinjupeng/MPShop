using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ApiServer.Model.Entity
{
    public partial class order
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string out_trade_no { get; set; }
        public string transaction_id { get; set; }
        public int pay_state { get; set; }
        public int total_fee { get; set; }
        public int address_id { get; set; }
        public string address_desc { get; set; }
        public string goods_carts_ids { get; set; }
        public string goods_name_desc { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
