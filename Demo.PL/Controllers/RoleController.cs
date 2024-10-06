using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager ,IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            if ( string.IsNullOrEmpty(SearchValue))
            {
                var Role=await  _roleManager.Roles.ToListAsync();
                var MappedRole = _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleViewModel>>(Role);

                return View(MappedRole);
            }
            else
            {
                var Role= await _roleManager.FindByNameAsync(SearchValue);
                if(Role is not null)
                {
                    var MappedRole = _mapper.Map<IdentityRole, RoleViewModel>(Role);
                    return View(new List<RoleViewModel>() { MappedRole });
                }
                else
                {
                    var role = await _roleManager.Roles.ToListAsync();
                    var MappedRole = _mapper.Map<IEnumerable<IdentityRole>, IEnumerable<RoleViewModel>>(role);

                    return View(MappedRole);
                }

              
            }
                
            
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {

            if (ModelState.IsValid)
            {
                var MappedRole = _mapper.Map<RoleViewModel, IdentityRole>(roleViewModel);
                await _roleManager.CreateAsync(MappedRole);
                return RedirectToAction("Index");
            }
            return View(roleViewModel);
        }

        public async Task<ActionResult> Details(string id, string action = "Details")
        {
            if (id == null)
            {
                return BadRequest();
            }
            var Role = await _roleManager.FindByIdAsync(id);
            if (Role == null)
            {
                return NotFound();
            }
            var MappedRole = _mapper.Map<IdentityRole, RoleViewModel>(Role);
            return View(action, MappedRole);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel roleVM, [FromRoute] string id)
        {
            if (roleVM.Id != id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {


                    var Role = await _roleManager.FindByIdAsync(roleVM.Id);
                    Role.Name = roleVM.RoleName;
                    await _roleManager.UpdateAsync(Role);
                    //var MappedRole = _mapper.Map<RoleViewModel, IdentityRole>(roleVM);
                    //await _roleManager.UpdateAsync(MappedRole);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(roleVM);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RoleViewModel roleVM, [FromRoute] string id)
        {
            if (roleVM.Id == id)
            {
                try
                {
                    var Role = await _roleManager.FindByIdAsync(id);
                    await _roleManager.DeleteAsync(Role);
                    return RedirectToAction(nameof(Index));

                }
                catch (System.Exception ex)
                {
                    //form
                    //string.Empty : the key is empty
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(roleVM);
                }
            }
            return BadRequest();
        }

    }
}
