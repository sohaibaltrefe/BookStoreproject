using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext context;

       public AuthorsController (ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var authorsVm = context.Authors
        .Select(author => new AuthorVM
        {
            Id = author.Id,
            Name = author.Name,
            CreatedOn = author.CreatedOn,
            UpdateOn = author.UpdateOn,
        })
        .ToList();
            return View(authorsVm);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View("Form");
        }
        [HttpPost]
        public IActionResult Create(AuthorFormVM authorFormVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", authorFormVM);
            }
            var existingAuthor = context.Authors
               .FirstOrDefault(a => a.Name == authorFormVM.Name);

            if (existingAuthor != null)
            {
                ModelState.AddModelError("Name", " no create Author name already exists");
                return View("Form", authorFormVM);
            }
            var Authors = new Author
            {
                Name = authorFormVM.Name,
            };
            try
            {
                context.Authors.Add(Authors);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("Name", "category name already exists");
                return View(authorFormVM);
            }

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var author = context.Authors.Find(id);

            if (author is null) { return NotFound(); }
            var viewModel = new AuthorFormVM
            {
                Id = id,
                Name = author.Name

            };
            return View("Form", viewModel);
        }
        [HttpPost]
        public IActionResult Edit(AuthorFormVM authorFormVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", authorFormVM);
            }
            var author = context.Authors.Find(authorFormVM.Id);
            if (author == null) { return NotFound(); }
            var existingAuthor = context.Authors
               .FirstOrDefault(a => a.Name == authorFormVM.Name && a.Id != authorFormVM.Id);

            if (existingAuthor != null)
            {
                ModelState.AddModelError("Name", " no update Author name already exists");
                return View("Form", authorFormVM);
            }

            author.Name = authorFormVM.Name;
            author.UpdateOn = DateTime.Now;
            context.SaveChanges();
            return RedirectToAction("Index");



        }
        public IActionResult Details(int id)
        {
            var Author = context.Authors.Find(id);
            if (Author is null) { return NotFound(); }
            var viewModel = new AuthorVM
            {
                Id = Author.Id,
                Name = Author.Name,

                CreatedOn = Author.CreatedOn,
                UpdateOn = Author.UpdateOn
            };
            return View(viewModel);
        }
        public IActionResult Delete(int id)
        {
            var Authorss = context.Authors.Find(id);

            if (Authorss is null) { return NotFound(); }

            context.Authors.Remove(Authorss);
            context.SaveChanges();
            return Ok();
        }
    }   
}
