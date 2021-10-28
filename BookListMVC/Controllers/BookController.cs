using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BookListMVC.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public Book Book { get; set; }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Book = new Book();
            if(id == null)
            {
                // create view
                return View(Book);
            }
            // update view
            Book = _db.Book.FirstOrDefault(u => u.Id == id);
            if(Book == null)
            {
                return NotFound();
            }
            return View(Book);
        }

        [HttpPost]
        public IActionResult Upsert()
        {
            if(ModelState.IsValid)
            {
                if(Book.Id == 0)
                {
                    // create
                    _db.Book.Add(Book);
                } else
                {
                    _db.Book.Update(Book);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Book);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Book.ToListAsync() });
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = await _db.Book.FirstOrDefaultAsync(u => u.Id == id);
            if(bookFromDb == null)
            {
                return Json(new { success = false, message = "Not Found!" });
            }
            _db.Book.Remove(bookFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message="Delete successfully!" });
        }
        #endregion

    }
}
