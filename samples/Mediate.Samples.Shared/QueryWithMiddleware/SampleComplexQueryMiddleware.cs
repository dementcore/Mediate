using Mediate.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared.QueryWithMiddleware
{

    public class SampleComplexQueryMiddleware : IQueryMiddleware<SampleComplexQuery, SampleComplexQueryResponse>
    {
        public async Task<SampleComplexQueryResponse> Invoke(SampleComplexQuery query, CancellationToken cancellationToken, NextMiddlewareDelegate<SampleComplexQueryResponse> next)
        {
            query.QueryData += " I'm using Mediate";

            SampleComplexQueryResponse response = await next();

            response.QueryResponseData += " [modified from middleware]";

            return response;
        }
    }
}
