using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UnityApi.Server
{
    public class MessageController : BaseController
    {

        [HttpGet]
        public IActionResult Get()
        {
            return View("message");
        }

        [HttpPut]
        public IActionResult Send(string message)
        {
            return View("message");
        }
    }
}