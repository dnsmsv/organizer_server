using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Organizer.Models;

namespace Organizer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors("AllowSpecificOrigin")]
    public class OrganizerController : ControllerBase
    {
        private readonly OrganizerContext _context;

        public OrganizerController(OrganizerContext context)
        {
            _context = context;
        }

        // GET: Organizer
        [HttpGet]
        public async Task<ActionResult<string>> GetDates()
        {
            var dates = await _context.Dates.ToListAsync();
            string json = string.Empty;
            dates.ForEach(d => json += JsonConvert.SerializeObject(d));
            return json;
        }

        // GET: Organizer/12-09-2020
        [EnableCors("AllowSpecificOrigin")]
        [HttpGet("{dateTimeAsString}.json")]
        public async Task<ActionResult<string>> GetDate(string dateTimeAsString)
        {
            var dateTime = DateTime.Parse(dateTimeAsString);
            var date = await _context.Dates.FirstOrDefaultAsync(d => d.TimeStamp == dateTime);

            if (date != null)
            {
                var res = new Dictionary<int, Response>();

                foreach (var task in date.Tasks)
                    res.Add(task.Id, new Response(date.TimeStamp.ToString("dd-MM-yyyy"), task.Title));

                return JsonConvert.SerializeObject(res);
            }

            return null;
        }

        // PUT: api/Organizer/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{dateTimeAsString}.json")]
        //[EnableCors("AllowSpecificOrigin")]
        //[HttpPut]
        //public async Task<IActionResult> PutDate(string dateTimeAsString, Response response)
        //{
        //    var dateTime = DateTime.Parse(dateTimeAsString);
        //    var date = await _context.Dates.FirstOrDefaultAsync(d => d.TimeStamp == dateTime);

        //    //if (date == null)
        //    //{
        //    //    var addingDate = new Date
        //    //    {
        //    //        TimeStamp = dateTime,
        //    //        Tasks = new List<Models.Task>()
        //    //    };
        //    //    addingDate.Tasks.Add(new Models.Task { Date = addingDate, Title = response.title });
        //    //    _context.Dates.Add(addingDate);
        //    //}

        //    //await _context.SaveChangesAsync();
        //    return new EmptyResult(); //NoContent();
        //}

        // POST: api/Organizer
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [EnableCors("AllowSpecificOrigin")]
        [HttpPost("{dateTimeAsString}.json")]
        public async Task<ActionResult<Response>> PostDate(string dateTimeAsString, Response response)
        {
            var dateTime = DateTime.Parse(dateTimeAsString);
            var date = await _context.Dates.FirstOrDefaultAsync(d => d.TimeStamp == dateTime);

            if (date == null)
            {
                var addingDate = new Date
                {
                    TimeStamp = dateTime,
                    Tasks = new List<Models.Task>()
                };
                _context.Tasks.Add(new Models.Task { Date = addingDate, Title = response.title });
                _context.Dates.Add(addingDate);
            }
            else
            {
                _context.Tasks.Add(new Models.Task { Date = date, Title = response.title });
            }

            await _context.SaveChangesAsync();
            return response;
        }

        // DELETE: api/Organizer/5
        [HttpDelete("{id}.json")]
        public async Task<ActionResult<Date>> DeleteDate(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound();

            Date date = task.Date;
            _context.Tasks.Remove(task);

            if (!date.Tasks.Any())
                _context.Dates.Remove(date);

            await _context.SaveChangesAsync();

            return date;
        }

        private bool DateExists(DateTime id)
        {
            return _context.Dates.Any(e => e.TimeStamp == id);
        }
    }
}
