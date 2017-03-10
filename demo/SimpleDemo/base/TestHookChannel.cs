using System;
using System.Collections.Generic;
using System.Text;
using WebHookAdpter.Core;

namespace SimpleDemo
{
    public class TestHookChannel : HookChannel
    {
        public TestHookChannel(string name) { Name = name; }
        public static TestHookChannel DingDing = new TestHookChannel("DingDing");
    }
}
