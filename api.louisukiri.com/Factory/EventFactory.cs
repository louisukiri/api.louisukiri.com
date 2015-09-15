using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.events;
using cicd.domain.context.trigger.values;

namespace godaddy.domains.cicd.Factory
{
    public class EventFactory
    {
        public static IEvent FromGenericApiEvent(Models.GenericApiEvent genericApiEvent)
        {
            switch (genericApiEvent.Type.ToLower())
            {
                case "deploy":
                    return new DeployEvent
                    {
                        What = genericApiEvent.What,
                        Where = genericApiEvent.Where,
                        Name = genericApiEvent.Name,
                        Destination = genericApiEvent.Destination
                    };
                case "pretest":
                    return new GenericEvent
                    {
                        What = genericApiEvent.What,
                        Where = genericApiEvent.Where,
                        Name = genericApiEvent.Name,
                        Destination = genericApiEvent.Destination,
                        Results = genericApiEvent.Results,
                        Type = EventType.PreTest
                    };
                    break;
                default:
                    return null;
            }
        }
    }
}