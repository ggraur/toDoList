using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using toDoList.Models;
using toDoList.ViewModels;

namespace toDoList.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;
        private readonly IForgotPassword  passwordRepo;

        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                ILogger<AccountController> logger, 
                                IForgotPassword passwordRepo)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.passwordRepo = passwordRepo;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);

                    logger.Log(LogLevel.Warning, confirmationLink);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        if (signInManager.IsSignedIn(User) && User.IsInRole("Administrator"))
                        {
                            return RedirectToAction("ListUsers", "Administration");
                        }
                        else
                        {
                            ViewBag.Signal = "ok";
                            ViewBag.ErrorTitle = "Registration successful";
                            ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                                "email, by cliking on the confirmation link we have emailed you!.";
                            return View("~/Views/Error/GeneralError.cshtml");
                        }
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()

            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            model.ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.EmailConfirmed &&
                    (await userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Account not activate, Please check you email and activate your account!");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password,
                                        model.RememberMe, true);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "User");
                        //return RedirectToAction("index", "home");
                    }

                }

                if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }

        //http://localhost:54324/Account/ConfirmEmail?userId=12a527c5-e995-4a86-b1c9-520c54450412&token=CfDJ8AN7DtCXzUpKnfnw8kxZyixCb2gm0AvJSUCESSqZkBqxQ5tzOU63Gj1hbpLYHPFMwLkRa9P%2Bbsbmjr7yvvkZtqFiPRpGlvg1wA3wYOard4qf0iAMaMjJW7ws%2FWmhlXvncHgSiVXw4iCOXgaFv16mIr%2BkH365dt%2FTm6h0AxUDKI7PvNiUjABciD1xyDK2u60MZSxyTW6krM29tekyMv8AsxTfGLPNsmCn3EuxaQx%2FMUiQxXTlXuzQNtbx93HzA4IrPQ%3D%3D
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user ID {userId} is invalid";
                return View("NotFound");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                @ViewBag.Signal = "ok";
                @ViewBag.ErrorTitle = "Email confirmation successful!";
                @ViewBag.ErrorMessage = "Thank you for confirming your email.";
                return View("~/Views/Error/GeneralError.cshtml");
            }
            else
            {
                @ViewBag.Signal = "notok";
                @ViewBag.ErrorTitle = "Email confirmation was not successful!";
                @ViewBag.ErrorMessage = "Sorry for the inconvenience, please contact our support team to report this issue.";
                return View("~/Views/Error/GeneralError.cshtml");
            }

        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    ViewBag.Signal = "ok";
                    ViewBag.ErrorTitle = "Resset password link!";
                    ViewBag.ErrorMessage = "Password reset link successfully sent  to your email."  +
                                           "Please check your email and click on the link on your email to be redirected to reset password form.";

                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);
                    logger.Log(LogLevel.Warning, passwordResetLink);

                    model.Token = token;
                    model.ResetLinkCreatedTime = DateTime.Now;
                    model.ResetLinkValidity = model.ResetLinkCreatedTime.AddHours(24);

                    ForgotPasswordViewModel resInsertLink =  passwordRepo.InsertResetLink(model);

                    if (resInsertLink != null)
                    {
                        return View("~/Views/Error/GeneralError.cshtml"); 
                    }
                    logger.Log(LogLevel.Warning, "Reset Password link insertion into DB failed");
                }
                ViewBag.Signal = "notok";
                ViewBag.ErrorTitle = "User not found!";
                ViewBag.ErrorMessage = "Please provide corect email address!";
                return View("~/Views/Error/GeneralError.cshtml");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            if (email == null || token == null)
            {
                return RedirectToAction("index", "home");
            }
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The user with the email {email} was not found!";
                return View("NotFound");
            }

            bool resetLinkIsValid = passwordRepo.ResetPasswordLinkIsValid(email, token);

            if (resetLinkIsValid == true)
            {
                return View();
            }

            ViewBag.Signal = "notok";
            ViewBag.ErrorTitle   = "Reset password link is not valid or is expired!";
            ViewBag.ErrorMessage = "Please reset you password again to receive a new reset password link!";
            return View("~/Views/Error/GeneralError.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null )
                {
                    ViewBag.Signal = "ok";
                    ViewBag.ErrorTitle = "Password sucessfully reseted!";
                    ViewBag.ErrorMessage = "Your password have ben reseted, now you can log in.";

                    var result = await userManager.ResetPasswordAsync(user,model.Token,model.Password);
                    if (result.Succeeded)
                    {
                        bool  updateResetLinkDate = passwordRepo.ConfirmResetLink(model.Email,model.Token);
                        //ConfirmResetLink
                        if (updateResetLinkDate)
                        {
                            return View("~/Views/Error/GeneralError.cshtml");
                        }
                        ViewBag.Signal = "notok";
                        ViewBag.ErrorTitle = "Error to update /reset password!";
                        ViewBag.ErrorMessage = "Please contact our suport team if the error persist!";
                       
                        logger.Log(LogLevel.Warning, "Error to update [ResetLinkConfirmationDate] !");
                        
                        return View("~/Views/Error/GeneralError.cshtml");
                    }
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                    return View(model);
                    
                }
                ViewBag.Signal = "notok";
                ViewBag.ErrorTitle = "User not found!";
                ViewBag.ErrorMessage = "Please provide correct user model!";
                return View("~/Views/Error/GeneralError.cshtml");
            }
            return View(model);
        }

    }
}
