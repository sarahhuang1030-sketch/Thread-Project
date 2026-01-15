// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Workshop04.Services;

namespace Workshop04.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly IImageUploadService _imageUploadService;
        private readonly ILogger<PersonalDataModel> _logger;

        public PersonalDataModel(
            UserManager<Customer> userManager,
            SignInManager<Customer> signInManager,
            IImageUploadService imageUploadService,
            ILogger<PersonalDataModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageUploadService = imageUploadService;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Password is required to delete your account")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAccountAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Verify the password
            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, lockoutOnFailure: false);
            if (!passwordCheck.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Incorrect password. Please try again.");
                return Page();
            }

            try
            {
                // Delete profile picture if exists
                if (!string.IsNullOrEmpty(user.ProfileImagePath))
                {
                    await _imageUploadService.DeleteProfilePictureAsync(user.ProfileImagePath);
                }

                // Delete the user account
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Unexpected error occurred deleting user.");
                }

                // Sign out the user
                await _signInManager.SignOutAsync();

                _logger.LogInformation("User with ID '{UserId}' deleted their account.", user.Id);

                // Redirect to home page with a success message
                TempData["AccountDeleted"] = "Your account has been successfully deleted.";
                return RedirectToPage("/Index", new { area = "" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user account.");
                StatusMessage = "Error: An unexpected error occurred while deleting your account. Please try again or contact support.";
                return Page();
            }
        }
    }
}
