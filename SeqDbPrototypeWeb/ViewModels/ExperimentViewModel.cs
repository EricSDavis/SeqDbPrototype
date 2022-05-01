using SeqDbPrototypeWeb.Models;

namespace SeqDbPrototypeWeb.Controllers
{
    internal class ExperimentViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string Owner { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}