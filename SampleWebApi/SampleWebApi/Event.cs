using System.Collections.Generic;

namespace SampleWebApi
{
    public class Event
    {
        public Event()
        {
            Location = new Location();
            Sessions = new List<Session>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public string OnlineUrl { get; set; }
        public Location Location { get; set; }
        public List<Session> Sessions { get; set; }
    }

    public class Session
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Presenter { get; set; }
        public int Duration { get; set; }
        public string Level { get; set; }
        public string Abstract { get; set; }
        public string[] Voters { get; set; }
        public int EventId { get; set; }
    }

    public class Location
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}