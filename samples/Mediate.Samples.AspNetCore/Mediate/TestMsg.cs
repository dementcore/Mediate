using Mediate.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.AspNetCore.Mediate
{
    public class TestMsg:IMessage<TestMsgReply>
    {
        public string Data { get; set; }
    }
}
