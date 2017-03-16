using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using WebHookAdpter.Core;

namespace WebHookAdpter.TemplateEngine
{
    public class FormatterEngine : ITemplateEngine
    {
        private const string NullFormat = "[null]";
        private static ConcurrentDictionary<Type, object[]> _getters = new ConcurrentDictionary<Type, object[]>();
        public string Bulider<T>(string template, T model) where T : class
        {
            object[] _values = null;
            if (template?.Length != 0 && model != null)
            {
                _values = _getters.GetOrAdd(typeof(T), f => GetValues(f));
            }
            if (_values == null) template = NullFormat;
            var formatter = new FormattedValues(template, _values);
            return formatter.ToString();
        }

        object[] GetValues<T>(T model)
        {
            return null;
        }
    }
}
