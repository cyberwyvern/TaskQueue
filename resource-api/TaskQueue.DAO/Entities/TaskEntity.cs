using System;
using System.ComponentModel.DataAnnotations;
using TaskQueue.DAO.Interface;

namespace TaskQueue.DAO.Entities
{
    public class TaskEntity : IEntity<int>
    {
        public int Id { get; set; }

        public int SequenceNumber { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        public int UserProfileId { get; set; }

        [Required]
        public UserProfile UserProfile { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? StartedDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        [Required]
        public TaskStatus Status { get; set; }
    }
}