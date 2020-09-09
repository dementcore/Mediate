using Mediate.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared
{
    public class TestMsg:IMessage<TestMsgReply>
    {
        public string Data { get; set; }
    }
}
