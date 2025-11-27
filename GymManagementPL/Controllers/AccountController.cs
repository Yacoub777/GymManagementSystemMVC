using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService, SignInManager<ApplicationUser> signInManager)
        {
            this._accountService = accountService;
            this._signInManager = signInManager;
        }

        #region Login

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("InvalidLogin", "Missing Data Or Check Input Data");
                return View(loginViewModel);
            }

            var User = _accountService.ValidateUser(loginViewModel);
            if (User is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(loginViewModel);
            }

            var Result = _signInManager.PasswordSignInAsync(
                                                                User,
                                                                loginViewModel.Password,
                                                                loginViewModel.RememberMe,
                                                                lockoutOnFailure: false
                                                            ).Result;
            if (Result.IsNotAllowed)
            {
                ModelState.AddModelError("InvalidLogin", "Your Account Is Not Allowed");
                return View(loginViewModel);
            }

            if (Result.IsLockedOut)
            {
                ModelState.AddModelError("InvalidLogin", "Your Account Is Locked Out");
                return View(loginViewModel);
            }

            if (Result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("InvalidLogin", "Login Failed");
            return View(loginViewModel);


        }

        #endregion


        #region Sign Out

        [HttpPost]
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));

        }

        #endregion

        public ActionResult AccessDenied()
        {
            return View();
        }

    }
}
