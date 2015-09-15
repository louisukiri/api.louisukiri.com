using System;
using System.Net.Http;

namespace cicd.domain.context.trigger.services.domain
{
    public class SlackTalkService
    {
        public void Send(Uri uri, string user, string text)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = uri;
                var contentString = string.Format("{{\"username\":\"{0}\",\"text\":\"{1}\"}}", user, text);
                
                var req = new HttpRequestMessage(HttpMethod.Post, uri);
                req.Content = new StringContent(contentString);

                HttpResponseMessage msg = client.SendAsync(req)
                    .Result;
            }
        }
    }
}
