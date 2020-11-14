using Mediate.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.Shared.QueryWithMiddleware
{
    public class SampleComplexQuery: BaseQuery<SampleComplexQueryResponse>
    {
        public string QueryData { get; set; }
    }
}
