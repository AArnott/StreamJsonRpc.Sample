using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreamJsonRpc.Sample.Web.Controllers
{
    public class JsonRpcServer
    {
        public int Add(int a, int b) => a + b;
    }
}
