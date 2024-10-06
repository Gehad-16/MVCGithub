using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	//يعني محدش ينفع يدخل هنا غير و هو عامل لوجين 
	[Authorize]
	public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string searchvalue)
        {
            if(string.IsNullOrEmpty(searchvalue))
            {    
                var employees =await  _unitOfWork.EmployeeRepository.GetAllAsync();
                var MappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
                return View(MappedEmployee);
            }
            else 
            {
                var employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(searchvalue) ;
                var MappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
                return View(MappedEmployee);
                
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //Console.WriteLine("ss");
            ViewBag.DepartmentsViewBag =await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {

            if (ModelState.IsValid)
            {
                employeeVM.EmployeeImage= DocumentSettings.UploadFile(employeeVM.Image, "Images");
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                
               await _unitOfWork.EmployeeRepository.AddAsync(MappedEmployee);
               await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            return View(employeeVM);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id, string action = "Details")
        {
            if (id == null)
            {
                return BadRequest();
            }
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }
            var MapedEmployee = _mapper.Map<Employee , EmployeeViewModel>(employee);
            return View(action, MapedEmployee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.DepartmentsViewBag =await _unitOfWork.DepartmentRepository.GetAllAsync();
            return await Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVM, [FromRoute] int? id)
        {
            if (employeeVM.Id != id)
            {

                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeVM.Image is not null)
                    {
                        employeeVM.EmployeeImage = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                    }
                    
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                   await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(employeeVM);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public IActionResult Delete(EmployeeViewModel employeeVM, [FromRoute] int? id)
        {
            if (employeeVM.Id == id)
            {
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
                    int x = _unitOfWork.CompleteAsync().Result;
                    if (x > 0 && employeeVM.EmployeeImage is not null)
                    {
                        DocumentSettings.DeleteFile("Images", employeeVM.EmployeeImage);
                    }
                    
                    return RedirectToAction(nameof(Index));

                }
                catch (System.Exception ex)
                {
                    //form
                    //string.Empty : the key is empty
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(employeeVM);
                }
            }
            return BadRequest();
        }
    }
}