using cicd.domain.context.trigger.values;

namespace cicd.domain.context.trigger.abstracts
{
    public interface IPersonality
    {
        string Verb(EventType eventType);
        string this[string index] { get;}
        string this[EventType eventType] { get;}
        string Talk(string content);
        string ConjMe(EventType type);
        string PrepositionAt(EventType type);
        string ActivityNoun(EventType eventType);
        string PrepositionTo(EventType eventType);
        string EventAction(IEvent Event);
    }
}
