using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SeqDbPrototypeWeb.Models
{
    //[Index(nameof(Abbreviation), IsUnique = true)]
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Abbreviation { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public List<Experiment>? Experiments { get; set; }
    }
}
