using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cicd.infrastructure
{
    [ExcludeFromCodeCoverage]
    public class AppSettings
    {
        public static string AuthToken
        {
            get { return "4d5b313a7b53ad08387849bd8bb6f9999e1ced6b"; }
        }

        public static Uri SlackWebHookUri
        {
            get { return new Uri("https://hooks.slack.com/services/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"); }
        }
    }
}
