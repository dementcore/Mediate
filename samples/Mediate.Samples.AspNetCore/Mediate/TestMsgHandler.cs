using Mediate.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.AspNetCore.Mediate
{
    public class TestMsgHandler : IMessageHandler<TestMsg, TestMsgReply>
    {
        public Task<TestMsgReply> Handle(TestMsg message, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestMsgReply() { Reply = "Reply from test msg with requested data: " + message.Data });
        }
    }

    public class TestMsgHandler2 : IMessageHandler<TestMsg, TestMsgReply>
    {
        public Task<TestMsgReply> Handle(TestMsg message, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestMsgReply() { Reply = "Reply from test msg with requested data: " + message.Data });
        }
    }
}
