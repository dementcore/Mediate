using Mediate.Core;
using Mediate.Samples.AspNetCore.Models;
using Mediate.Samples.Shared.Query;
using Mediate.Samples.Shared.QueryWithMiddleware;
using Mediate.Samples.Shared.Event;
using Mediate.Samples.Shared.EventWithMiddleware;
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

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            return View();
        }

        public async Task<IActionResult> Query(CancellationToken cancellationToken)
        {
            SampleQuery query = new SampleQuery() { QueryData = "Sample Query Data" };

            SampleQueryResponse res = await _mediator.Send<SampleQuery, SampleQueryResponse>(query, cancellationToken);

            ViewBag.Response = res.QueryResponseData;

            return View();
        }

        public async Task<IActionResult> QueryMiddleware(CancellationToken cancellationToken)
        {
            SampleComplexQuery query = new SampleComplexQuery() { QueryData = "Sample Complex Query Data" };

            SampleComplexQueryResponse res = await _mediator.Send<SampleComplexQuery, SampleComplexQueryResponse>(query, cancellationToken);

            ViewBag.Response = res.QueryResponseData;

            return View();
        }

        public async Task<IActionResult> Event(CancellationToken cancellationToken)
        {
            SampleEvent @event = new SampleEvent() { EventData = "Sample event data" };

            await _mediator.Dispatch(@event, cancellationToken);

            return View();
        }

        public async Task<IActionResult> EventMiddleware(CancellationToken cancellationToken)
        {
            SampleComplexEvent @event = new SampleComplexEvent() { EventData = "Sample complex event data" };

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
