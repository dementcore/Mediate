using Mediate.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared
{
    public class OnHomeInvoked:IEvent
    {
        public string RequestId { get; set; }
    }
}
