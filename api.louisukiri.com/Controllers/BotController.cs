using System.Net;
using System.Net.Http;
using System.Web.Http;
using cicd.domain.context.trigger.abstracts;
using godaddy.domains.cicd.Factory;
using godaddy.domains.cicd.Models;

namespace godaddy.domains.cicd.Controllers
{
    public class BotController : ApiController
    {
        public IBot Bot;
        public BotController(IBot bot)
        {
            Bot = bot;
        }
        [Route("api/v1/event"), HttpPost]
        public HttpResponseMessage Post(GenericApiEvent apiEvent)
        {
            if (apiEvent == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            var Event = EventFactory.FromGenericApiEvent(apiEvent);
            if (Event == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            Bot.Trigger(Event);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}