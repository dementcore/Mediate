using Mediate.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared.Event
{
    public class SampleEvent:IEvent
    {
        public string EventData { get; set; }
    }
}
