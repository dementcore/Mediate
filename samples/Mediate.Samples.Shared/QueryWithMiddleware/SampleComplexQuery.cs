namespace Mediate.Samples.Shared.QueryWithMiddleware
{
    public class SampleComplexQuery: BaseQuery<SampleComplexQueryResponse>
    {
        public string QueryData { get; set; }
    }
}
