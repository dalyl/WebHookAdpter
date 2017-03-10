using System;
using System.Collections.Generic;
using System.Text;
using WebHookAdpter.Core;

namespace SimpleDemo
{
    public abstract class InnerHookTemplateBase<T, C> : HookTemplateBase<T> where T : InnerHookTemplateBase<T, C>
    {
        public int UserID { get; set; }

        public int GroupID { get; set; }

        public abstract C Content { get; set; }

        public InnerHookTemplateBase(C content) { Content = content; }

    }
}
