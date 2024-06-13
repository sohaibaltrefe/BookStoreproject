using BookStore.Data;
using BookStore.Models;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{        [Authorize]

    public class BooksController : Controller
    {

        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BooksController(ApplicationDbContext context,IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var books = context.Books.Include(book=>book.Author).Include(book => book.Categories).ThenInclude(book => book.category).ToList();

            var bookVMs = books.Select(book => new BookVM
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author.Name,
                Publisher = book.Publisher,
                PublishData = book.PublishData,
                ImageUrl = book.ImageUrl,
                Categories = book.Categories.Select(book=>book.category.Name).ToList(),
            }).ToList();



            return View(bookVMs);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var authors = context.Authors.OrderBy(author => author.Name).ToList();
            var categories = context.categories.OrderBy(category => category.Name).ToList();

            var authorList = authors.Select(author => new SelectListItem
            {
                Value = author.Id.ToString(),
                Text = author.Name
            }).ToList();

            var categoryList = categories.Select(category => new SelectListItem
            {
                Value = category.Id.ToString(),
                Text = category.Name
            }).ToList();

            var viewModel = new BookFormVM
            {
                Authors = authorList,
                Categories = categoryList,
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(BookFormVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var existingBook = context.Books
               .FirstOrDefault(t => t.Title == viewModel.Title );

            if (existingBook != null)
            {
                ModelState.AddModelError("Title", " no create book name already exists");
                return View("Create", viewModel);
            }

            string imageName = null;
            if (viewModel.ImageUrl != null)
            {
                imageName = Path.GetFileName(viewModel.ImageUrl.FileName);
                var path = Path.Combine($"{webHostEnvironment.WebRootPath}/img/books", imageName);
                var stream = System.IO.File.Create(path);
               
                    viewModel.ImageUrl.CopyTo(stream);
                
            }

            var book = new Book
            {
                Title = viewModel.Title,
                AuthorId = viewModel.AuthorId,
                Publisher = viewModel.Publisher,
                PublishData = viewModel.PublishData,
                ImageUrl = imageName,
                Description = viewModel.Description,
                Categories = viewModel.SelectedCategories.Select(id => new BookCategory
                {
                    CategoryId = id,
                }).ToList(),
            };

            context.Books.Add(book);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

     



        public IActionResult Details(int id)
        {
            var book = context.Books
                .Include(BookAut => BookAut.Author)
                .Include(Bookcate => Bookcate.Categories)
                    .ThenInclude(Bookcate => Bookcate.category)
                .First(b => b.Id == id);

            if (book is null)
            {
                return NotFound();
            }

            var bookVM = new BookVM
            {
                Id = book.Id,   
                Title = book.Title,
                Author = book.Author.Name, 
                Publisher = book.Publisher, 
                PublishData = book.PublishData,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                Categories = book.Categories.Select(Bookcate => Bookcate.category.Name).ToList(), 
            };

            return View(bookVM); 
        }


        public IActionResult Delete(int id)
        {
            var book = context.Books.Find(id);
            if (book is null)
            {
                return NotFound();
            }
            if(book.ImageUrl != null) { 
            var path = Path.Combine(webHostEnvironment.WebRootPath,"img/books", book.ImageUrl);
            if(System.IO.File.Exists(path))
            {
            System.IO.File.Delete(path);

            }}
            context.Books.Remove(book);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
