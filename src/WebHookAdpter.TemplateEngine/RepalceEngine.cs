using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WebHookAdpter.Core;

namespace WebHookAdpter.TemplateEngine
{
    public class RepalceEngine : ITemplateEngine
    {
        public string Bulider<T>(string template, T model) where T : class
        {
            if (model == default(T)) return template;
            var properties = new List<string>();
            properties.ForEach(it => template = template.Replace(template, $"{{{it}}}"));
            return template;
        }

       
    }

}
