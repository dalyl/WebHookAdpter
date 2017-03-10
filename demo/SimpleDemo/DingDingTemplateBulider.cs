using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDemo
{
 
    public class DingDingTemplateBulider
    {

        public static string TextTemplate()
        {
            return "";
        }


        public static string LinkTemplate()
        {
            return "";
        }

        /*

     {
"msgtype": "markdown",
"markdown": {
 "title":"杭州天气",
 "text": "#### 杭州天气\n" +
         "> 9度，西北风1级，空气良89，相对温度73%\n\n" +
         "> ![screenshot](http://image.jpg)\n"  +
         "> ###### 10点20分发布 [天气](http://www.thinkpage.cn/) \n"
}
}

     */


        public static string MarkDownTemplate(string title, params string[] bodys)
        {
            return MarkDownTemplate(title, bodys, null);
        }

        public static string MarkDownTemplate(string title, string[] bodys, Dictionary<string, string[]> subs)
        {
            var bulider = new StringBuilder();
            bulider.Append("{ \"msgtype\": \"markdown\",");
            bulider.Append("\"markdown\": {");
            bulider.Append($"\"title\":\"{title}\",");
            bulider.AppendLine($"\"text\": \"#### {title}");
            if (bodys != null) foreach (var line in bodys) bulider.AppendLine($">{line}");
            if (subs != null)
            {
                foreach (var sub in subs)
                {
                    if (!string.IsNullOrEmpty(sub.Key)) bulider.AppendLine($"\"text\": \"#### {sub.Key}");
                    if (sub.Value != null) foreach (var line in sub.Value) bulider.AppendLine($">{line}");
                }
            }
            bulider.Append("\"}");
            return bulider.ToString();
        }

        static void DingTalk()
        {

            while (true)
            {
                try
                {
                    //                    var msg = new TextMessage
                    //                    {
                    //                        at = new AT("13799270040"),
                    //                        text = new TextContent("我就是我, 是不一样的烟火"),
                    //                        isAtAll = true,
                    //                    };

                    //                    var link = new LinkMessage
                    //                    {
                    //                        link = new LinkContent("百度推广", "我就是我, 是不一样的烟火", "https://baidu.com", "https://img.alicdn.com/top/i1/LB1lIUlPFXXXXbGXFXXXXXXXXXX"),
                    //                        //  at = new AT("13799270040"),
                    //                    };

                    //                    var mark = new MarkDownMessage
                    //                    {
                    //                        markdown = new MarkDownContent("任务单下单", $@"
                    //#### 新的任务单提交调查 
                    //> KA特比勒（中国）投资有限公司
                    //> 报告单号: THBC-600175-200647
                    //> 报告语言：双语报告
                    //> 创建时间：{DateTime.Now.AddDays(-1)}
                    //> 提交时间：{DateTime.Now.AddDays(1)}
                    //>   创建者：康峰
                    //>   CS助理：马雪婷
                    //> ![screenshot](https://img.alicdn.com/top/i1/LB1lIUlPFXXXXbGXFXXXXXXXXXX)
                    //#### 候选人
                    //> 姓名：虚拟人
                    //> 电话：131########
                    //> 邮箱：ceshi@taihe.com.cn
                    //> ######  [资质待分配列表](http://localhost:2876/PC/Task/WorkTaskList) 
                    //> ######  [工作待分配列表](http://localhost:2876/PC/Task/WorkTaskList) 
                    //"),
                    //                    };

                    //                    Uri serviceReq = new Uri(addr);
                    //                    var str = "{\"msgtype\": \"text\",\"text\": {\"content\": \"我就是我, 是不一样的烟火\" }  }";//msg.ToJson()
                    //                    str = msg.ToJson();
                    //                    str = link.ToJson();
                    //                    str = mark.ToJson();
                    //                    HttpContent content = new StringContent(str, Encoding.UTF8, "application/json");
                    //                    HttpClient Client = new HttpClient();
                    //                    var result = Client.PostAsync(serviceReq, content).Result;
                    Console.ReadLine();
                }
                catch (Exception ex)
                {

                }
            }

        }
    }

}
