using System;
using System.Collections;
using System.Runtime.InteropServices;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.values;
using System.Collections.Generic;

namespace cicd.domain.context.trigger.entity.bot
{
    public class SlackBotBrain : IBrain
    {
        public IBot Bot { get; private set; }
        public IPersonality Personality { get; set; }
        public SlackBotBrain(IBot bot, IPersonality personality)
        {
            Bot = bot;
            Personality = personality;
        }
        public void Process(IEvent Event)
        {
            //SendTalkCommand(Personality["verb-me"] + " " + Personality.Verb(Event.Type) +" "+Event.Name);
            //var template = Event.Run();
            //var expandedTemplate = ExpandTemplate(template);
            //if (!string.IsNullOrWhiteSpace(expandedTemplate))
            //{
            //    SendTalkCommand(Personality["verb-me"] + " " + expandedTemplate);
            //}
            if (Event == null)
            {
                return;
            }
            if (isResultsEvent(Event))
            {
                var resultMessage = string.Format("{0} of {1} are:\n{2}", Personality.ActivityNoun(Event.Type), Personality[Event.Type],Event.Results.Flatten(":"));
                Bot.Say(resultMessage.Trim());
            }
            else
            {
                var message = string.Format("{0} {1}", Personality.ConjMe(Event.Type), Personality.EventAction(Event));
                Bot.Say(message.TrimEnd());
            }
        }
        private bool isResultsEvent(IEvent Event)
        {
            return Event.Type == EventType.TestResults && Event.Results != null;
        }
    }

    public static class IDictionaryExtension
    {
        public static string Flatten(this IDictionary<string, string> dictionary, string delim)
        {
            var result = string.Empty;
            foreach (var key in dictionary.Keys)
            {
                var value = dictionary[key];
                result += string.Format("{0}{2}{1}\n",key, value,delim);
            }
            return result;
        }
    }
}
