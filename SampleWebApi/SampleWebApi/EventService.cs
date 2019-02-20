using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public void SaveEvents(List<Event> events)
        {
            var data = JsonConvert.SerializeObject(events);
            var rootPath = _hostingEnvironment.ContentRootPath;
            File.WriteAllText(rootPath + "/events.json", data);
        }

        public List<Session> SearchSessions(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();
            var events = GetEvents();
            var filteredSessions = events.SelectMany(e =>
            {
                e.Sessions.ForEach(s => s.EventId = e.Id);
                return e.Sessions;
            }).Where(s => s.Name.ToLower().Contains(searchTerm)
                            || s.Abstract.ToLower().Contains(searchTerm))
                            .ToList();

            return filteredSessions;
        }

        public Event GetEvent(int id)
        {
            return GetEvents().FirstOrDefault(e => e.Id == id);
        }

        public int SaveEvent(Event value)
        {
            var events = GetEvents();
            if (value.Id > 0)
            {
                value.Sessions = events.FirstOrDefault(e => e.Id == value.Id).Sessions;
                events.Remove(events.Find(e => e.Id == value.Id));
            }
            else
            {
                value.Id = events.OrderByDescending(i => i.Id).FirstOrDefault().Id + 1;
            }

            events.Add(value);
            SaveEvents(events);

            return value.Id;
        }
    }
}
