using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    //يعني محدش ينفع يدخل هنا غير و هو عامل لوجين 
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentRositoey;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            //_departmentRositoey = departmentRositoey;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var departments= await _unitOfWork.DepartmentRepository.GetAllAsync();
           await _unitOfWork.CompleteAsync();
            return View(departments);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(department);
           
        }
        public async Task<IActionResult> Details(int? id,string action="Details")
        {
            if(id is null)
            {
                return BadRequest(); //return 400
            }
            var department =await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            if(department is null)
            {
                return NotFound();
            }
            return View(action , department);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
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
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Department department ,[FromRoute] int id)
        {
            if(id!=department.ID)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.DepartmentRepository.Update(department);
                    await _unitOfWork.CompleteAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
 
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Department department , [FromRoute] int id)
        {
            if (id != department.ID)
            {
                return BadRequest();
            }
            else
            {
                _unitOfWork.DepartmentRepository.Delete(department);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            //try
            //     {
            //     _unitOfWork.DepartmentRepository.Delete(department);
            //     await _unitOfWork.CompleteAsync();
            //     return RedirectToAction(nameof(Index));

            //     }
            //     catch (System.Exception ex)
            //     {
            //         //form
            //         //string.Empty : the key is empty
            //         ModelState.AddModelError(string.Empty, ex.Message);
            //         return View(department);
            //     }







        }


    }
}
