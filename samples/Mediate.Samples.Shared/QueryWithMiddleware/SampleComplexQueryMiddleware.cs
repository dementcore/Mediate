﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mediate.Core.Abstractions;

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
