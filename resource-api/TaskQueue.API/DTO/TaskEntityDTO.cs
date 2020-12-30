using System;
using TaskQueue.DAO.Entities;

namespace TaskQueue.API.DTO
{
    public class TaskEntityDTO
    {
        public int Id { get; set; }

        public DateTime? StartedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public TaskStatus Status { get; set; }
    }
}