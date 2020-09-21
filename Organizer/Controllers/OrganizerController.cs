using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class OrganizerController : ControllerBase
    {
        private readonly ITaskRepository taskRepository;

        public OrganizerController(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        [HttpGet("{dateTimeAsString}.{format?}")]
        public async Task<ActionResult<Dictionary<int, Response>>> GetTask(string dateTimeAsString)
        {
            try
            {
                var dateTime = DateTime.Parse(dateTimeAsString);
                Date date = await taskRepository.GetDate(dateTime);

                if (date != null)
                {
                    var res = new Dictionary<int, Response>();

                    foreach (var task in date.Tasks)
                        res.Add(task.Id, new Response(date.TimeStamp.ToString("dd-MM-yyyy"), task.Title));

                    return Ok(JsonConvert.SerializeObject(res));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error getting data from the database");
            }

            return null;
        }

        [HttpPost("{dateTimeAsString}.{format?}")]
        public async Task<ActionResult<Response>> PostTask(string dateTimeAsString, Response response)
        {
            try
            {
                if (response == null)
                    return BadRequest();

                var dateTime = DateTime.Parse(dateTimeAsString);
                var date = await taskRepository.GetDate(dateTime);
                Models.Task task;

                if (date == null)
                {
                    var addingDate = new Date
                    {
                        TimeStamp = dateTime,
                        Tasks = new List<Models.Task>()
                    };
                    task = new Models.Task { Date = addingDate, Title = response.title };
                    await taskRepository.AddDate(addingDate);
                    await taskRepository.AddTask(task);
                }
                else
                {
                    task = new Models.Task { Date = date, Title = response.title };
                    await taskRepository.AddTask(task);
                }

                await taskRepository.Save();
                return CreatedAtRoute(new { id = dateTimeAsString }, new { name = task.Id, response });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Task adding to database error");
            }
        }

        [HttpDelete("{dateTimeAsString}/{id}.json")]
        public async Task<ActionResult<Models.Task>> DeleteTask(int id)
        {
            try
            {
                var task = await taskRepository.GetTask(id);

                if (task == null)
                    return NotFound();

                taskRepository.DeleteTask(task);

                if (task.Date.Tasks.Count == 1)
                    taskRepository.DeleteDate(task.Date);

                await taskRepository.Save();
                return task;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Task deleting error");
            }
        }
    }
}
