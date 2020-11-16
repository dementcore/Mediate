using Mediate.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared.Query
{
    public class SampleQueryHandler : IQueryHandler<SampleQuery, SampleQueryResponse>
    {
        public Task<SampleQueryResponse> Handle(SampleQuery message, CancellationToken cancellationToken)
        {
            return Task.FromResult(new SampleQueryResponse()
            {
                QueryResponseData = $"Hi {message.QueryData}!!!"
            });
        }
    }
}
