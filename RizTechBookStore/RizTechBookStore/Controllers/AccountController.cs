using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RizTechBookStore.Models;
using RizTechBookStore.Repository;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace RizTechBookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [Route("signup")]
        public IActionResult Signup()
        {
            return View();
        }

        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> Signup(SignUpUserModel signUpUserModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(signUpUserModel);
                if (!result.Succeeded) 
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(signUpUserModel);
                }
                ModelState.Clear();
                return RedirectToAction("ConfirmEmail", new {email = signUpUserModel.Email});
                // Process the signup logic here
            }
            return View(signUpUserModel);
        }
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(SignInModel signInModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordSignInAsync(signInModel);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsNotAllowed)
                {

                    ModelState.AddModelError("", "Not allowed to login");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View(signInModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _accountRepository.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            //await _accountRepository.SignOutAsync();
            return View();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (ModelState.IsValid)
            {
                var result =await _accountRepository.ChangePasswordAsync(changePasswordModel);
                if (result.Succeeded)
                {
                    ViewBag.IsSucess = true;
                    ModelState.Clear();
                    return View();
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(changePasswordModel);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
        {
            

            EmailConfirmModel confirmModel = new EmailConfirmModel()
            {
                Email = email,
                EmailVerified = false,
                EmailSent = false,
                IsConfirmed = false
            };

            // If uid or token is missing, this was likely a redirect after signup.
            // Show that a confirmation email was sent (if the user exists) rather than an error.
            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(token))
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var user = await _accountRepository.GetUserByEmailAsync(email);
                    if (user != null)
                    {
                        confirmModel.IsConfirmed = user.EmailConfirmed;
                        // If the user exists and is not confirmed, assume an email was sent during registration
                        confirmModel.EmailSent = !user.EmailConfirmed;
                    }
                }

                return View(confirmModel);
            }

            // Mark that verification was attempted (uid/token provided)
            confirmModel.VerificationAttempted = true;

            // Try multiple token variants to handle encoding differences from email clients
            var attempts = new List<string>
            {
                token,
                System.Net.WebUtility.UrlDecode(token),
                token.Replace(' ', '+'),
                System.Net.WebUtility.UrlDecode(token.Replace(' ', '+'))
            };

            foreach (var t in attempts)
            {
                if (string.IsNullOrEmpty(t)) continue;
                var result = await _accountRepository.ConfirmEmailAsync(uid, t);
                if (result.Succeeded)
                {
                    confirmModel.EmailVerified = true;
                    break;
                }
            }

            // If verification failed, check whether the user is already confirmed or whether an email was sent
            if (!confirmModel.EmailVerified && !string.IsNullOrEmpty(email))
            {
                var user = await _accountRepository.GetUserByEmailAsync(email);
                if (user != null)
                {
                    confirmModel.IsConfirmed = user.EmailConfirmed;
                    confirmModel.EmailSent = !user.EmailConfirmed;
                }
            }

            return View(confirmModel);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel emailConfirmModel)
        {
            var user = await _accountRepository.GetUserByEmailAsync(emailConfirmModel.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    emailConfirmModel.IsConfirmed = true;
                    return View(emailConfirmModel);
                }
                await _accountRepository.GenerateEmailConfirmationTokenAsync(user);
                emailConfirmModel.EmailSent = true;
                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Somthing went wrong.");
            }
            return View(emailConfirmModel);
        }

        [AllowAnonymous, HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous, HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
        {
            if (ModelState.IsValid)
            {
                var user =await _accountRepository.GetUserByEmailAsync(forgotPasswordModel.Email);
                if (user != null)
                {
                    await _accountRepository.GenerateForgotPasswordTokenAsync(user);
                }

                ModelState.Clear();
                forgotPasswordModel.EmailSent = true;
            }
            return View(forgotPasswordModel);
        }

        [AllowAnonymous, HttpGet("reset-password")]
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPasswordModel model = new ResetPasswordModel()
            { 
                Token = token,
                UserId = uid
            };

            // Return the model so the view has the token and user id prefilled
            return View(model);
        }

        [AllowAnonymous, HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (ModelState.IsValid)
            {
                resetPasswordModel.Token.Replace(' ', '+');
                var result = await _accountRepository.ResetPasswordAsync(resetPasswordModel);
                if (result.Succeeded)
                {
                    ModelState.Clear();
                    resetPasswordModel.IsSuccess = true;
                    return View(resetPasswordModel);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(resetPasswordModel);
        }
    }
}
