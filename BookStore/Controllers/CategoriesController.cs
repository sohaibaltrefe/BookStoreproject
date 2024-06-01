using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext context;

        public CategoriesController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var categories = context.categories.ToList();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return View("create", categoryVM);
            }
            var existingCategory = context.categories
               .FirstOrDefault(c => c.Name == categoryVM.Name);

            if (existingCategory != null)
            {
                ModelState.AddModelError("Name", "Category name already exists");
                return View("Create", categoryVM);
            }
            var category = new Category
            {
                Name = categoryVM.Name,
            };
            context.categories.Add(category);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = context.categories.Find(id);

            if (category is null) { return NotFound(); }

            var viewModel = new CategoryVM
            {
                Id = id,
                Name = category.Name

            };
            return View("Create", viewModel);
        }
        [HttpPost]
        public IActionResult Edit(CategoryVM categoryVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", categoryVM);
            }
            var category = context.categories.Find(categoryVM.Id);
            if (category == null) { return NotFound(); }
            var existingCategory = context.categories
                .FirstOrDefault(c => c.Name == categoryVM.Name && c.Id != categoryVM.Id);

            if (existingCategory != null)
            {
                ModelState.AddModelError("Name", "Category name already exists");
                return View("Create", categoryVM);
            }
            category.Name = categoryVM.Name;
            category.UpdateOn = DateTime.Now;
            context.SaveChanges();
            return RedirectToAction("Index");



        }
        public IActionResult Details(int id)
        {
            var category = context.categories.Find(id);
            if (category is null) { return NotFound(); }
            var viewModel = new CategoryVM
            {
                Id = category.Id,
                Name = category.Name,

                CreatedOn = category.CreatedOn,
                UpdateOn = category.UpdateOn
            };
            return View(viewModel);
        }
        public IActionResult Delete(int id)
        {
            var category = context.categories.Find(id);

            if (category is null) { return NotFound(); }

            
            context.categories.Remove(category);
            context.SaveChanges();
            return Ok();
        }
    }
    
}