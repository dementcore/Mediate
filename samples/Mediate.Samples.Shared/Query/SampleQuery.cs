using Mediate.Abstractions;

namespace Mediate.Samples.Shared.Query
{
    public class SampleQuery: IQuery<SampleQueryResponse>
    {
        public string QueryData { get; set; }
    }
}
