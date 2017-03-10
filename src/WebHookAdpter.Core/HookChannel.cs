using System;
using System.Collections.Generic;
using System.Text;

namespace WebHookAdpter.Core
{
    public abstract class HookChannel
    {
        public string Name { get; set; }

        //None,
        //DingDing,
        //WeiChat,
        //Other,
    }

}
