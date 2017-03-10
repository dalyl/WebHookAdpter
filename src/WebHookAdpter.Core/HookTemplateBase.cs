using System;
using System.Collections.Generic;
using System.Text;

namespace WebHookAdpter.Core
{
    
    public abstract class HookTemplateBase<T> where T : HookTemplateBase<T> 
    {
        public abstract HookDefine Define { get; }

        public abstract List<HookChannel> Channels { get; }

        public IClient Client { get; set; }

        public ITemplateEngine Engine { get; set; }

        protected abstract string GetAddress(HookChannel channel);

        protected abstract string GetContent(HookChannel channel);

        protected string replaceTemplate<C>(C model, string template) where C : class
        {
            return Engine.Bulider(template, model);
        }

        public void SendMessage(HookChannel channel)
        {
            var Address = GetAddress(channel);
            var Content = GetContent(channel);
            var result = Client.Post(Address, Content);
        }

        public void SendMessage()
        {
            Channels.ForEach(it => SendMessage(it));
        }

    }

}
