using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SampleWebApi
{
    public class EventService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public EventService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public List<Event> GetEvents()
        {
            var rootPath = _hostingEnvironment.ContentRootPath;
            using (StreamReader r = new StreamReader(rootPath + "/events.json"))
            {
                string json = r.ReadToEnd();
                var events = JsonConvert.DeserializeObject<List<Event>>(json);

                return events;
            }
        }
    } 
}
