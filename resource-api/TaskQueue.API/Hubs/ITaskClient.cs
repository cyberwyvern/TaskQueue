using System.Threading.Tasks;
using TaskQueue.API.Models;

namespace TaskQueue.API.Hubs
{
    public interface ITaskClient
    {
        Task TaskChanged(TaskModel taskEntity);
    }
}