using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SampleWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventService _service;

        public EventsController(IHostingEnvironment hostingEnvironment)
        {
            _service = new EventService(hostingEnvironment);
        }
        
        [HttpGet]
        public ActionResult<List<Event>> Get()
        {
            return _service.GetEvents();
        }
        
        [HttpGet("{id}")]
        public ActionResult<Event> Get(int id)
        {
            return _service.GetEvent(id);
        }

        [HttpGet("sessions")]
        public ActionResult<List<Session>> SearchSessions(string searchTerm)
        {
            return _service.SearchSessions(searchTerm);
        }

        [HttpPost]
        public ActionResult<int> Post([FromBody] Event value)
        {
            return _service.SaveEvent(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
