﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ApiServer.Auth.WeChat.MiniProgram
{
    public class Input
    {
        public string code { get; set; }
        //public object State { get; set; }//不涉及刀回调，所以不必考虑用户状态字段
    }
}
