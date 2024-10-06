using Demo.DAL.Models;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    //Default
    [AllowAnonymous]
    public class AccountController : Controller
    {
        
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}
        #region Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)//server side validation
            {
                var User = new AppUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    FName = model.FName,
                    LName = model.LName,
                    IsAgree = model.IsAgree,
                };

                var result = await _userManager.CreateAsync(User, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View(model);
            }
            return View(model);
        }

        #endregion


        #region Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var Flag = await _userManager.CheckPasswordAsync(User, model.Password);
                    if (Flag)
                    {
                        //false علشان ميعملش اكونت لو الباسورد غلط
                        //الهدف منها انها بتشوف لو الاكونت ده كان معمولة بلوك مثلا او لو انا عايزه احتفظ بالباسورد
                        // PasswordSignInAsync   هدفها انها بتولد ليك توكين التوكين ده زي رقم البطاقه بتاعك كده ولكن داخل الويبسايت علشان مثلا لو عملت كومنت علي حاجه معينه يعرف ان ده انا 
                        // في كل مره بعمل فيها لوجين بيعمل توليد لتوكين ليا بيفضل لازقه في كل ريكويست انا ببعته علشان يتاكد اذا كان ليا الحق اني اطلب الطلب ده ولا لا 
                        // Token ==> is an encripted string 
                        var result = await _signInManager.PasswordSignInAsync(User, model.Password, model.RemeberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Incorrect Password");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, " Incorrect Email");
                }

            }
            return View(model);

        }
        #endregion

        #region SignOut

        public new async Task< IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion
    }
}
