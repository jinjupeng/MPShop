using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Model.Model.Config
{

    public class WxAuthConfig
    {
        public string appid { get; set; }
        public string secret { get; set; }
        public string js_code { get; set; }
        public string grant_type { get; set; }
    }
}
