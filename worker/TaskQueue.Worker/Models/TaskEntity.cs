using System;

namespace TaskQueueWorker
{
    public class TaskEntity
    {
        public int Id { get; set; }

        public DateTime? StartedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public TaskStatus Status { get; set; }
    }
}