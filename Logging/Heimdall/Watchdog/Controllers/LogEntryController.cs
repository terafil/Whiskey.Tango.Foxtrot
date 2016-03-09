using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Watchdog.Engine;
using Watchdog.Models;

namespace Watchdog.Controllers
{
    [RoutePrefix("api/v1/logentries")]
    public class LogEntryController : ApiController
    {
        private IObserver<ApplicationLogEntry> Bifrost {get; set;}
        public LogEntryController(IObserver<ApplicationLogEntry> bifrost)
        {
            Bifrost = bifrost;
        }
        [Route("{app}")]
        [HttpGet]
        public IHttpActionResult GetEntries(string app, [FromUri] LogEntryQuery query)
        {
            return Ok();
        }
        [Route("{app}/{logLevel}")]
        [HttpGet]
        public IHttpActionResult GetEntriesForLogLevel(string app, LogLevel logLevel, [FromUri] LogEntryQuery query)
        {
            return Ok();
        }
        [Route("{app}/{logLevel}")]
        [HttpPost]
        public IHttpActionResult CreateEntry(string app, LogLevel logLevel, LogEntryModel model)
        {
            Bifrost.OnNext(new ApplicationLogEntry { ApplicationId = app, LogLevel = logLevel, Message = model.message, OccurredOn = model.occurredOn });

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Accepted);
            return new ResponseMessageResult(response);
        }
    }
}
