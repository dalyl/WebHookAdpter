using System;
using System.Collections.Generic;
using System.Text;

namespace WebHookAdpter.Core
{
    public  interface ITemplateEngine
    {
        string Bulider<T>(string template,T model) where T : class;
    }
}
