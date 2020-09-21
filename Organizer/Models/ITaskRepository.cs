using System;
using System.Threading.Tasks;

namespace Organizer.Models
{
    public interface ITaskRepository
    {
        Task<Date> GetDate(DateTime dateTime);
        Task<Models.Task> GetTask(int id);
        Task<Date> AddDate(Date date);
        Task<Task> AddTask(Task task);
        Date DeleteDate(Date date);
        Task DeleteTask(Task task);
        Task<int> Save();

    }
}
