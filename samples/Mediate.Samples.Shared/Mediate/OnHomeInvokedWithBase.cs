using Mediate.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared
{
    public class OnHomeInvokedWithBase : BaseEvent
    {
        public string TestData { get; set; }
    }
}
