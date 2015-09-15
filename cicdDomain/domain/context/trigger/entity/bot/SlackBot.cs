using System;
using cicd.domain.context.trigger.abstracts;
using cicd.domain.context.trigger.services;
using cicd.domain.context.trigger.services.domain;
using cicd.infrastructure;

namespace cicd.domain.context.trigger.entity.bot
{
    public class SlackBot : IBot
    {
        public IBrain Brain { get; private set; }
        public string Name { get; private set; }
        private readonly SlackTalkService _talkService = new SlackTalkService();
        public SlackBot()
        {
            Brain = new SlackBotBrain(this, new SlackBotPersonality());
            Name = "Wargs-Bot";
        }
        public SlackBot(IBrain brain)
        {
            Brain = brain;
            Name = "Wargs-Bot";
        }

        public void Trigger(IEvent Event)
        {
            if (Event == null)
            {
                throw new ArgumentException("event");
            }
            Brain.Process(Event);
        }
        public void Say(string text)
        {
            _talkService.Send(AppSettings.SlackWebHookUri, Name, text);
        }
    }
}
