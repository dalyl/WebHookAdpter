using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleDemo;

namespace Test.SimpleDemo
{
    [TestClass]
    public class TaskCreateUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var content = new HookContent_TaskCreate
            {
                TaskID = 1111,
                Customer = "北京*****有限公司",
                Creater = "经理",
                TaskNo = "THBC-600170-200651",
            };
            var template = new Hook_TaskCreateTemplate(content);
            //visual studio 2017 ？do you  have cache the type info? why update the last now?
            template.SendMessage();
            
        }
    }
}
