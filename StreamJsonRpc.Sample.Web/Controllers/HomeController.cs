using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StreamJsonRpc.Sample.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Socket()
        {
            if (this.HttpContext.WebSockets.IsWebSocketRequest)
            {
                var socket = await this.HttpContext.WebSockets.AcceptWebSocketAsync();
                using (var jsonRpc = new JsonRpc(new WebSocketMessageHandler(socket), new JsonRpcServer()))
                {
                    jsonRpc.CancelLocallyInvokedMethodsWhenConnectionIsClosed = true;
                    jsonRpc.StartListening();
                    await jsonRpc.Completion;
                }

                return new EmptyResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }
    }
}
