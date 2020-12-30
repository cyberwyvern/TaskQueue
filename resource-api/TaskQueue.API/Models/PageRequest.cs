using System.ComponentModel.DataAnnotations;

namespace TaskQueue.API.Models
{
    public class PageRequest
    {
        [Range(0, int.MaxValue)]
        public int PageIndex { get; set; }

        [Range(1, 50)]
        public int PageSize { get; set; }
    }
}