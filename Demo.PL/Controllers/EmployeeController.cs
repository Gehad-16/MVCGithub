using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IEmployeeRepository employeeRepository , IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }
        public IActionResult Index(string searchvalue)
        {
            if(string.IsNullOrEmpty(searchvalue))
            {var employees = _employeeRepository.GetAll();
                return View(employees);
            }
            else 
            {
                var employees = _employeeRepository.GetEmployeesByName(searchvalue);
                return View(employees);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DepatnmentsViewBag = _departmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employeeRepository.Add(employee);
                return RedirectToAction("Index");
            }
            return View(employee);
        }
        [HttpGet]
        public IActionResult Details(int? id, string action = "Details")
        {
            if (id == null)
            {
                return BadRequest();
            }
            var employee = _employeeRepository.GetById(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            return View(action, employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        public IActionResult Edit(Employee employee, [FromRoute] int? id)
        {
            if (employee.Id != id)
            {

                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _employeeRepository.Update(employee);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(employee);

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(Employee employee, [FromRoute] int? id)
        {
            if (employee.Id == id)
            {
                try
                {
                    _employeeRepository.Delete(employee);
                    return RedirectToAction(nameof(Index));

                }
                catch (System.Exception ex)
                {
                    //form
                    //string.Empty : the key is empty
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(employee);
                }
            }
            return BadRequest();
        }
    }
}