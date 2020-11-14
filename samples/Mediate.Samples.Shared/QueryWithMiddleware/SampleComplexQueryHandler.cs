using Mediate.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
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
                QueryResponseData = "Complex query response data: " + message.QueryId + "Query Data: " + message.QueryData
            });
        }
    }
}
