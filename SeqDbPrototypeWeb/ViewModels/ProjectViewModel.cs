using SeqDbPrototypeWeb.Models;

namespace SeqDbPrototypeWeb.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Abbreviation { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int ExperimentCount { get; set; }

        public List<Experiment>? Experiments { get; set; }

    }
}
