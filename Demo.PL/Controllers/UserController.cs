using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager , IMapper mapper)
        {
			_userManager = userManager;
            _mapper = mapper;
        }
        public async Task< IActionResult >Index(string SearchValue)
		{
			if (string.IsNullOrEmpty(SearchValue))
			{
				//Mapping from list of AppUser to UserViewModel
				var Users = await  _userManager.Users.Select(
					U => new UserViewModel
					{
						Id = U.Id,
						FName = U.FName,
						LName = U.LName,
						Email = U.Email,
						phoneNumber = U.PhoneNumber,
						Roles = _userManager.GetRolesAsync(U).Result
					}).ToListAsync();
				return View(Users); // Users is alist of Users
			}
			else
			{
				//Mapping from list of AppUser to UserViewModel
				var User= await _userManager.FindByEmailAsync(SearchValue);
				var MappedUser = new UserViewModel()
				{
					Id = User.Id,
					FName = User.FName,
					LName = User.LName,
					Email = User.Email,
					phoneNumber = User.PhoneNumber,
					Roles = _userManager.GetRolesAsync(User).Result
				};
				return View(new List<UserViewModel> { MappedUser }); //So we need to convert the opject MappedUser to list 
			}
			

		}

		public async Task< ActionResult> Details(string id, string action = "Details")
		{
            if (id == null)
            {
                return BadRequest();
            }
            var User = await _userManager.FindByIdAsync(id);
			if (User == null)
			{
				return NotFound();
			}
            var MappedUser = _mapper.Map<AppUser,UserViewModel>(User);
            return View(action, MappedUser);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel userVM, [FromRoute] string id)
        {
            if (userVM.Id != id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    //Manual mapping (we dont use mapper)
                    var User = await _userManager.FindByIdAsync(id);
					User.FName = userVM.FName;
					User.LName = userVM.LName;
					User.PhoneNumber = userVM.phoneNumber;

                    await _userManager.UpdateAsync(User);
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(userVM);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserViewModel UserVM, [FromRoute] string id)
        {
            if (UserVM.Id == id)
            {
                try
                {
                    var User =await _userManager.FindByIdAsync(id);
                    await _userManager.DeleteAsync(User);
                    return RedirectToAction(nameof(Index));

                }
                catch (System.Exception ex)
                {
                    //form
                    //string.Empty : the key is empty
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(UserVM);
                }
            }
            return BadRequest();
        }


    }
}
