using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace WebHookAdpter.TemplateEngine
{

    public class FormattedValues : IReadOnlyList<KeyValuePair<string, object>>
    {
        private const string NullFormat = "[null]";
        private static ConcurrentDictionary<string, ValuesFormatter> _formatters = new ConcurrentDictionary<string, ValuesFormatter>();
        private readonly ValuesFormatter _formatter;
        private readonly object[] _values;
        private readonly string _originalMessage;

        public FormattedValues(string format, params object[] values)
        {
            if (values?.Length != 0 && format != null)
            {
                _formatter = _formatters.GetOrAdd(format, f => new ValuesFormatter(f));
            }

            _originalMessage = format ?? NullFormat;
            _values = values;
        }

        public KeyValuePair<string, object> this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new IndexOutOfRangeException(nameof(index));
                }

                if (index == Count - 1)
                {
                    return new KeyValuePair<string, object>("{OriginalFormat}", _originalMessage);
                }

                return _formatter.GetValue(_values, index);
            }
        }

        public int Count
        {
            get
            {
                if (_formatter == null)
                {
                    return 1;
                }

                return _formatter.ValueNames.Count + 1;
            }
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            for (int i = 0; i < Count; ++i)
            {
                yield return this[i];
            }
        }

        public override string ToString()
        {
            if (_formatter == null)
            {
                return _originalMessage;
            }

            return _formatter.Format(_values);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
