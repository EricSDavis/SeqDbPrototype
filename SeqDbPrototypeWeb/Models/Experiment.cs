using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SeqDbPrototypeWeb.Models
{
    public class Experiment
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public string Owner { get; set; }
        [Required]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
