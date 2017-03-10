using System;
using System.Collections.Generic;
using System.Text;
using WebHookAdpter.Core;

namespace SimpleDemo
{
    public class TestHookDefine : HookDefine
    {
        public TestHookDefine(string name) { Name = name; }
        public static TestHookDefine Task_Create = new TestHookDefine("Task_Create");
    }

}
