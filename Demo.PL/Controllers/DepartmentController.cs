using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRositoey;

        public DepartmentController(IDepartmentRepository departmentRositoey)
        {
            _departmentRositoey = departmentRositoey;
        }
        public IActionResult Index()
        {
            var departments=_departmentRositoey.GetAll();
            return View(departments);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _departmentRositoey.Add(department);
                return RedirectToAction(nameof(Index));
            }
            return View(department);
           
        }
        public IActionResult Details(int? id,string action="Details")
        {
            if(id is null)
            {
                return BadRequest(); //return 400
            }
            var department = _departmentRositoey.GetById(id.Value);
            if(department is null)
            {
                return NotFound();
            }
            return View(action , department);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //if (id is null)
            //{
            //    return BadRequest();
            //}
            //var department = _departmentRositoey.GetById(id.Value);
            //if(department is null)
            //{
            //    return NotFound();
            //}
            //return View(department);
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department department ,[FromRoute] int id)
        {
            if(id!=department.ID)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _departmentRositoey.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch(System.Exception ex) 
                {
                    //form
                    //string.Empty : the key is empty
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
               
            }
            return View(department);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
 
            return Details(id, "Delete");
        }
        [HttpPost]
        public IActionResult Delete(Department department , [FromRoute] int id)
        {
            if (id != department.ID)
            {
                return BadRequest();
            }
           try
                {
                    _departmentRositoey.Delete(department);
                    return RedirectToAction(nameof(Index));

                }
                catch (System.Exception ex)
                {
                    //form
                    //string.Empty : the key is empty
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(department);
                }

            
           
           
           
             

        }


    }
}
