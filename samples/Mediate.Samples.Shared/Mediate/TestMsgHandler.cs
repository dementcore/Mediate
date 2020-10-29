using Mediate.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared
{
    public class TestMsgHandler : IQueryHandler<TestMsg, TestMsgReply>
    {
        public Task<TestMsgReply> Handle(TestMsg message, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestMsgReply()
            {
                Reply = "Reply from test msg handler with requested data: "
                + message.Data + " " + Guid.NewGuid()
            });
        }
    }

    public class TestMsgHandler2 : IQueryHandler<TestMsg, TestMsgReply>
    {
        public Task<TestMsgReply> Handle(TestMsg message, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TestMsgReply()
            {
                Reply = "Reply from test msg handler 2 with requested data: "
                + message.Data + " " + Guid.NewGuid()
            });
        }
    }
}
