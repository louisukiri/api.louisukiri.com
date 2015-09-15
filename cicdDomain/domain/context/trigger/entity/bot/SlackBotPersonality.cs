using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.values;

namespace cicd.domain.context.trigger.entity.bot
{
    public class SlackBotPersonality : IPersonality
    {
        private IDictionary<string, IList<string>> TermMap= new Dictionary<string, IList<string>>
        {
            {"pronoun-me",new List<string>{"I"}},
            {"verb-me", new List<string>{"I am"}},
            {"verb-run", new List<string>{"running","executing","triggering"}},
            {"verb-ran", new List<string>{"ran","executed","triggered"}},
            {"hello", new List<string>{"hello","hi","hey"}},
            {"verb-tested", new List<string>{"tested"}},
            {"verb-test", new List<string>{"testing"}},
            //conjugation present tense of to-be
            {"conj-pres-me", new List<string>{"I am"}}

        };

        public SlackBotPersonality()
        {
        }

        public SlackBotPersonality(params KeyValuePair<string, IList<string>>[] maps)
        {
            foreach (var map in maps)
            {
                TermMap.AddUpdate(map.Key, map.Value);
            }
        }
        public string Verb(EventType eventType)
        {
            return this[eventType];
        }
        public string this[string index]
        {
            get
            {
                return TermMap[index].Random();
            }
        }

        public string this[EventType eventType]
        {
            get
            {
                switch (eventType)
                {
                    case EventType.Build:
                        return "building";
                    case EventType.TestResults:
                        return "testing";
                    case EventType.Deploy:
                        return "deploying";
                    default:
                        return string.Empty;
                }
            }
        }
        public string Talk(string content)
        {
            return content??string.Empty;
        }
        //Returns a conjugated first person for verb to-be
        public string ConjMe(EventType type)
        {
            return "I am";
        }
        public string PrepositionAt(EventType type)
        {
            return "at";
        }


        public string ActivityNoun(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.Build:
                    return "The Build";
                case EventType.TestResults:
                    return "The Results";
            }
            return string.Empty;
        }
        public string EventAction(IEvent Event)
        {
            var eventType = Event.Type;
            switch (eventType)
            {
                case EventType.Build:
                    return "building "+ Event.What + ((!string.IsNullOrWhiteSpace(Event.Where))?" at "+Event.Where:string.Empty);
                case EventType.TestResults:
                    return "running tests";
                case EventType.Deploy:
                    return "deploying" + " " + Event.What + (!string.IsNullOrWhiteSpace(Event.Destination)?" to " + Event.Destination:string.Empty);
                case EventType.PreTest:
                    return string.Format("beginning tests on {0} at {1}. Please hold for results...", Event.What, Event.Where);
            }
            return string.Empty;
        }


        public string PrepositionTo(EventType eventType)
        {
            return "to";
        }
    }

    public static class ListExtensions
    {
        public static void AddUpdate<j>(this IDictionary<string, j> dictionary, string key, j value)
        {

            key = key.ToLower();
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }
        public static T Random<T>(this IList<T> list)
        {
            int randomIndex = new Random(Guid.NewGuid().GetHashCode()).Next(0, list.Count);
            return list[randomIndex];
        }
    }
}
