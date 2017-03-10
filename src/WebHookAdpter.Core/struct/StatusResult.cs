using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace WebHookAdpter.Core
{

    public class StatusResult
    {
        public StatusResult() { Succeeded = true; }

        public StatusResult(bool success)
        {
            Succeeded = success;
        }

        public StatusResult(StatusResult status)
        {
            AddErrors(status);
        }

        public StatusResult(IEnumerable<string> errors)
        {
            if (errors == null || errors.Count() == 0)
            {
                Succeeded = true;
            }
            else
            {
                AddErrors(errors);
            }
        }

        public StatusResult(params string[] errors)
        {
            if (errors == null)
            {
                Succeeded = true;
            }
            else
            {
                Succeeded = !(errors.Count() > 0);
                Errors = errors.ToList();
            }
        }

        public List<string> Errors { get; private set; }

        public StatusResult AddError(string error)
        {
            if (Errors == null)
                Errors = new List<string>();
            Errors.Add(error);
            Succeeded = false;
            return this;
        }

        public StatusResult AddErrors(IEnumerable<string> errors)
        {
            if (Errors == null)
                Errors = new List<string>();
            Errors.AddRange(errors);
            Succeeded = false;
            return this;
        }

        public StatusResult AddErrors(StatusResult status)
        {
            if (status.Succeeded) return this;
            if (status.Errors == null) return this;
            if (status.Errors.Count == 0) return this;
            AddErrors(status.Errors);
            return this;
        }

        public bool Succeeded { get; private set; }

    }

    public class StatusResult<T> : StatusResult
    {
        public T Result { get; protected set; }

        public StatusResult<T> AddResult(T model)
        {
            Result = model;
            return this;
        }

        public StatusResult<T> AddResult(T model, StatusResult status)
        {
            Result = model;
            base.AddErrors(status);
            return this;
        }

        public void Copy(StatusResult<T> source)
        {
            this.AddResult(source.Result);
            this.AddErrors(source);
        }

        public StatusResult(StatusResult<T> source) : base(true)
        {
            Copy(source);
        }

        public StatusResult(StatusResult status)
        {
            AddErrors(status);
        }

        // [JsonConstructor]
        public StatusResult(bool success, T result, IEnumerable<string> errors) : base(errors)
        {
            Result = result;
        }

        public StatusResult(bool success, T result) : base(success)
        {
            Result = result;
        }

        public StatusResult(T result) : base(true)
        {
            Result = result;
        }

        public StatusResult(bool success) : base(success) { }

        public StatusResult(IEnumerable<string> errors) : base(errors) { }

        public StatusResult(params string[] errors) : base(errors) { }

        public StatusResult() : base() { }


        public new StatusResult<T> AddError(string error)
        {
            error = error.Replace("\n", string.Empty).Replace("\r", string.Empty);
            base.AddError(error);
            return this;
        }

        public new StatusResult<T> AddErrors(IEnumerable<string> errors)
        {
            base.AddErrors(errors);
            return this;
        }

        public new StatusResult<T> AddErrors(StatusResult status)
        {
            base.AddErrors(status);
            return this;
        }

    }

}
