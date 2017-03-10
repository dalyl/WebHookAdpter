using System;
using System.Collections.Generic;
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
