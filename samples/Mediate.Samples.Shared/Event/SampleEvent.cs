using Mediate.Abstractions;

namespace Mediate.Samples.Shared.Event
{
    public class SampleEvent:IEvent
    {
        public string EventData { get; set; }
    }
}
