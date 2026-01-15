// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Workshop04.Services;
using Workshop04.Data.Data;
using Workshop04.Data.Models;

namespace Workshop04.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly IImageUploadService _imageUploadService;
        private readonly TravelExpertsContext _context;

        public IndexModel(
            UserManager<Customer> userManager,
            SignInManager<Customer> signInManager,
            IImageUploadService imageUploadService,
            TravelExpertsContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageUploadService = imageUploadService;
            _context = context;
        }

        public string Username { get; set; }
        public string CurrentProfilePicture { get; set; }
        public decimal CreditBalance { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public bool RemoveProfilePicture { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(25)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(25)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [StringLength(75)]
            [Display(Name = "Address")]
            public string Address { get; set; }

            [Required]
            [StringLength(50)]
            [Display(Name = "City")]
            public string City { get; set; }

            [Required]
            [StringLength(2)]
            [Display(Name = "Province")]
            public string Province { get; set; }

            [Required]
            [StringLength(7)]
            [RegularExpression(@"^[A-Za-z]\d[A-Za-z]\s?\d[A-Za-z]\d$",
                ErrorMessage = "Postal code must be in Canadian format: A1A 1A1")]
            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }

            [StringLength(25)]
            [Display(Name = "Country")]
            public string Country { get; set; }

            [Required]
            [Phone]
            [Display(Name = "Home Phone")]
            [RegularExpression(@"^\d{3}-?\d{3}-?\d{4}$",
                ErrorMessage = "Phone must be 10 digits in format: XXX-XXX-XXXX")]
            public string HomePhone { get; set; }

            [Phone]
            [Display(Name = "Business Phone")]
            [RegularExpression(@"^\d{3}-?\d{3}-?\d{4}$",
                ErrorMessage = "Phone must be 10 digits in format: XXX-XXX-XXXX")]
            public string BusinessPhone { get; set; }

            [Display(Name = "Profile Picture")]
            public IFormFile ProfilePicture { get; set; }
        }

        private async Task LoadAsync(Customer user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;
            CurrentProfilePicture = user.ProfileImagePath;
            CreditBalance = user.CreditBalance;

            Input = new InputModel
            {
                FirstName = user.CustFirstName,
                LastName = user.CustLastName,
                Address = user.CustAddress,
                City = user.CustCity,
                Province = user.CustProv,
                PostalCode = FormatPostalCode(user.CustPostal),
                Country = user.CustCountry,
                HomePhone = FormatPhoneNumber(user.CustHomePhone),
                BusinessPhone = FormatPhoneNumber(user.CustBusPhone)
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            // Update customer information
            user.CustFirstName = Input.FirstName;
            user.CustLastName = Input.LastName;
            user.CustAddress = Input.Address;
            user.CustCity = Input.City;
            user.CustProv = Input.Province;
            user.CustPostal = StripPostalCodeFormatting(Input.PostalCode);
            user.CustCountry = Input.Country;
            user.CustHomePhone = StripPhoneFormatting(Input.HomePhone);
            user.CustBusPhone = StripPhoneFormatting(Input.BusinessPhone);

            await _userManager.UpdateAsync(user);

            // Handle profile picture removal
            if (RemoveProfilePicture && !string.IsNullOrEmpty(user.ProfileImagePath))
            {
                await _imageUploadService.DeleteProfilePictureAsync(user.ProfileImagePath);
                user.ProfileImagePath = null;
                await _userManager.UpdateAsync(user);
            }

            // Handle profile picture upload
            if (Input.ProfilePicture != null)
            {
                // Validate image before processing
                if (!_imageUploadService.IsValidImage(Input.ProfilePicture))
                {
                    StatusMessage = "Error: Invalid image file. Please upload a JPG, PNG, or GIF file under 5MB.";
                    await LoadAsync(user);
                    return Page();
                }

                // Delete old picture if exists
                if (!string.IsNullOrEmpty(user.ProfileImagePath))
                {
                    await _imageUploadService.DeleteProfilePictureAsync(user.ProfileImagePath);
                }

                // Save new picture
                var imagePath = await _imageUploadService.SaveProfilePictureAsync(Input.ProfilePicture, user.Id);
                if (imagePath != null)
                {
                    user.ProfileImagePath = imagePath;
                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    StatusMessage = "Error uploading profile picture. Please try again.";
                    await LoadAsync(user);
                    return Page();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated successfully.";
            return RedirectToPage();
        }

        // Helper method to strip phone formatting
        private string StripPhoneFormatting(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return null;

            return new string(phone.Where(char.IsDigit).ToArray());
        }

        // Helper method to strip postal code formatting
        private string StripPostalCodeFormatting(string postal)
        {
            if (string.IsNullOrEmpty(postal))
                return null;

            return postal.Replace(" ", "").ToUpperInvariant();
        }

        // Helper method to format phone number for display
        private string FormatPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return null;

            // Remove any non-digit characters
            var digits = new string(phone.Where(char.IsDigit).ToArray());
            
            // Format as XXX-XXX-XXXX if we have 10 digits
            if (digits.Length == 10)
            {
                return $"{digits.Substring(0, 3)}-{digits.Substring(3, 3)}-{digits.Substring(6, 4)}";
            }

            return phone; // Return as-is if not 10 digits
        }

        // Helper method to format postal code for display
        private string FormatPostalCode(string postal)
        {
            if (string.IsNullOrEmpty(postal))
                return null;

            // Remove spaces and convert to uppercase
            var cleaned = postal.Replace(" ", "").ToUpperInvariant();

            // Format as A1A 1A1 if we have 6 characters
            if (cleaned.Length == 6)
            {
                return $"{cleaned.Substring(0, 3)} {cleaned.Substring(3, 3)}";
            }

            return postal; // Return as-is if not proper format
        }
    }
}