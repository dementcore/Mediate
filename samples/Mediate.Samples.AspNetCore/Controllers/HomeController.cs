using Mediate.Core;
using Mediate.Samples.AspNetCore.Models;
using Mediate.Samples.Shared;
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
            TestMsg test = new TestMsg() { Data = "Test Data" };

            TestMsgReply res = await _mediator.Send<TestMsg,TestMsgReply>(test, cancellationToken);

            ViewBag.TestMsg = res.Reply;

            return View();
        }

        public async Task<IActionResult> Privacy(CancellationToken cancellationToken)
        {
            OnHomeInvoked @event = new OnHomeInvoked() { TestData = Activity.Current.Id };

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
