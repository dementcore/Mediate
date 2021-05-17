// File: HomeController.cs
// The MIT License
//
// Copyright (c) 2021 DementCore
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//

using Mediate.Abstractions;
using Mediate.Samples.AspNetCore.Models;
using Mediate.Samples.Shared.Event;
using Mediate.Samples.Shared.EventWithMiddleware;
using Mediate.Samples.Shared.Query;
using Mediate.Samples.Shared.QueryWithMiddleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMediator _mediator;

        public HomeController(ILogger<HomeController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Query()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Query(Models.QueryMiddlewareModel model, CancellationToken cancellationToken)
        {
            SampleQuery query = new SampleQuery() { QueryData = model.Name };

            SampleQueryResponse res = await _mediator.Send(query, cancellationToken);

            ViewBag.Response = res.QueryResponseData;

            return View();
        }

        public IActionResult QueryMiddleware()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QueryMiddleware(Models.QueryMiddlewareModel model, CancellationToken cancellationToken)
        {
            SampleComplexQuery query = new SampleComplexQuery() { QueryData = model.Name };

            SampleComplexQueryResponse res = await _mediator.Send(query, cancellationToken);

            ViewBag.Response = res.QueryResponseData;

            return View();
        }

        public IActionResult Event()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Event(Models.QueryMiddlewareModel model, CancellationToken cancellationToken)
        {
            SampleEvent @event = new SampleEvent() { EventData = model.Name };

            await _mediator.Dispatch(@event, cancellationToken);

            return View();
        }

        public IActionResult EventMiddleware()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EventMiddleware(Models.QueryMiddlewareModel model, CancellationToken cancellationToken)
        {
            SampleComplexEvent @event = new SampleComplexEvent() { EventData = model.Name };

            await _mediator.Dispatch(@event, cancellationToken);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
