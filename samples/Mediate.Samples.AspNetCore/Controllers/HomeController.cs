using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mediate.Samples.AspNetCore.Models;
using Mediate.Samples.AspNetCore.Mediate;
using System.Threading;
using Mediate.Abstractions;

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
            OnHomeInvoked @event = new OnHomeInvoked() { RequestId = Activity.Current.Id };

            await _mediator.Dispatch(@event,DispatchPolicy.Queued, cancellationToken);

            TestMsg test = new TestMsg() { Data = "Test Data" };

            TestMsgReply res = await _mediator.Send(test, cancellationToken);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
