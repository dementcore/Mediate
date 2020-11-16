using Mediate.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared.QueryWithMiddleware
{
    public class SampleComplexQueryHandler : IQueryHandler<SampleComplexQuery, SampleComplexQueryResponse>
    {
        public Task<SampleComplexQueryResponse> Handle(SampleComplexQuery message, CancellationToken cancellationToken)
        {
            return Task.FromResult(new SampleComplexQueryResponse()
            {
                QueryResponseData = $"Hi {message.QueryData}!!!"
            });
        }
    }
}
