using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeqDbPrototypeWeb.Data;
using SeqDbPrototypeWeb.Models;

namespace SeqDbPrototypeWeb.Controllers
{
    public class ExperimentController : Controller
    {

        private readonly ApplicationDbContext _db;

        public ExperimentController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(int? ProjectId)
        {
            IEnumerable<ExperimentViewModel> experiments = _db.Experiment.Select(u => new ExperimentViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Description = u.Description,
                Owner = u.Owner,
                CreatedDateTime = u.CreatedDateTime,
                ProjectId = u.ProjectId,
                Project = u.Project
            })
                .OrderBy(u => u.Project.Abbreviation);
            
            //View by Project
            if (ProjectId != null)
            {
                ViewData["ProjectId"] = ProjectId;
                IEnumerable<ExperimentViewModel> projectSpecificExperiments = experiments.Where(u => u.ProjectId == ProjectId);
                return View(projectSpecificExperiments);
            }
            return View(experiments);
        }

        //GET
        public IActionResult Create(int? ProjectId)
        {
            PopulateProjectDropDownList(ProjectId);
            ViewData["ProjectId"] = ProjectId;
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Experiment obj)
        {
            PopulateProjectDropDownList();

            //Bind project to object via selected ProjectId
            obj.Project = _db.Project
                .Include(u => u.Experiments)
                .First(u => u.Id == obj.ProjectId);

            ModelState.ClearValidationState("Project");
            if (!TryValidateModel(obj))
            {
                return View(obj);
            }

            if (obj.Project.Experiments != null)
            {
                //Ensure no duplicate name exist in the database in the same project
                bool duplicateName = obj.Project.Experiments
                    .Select(u => u.Name)
                    .Contains(obj.Name);

                if (duplicateName)
                {
                    ModelState.AddModelError("Name",
                        "This experiment name already exists for " +
                        obj.Project.Abbreviation);
                }
            }
            
            if (ModelState.IsValid)
            {
                _db.Experiment.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Experiment created successfully";
                return RedirectToAction("Index", "Experiment", new { ProjectId = obj.ProjectId });
            }

            return View(obj);
            
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if(id==null || id==0)
            {
                return NotFound();
            }

            var project = _db.Project.FirstOrDefault(u => u.Id == id);

            
            return View(project);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Experiment obj)
        {

            //Ensure no duplicate abbreviations exist
            //in the database; allow updating without
            //changing the abbreviation.

            bool duplicateAbbreviation = _db.Project
                .Select(x => x.Abbreviation)
                .Contains(obj.Abbreviation);

            if (duplicateAbbreviation)
            {
                bool sameObject = _db.Project
                    .AsNoTracking()
                    .First(u => u.Abbreviation == obj.Abbreviation)
                    .Id.Equals(obj.Id);

                if (!sameObject)
                {
                    ModelState.AddModelError("Abbreviation",
                        "This project abbreviation already exists.");
                }

            }


            if (ModelState.IsValid)
            {
                _db.Experiment.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Experiment updated successfully";
                return RedirectToAction("Index");
            }

            return View(obj);

        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var experiment = _db.Experiment.FirstOrDefault(u => u.Id == id);

            return View(experiment);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {

            var obj = _db.Experiment.FirstOrDefault(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _db.Experiment.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Experiment deleted successfully";
            return RedirectToAction("Index", "Experiment", new { ProjectId = obj.ProjectId });

        }

        private void PopulateProjectDropDownList(int? selectedProjectId = null)
        {
            IEnumerable<Project> projects = _db.Project.OrderBy(u => u.Abbreviation);
            ViewBag.ProjectIds = new SelectList(projects, "Id", "Abbreviation", selectedProjectId);
        }
    }
}
