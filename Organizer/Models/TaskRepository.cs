using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Organizer.Models
{
    public class TaskRepository : ITaskRepository
    {
        private readonly OrganizerContext context;

        public TaskRepository(OrganizerContext context)
        {
            this.context = context;
        }

        public async Task<Date> AddDate(Date date)
        {
            var result = await context.Dates.AddAsync(date);
            return result.Entity;
        }

        public async Task<Task> AddTask(Task task)
        {
            var result = await context.Tasks.AddAsync(task);
            return result.Entity;
        }

        public Date DeleteDate(Date date)
        {
            var result = context.Dates.Remove(date);
            return result.Entity;
        }

        public Task DeleteTask(Task task)
        {
            var result = context.Tasks.Remove(task);
            return result.Entity;
        }

        public async Task<Date> GetDate(DateTime dateTime)
        {
            return await context.Dates.FirstOrDefaultAsync(d => d.TimeStamp == dateTime);
        }

        public async Task<Task> GetTask(int id)
        {
            return await context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<int> Save()
        {
            return await context.SaveChangesAsync();
        }
    }
}
