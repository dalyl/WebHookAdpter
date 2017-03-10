using System;
using System.Collections.Generic;
using System.Text;
using WebHookAdpter.Core;

namespace SimpleDemo
{
    public abstract class CustomerHookTemplateBase<T> : HookTemplateBase<T> where T : CustomerHookTemplateBase<T>
    {
        public int CustomerID { get; set; }
    }
}
