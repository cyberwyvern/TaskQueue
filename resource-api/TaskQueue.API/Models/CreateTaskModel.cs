using System.ComponentModel.DataAnnotations;

namespace TaskQueue.API.Models
{
    public class CreateTaskModel
    {
        [Required]
        [StringLength(256)]
        public string Title { get; set; }
    }
}