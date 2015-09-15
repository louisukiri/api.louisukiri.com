using System.Collections.Generic;

namespace godaddy.domains.cicd.Models
{
    public class GenericApiEvent
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string What { get; set; }
        public string Where { get; set; }
        public string Destination { get; set; }
        public IDictionary<string, string> Results { get; set; }
    }
}
