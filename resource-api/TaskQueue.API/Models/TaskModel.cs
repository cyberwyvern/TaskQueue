namespace TaskQueue.API.Models
{
    public class TaskModel
    {
        public int Id { get; set; }

        public int SequenceNumber { get; set; }

        public string Title { get; set; }

        public string CreatedBy { get; set; }

        public long CreatedDate { get; set; }

        public long? DurationMs { get; set; }

        public string Status { get; set; }
    }
}
