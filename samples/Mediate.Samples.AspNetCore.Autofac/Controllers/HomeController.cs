using Mediate.Abstractions;
using Mediate.Samples.AspNetCore.Autofac.Models;
using Mediate.Samples.Shared.Event;
using Mediate.Samples.Shared.EventWithMiddleware;
using Mediate.Samples.Shared.Query;
using Mediate.Samples.Shared.QueryWithMiddleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Mediate.Samples.AspNetCore.Autofac.Controllers
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

            SampleQueryResponse res = await _mediator.Send<SampleQuery, SampleQueryResponse>(query, cancellationToken);

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

            SampleComplexQueryResponse res = await _mediator.Send<SampleComplexQuery, SampleComplexQueryResponse>(query, cancellationToken);

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
