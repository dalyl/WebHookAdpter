using System;
using System.Collections.Generic;
using System.Text;
using WebHookAdpter.Core;

namespace SimpleDemo
{


    public class HookContent_TaskCreate
    {
        /// <summary>
        /// 
        /// </summary>
        public int TaskID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TaskNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Creater { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    public class Hook_TaskCreateTemplate : InnerHookTemplateBase<Hook_TaskCreateTemplate, HookContent_TaskCreate>
    {
        public override HookDefine Define => TestHookDefine.Task_Create;

        public override List<HookChannel> Channels => new List<HookChannel> { TestHookChannel.DingDing };

        public override HookContent_TaskCreate Content { get; set; }

        public Hook_TaskCreateTemplate(HookContent_TaskCreate content) : base(content)
        {
            Client = new WebHookAdpter.Client.Client();
            Engine = new WebHookAdpter.TemplateEngine.RepalceEngine();
        }

        protected override string GetAddress(HookChannel channel)
        {
            if (channel == TestHookChannel.DingDing)
            {
                var addr = "https://oapi.dingtalk.com/robot/send?access_token=8e939ff84f0089b70d713b8343ffb6aeea3497b0e180d28db8424abce3b25676";
                var serviceAddress = "https://oapi.dingtalk.com/robot/send?access_token=2ca23b3145fc33b1a72d11682df5b2298814136b3b01bc9adfead69ff86d13e3";
                return serviceAddress;
            }
            throw (new Exception("接口调取错误"));
        }

        protected override string GetContent(HookChannel channel)
        {
            if (Define == TestHookDefine.Task_Create)
            {
                var template = DingDingTemplateBulider.MarkDownTemplate("任务单创建", "{Customer}", "报告单号：{TaskNo}", "创建人：{Creater}", "###### {CreateTime} 创建 [查看](http://192.168.1.246:804/cs/order/Order_TaskDetails/{TaskID}) ");
                return replaceTemplate(Content, template);
            }
            throw (new Exception("接口调取错误"));
        }
    }

}
