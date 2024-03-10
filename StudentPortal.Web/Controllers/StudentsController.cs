using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;

namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly DataContext _dataContext;

        public StudentsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
            var student = new Student
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Subscribed = viewModel.Subscribed
            };

            await _dataContext.AddAsync(student);
            await _dataContext.SaveChangesAsync();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var students = await _dataContext.Students.ToListAsync();
            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await _dataContext.Students.FindAsync(id);

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult>Edit(Student viewModel)
        {
            var student = await _dataContext.Students.FindAsync(viewModel.Id);

            if(student is not null)
            {
                student.Name = viewModel.Name;
                student.Email = viewModel.Email;
                student.Phone = viewModel.Phone;
                student.Subscribed = viewModel.Subscribed;

                await _dataContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Student viewModel)
        {
            var student = _dataContext.Students.
                AsNoTracking().FirstOrDefaultAsync(s=>s.Id==viewModel.Id);

            if (student!=null)
            {
                _dataContext.Remove(viewModel);
                await _dataContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }
    }
}
