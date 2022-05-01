using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeqDbPrototypeWeb.Data;
using SeqDbPrototypeWeb.Models;
using SeqDbPrototypeWeb.ViewModels;

namespace SeqDbPrototypeWeb.Controllers
{
    public class ProjectController : Controller
    {

        private readonly ApplicationDbContext _db;

        public ProjectController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<ProjectViewModel> projects = _db.Project.Select(u => new ProjectViewModel
            {
                Id = u.Id,
                Abbreviation = u.Abbreviation,
                Name = u.Name,
                Description = u.Description,
                ExperimentCount = u.Experiments == null ? 0 : u.Experiments.Count()
            }) ;
            return View(projects);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project obj)
        {

            //Ensure no duplicate abbreviations exist
            //in the database

            bool duplicateAbbreviation = _db.Project
                .Select(x => x.Abbreviation)
                .Contains(obj.Abbreviation);

            if (duplicateAbbreviation)
            {
                ModelState.AddModelError("Abbreviation",
                    "This project abbreviation already exists.");
            }

            if (ModelState.IsValid)
            {
                _db.Project.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Project created successfully";
                return RedirectToAction("Index");
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
        public IActionResult Edit(Project obj)
        {

            //Ensure no duplicate abbreviations exist
            //in the database; allow updating without
            //changing the abbreviation.

            bool duplicateAbbreviation = _db.Project
                .Select(x => x.Abbreviation)
                .Contains(obj.Abbreviation);

            if (duplicateAbbreviation)
            {
                //Project temp = _db.Project.AsNoTracking().First(u => u.Abbreviation == obj.Abbreviation);
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
                _db.Project.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Project updated successfully";
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

            var project = _db.Project.FirstOrDefault(u => u.Id == id);

            return View(project);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {

            var obj = _db.Project.FirstOrDefault(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _db.Project.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Project deleted successfully";
            return RedirectToAction("Index");

        }
    }
}
