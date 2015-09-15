using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using cicd.domain.context.trigger.entity;
using Newtonsoft.Json;

namespace cicd.domain.context.trigger.factory
{
    public static class SettingsFactory
    {
        public static Settings SettingsFromJson(string json)
        {
            try
            {
                Settings settings = JsonConvert.DeserializeObject<Settings>(json);
                
                return settings;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
