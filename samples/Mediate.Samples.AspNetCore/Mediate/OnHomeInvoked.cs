using Mediate.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mediate.Samples.AspNetCore.Mediate
{
    public class OnHomeInvoked:IEvent
    {
        public string RequestId { get; set; }
    }
}
