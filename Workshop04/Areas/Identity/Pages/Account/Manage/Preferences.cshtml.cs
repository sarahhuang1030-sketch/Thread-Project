using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Workshop04.Data.Models;
using Workshop04.Data.Data;
using Workshop04.Constants;
using System.ComponentModel.DataAnnotations;

namespace Workshop04.Areas.Identity.Pages.Account.Manage
{
    public class PreferencesModel : PageModel
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly TravelExpertsContext _context;

        public PreferencesModel(
            UserManager<Customer> userManager,
            SignInManager<Customer> signInManager,
            TravelExpertsContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public List<SelectListItem> ClimateOptions { get; set; }
        public List<SelectListItem> ActivityOptions { get; set; }
        public List<SelectListItem> CompanionOptions { get; set; }
        public List<SelectListItem> LocationOptions { get; set; }

        public class InputModel
        {
            [Display(Name = "Preferred Climate")]
            public string PreferredClimate { get; set; }

            [Display(Name = "Preferred Activities")]
            public List<string> PreferredActivities { get; set; }

            [Display(Name = "Travel Companion")]
            public string TravelCompanion { get; set; }

            [Display(Name = "Preferred Location")]
            public string PreferredLocation { get; set; }
        }

        private void InitializePreferenceOptions()
        {
            ClimateOptions = TravelPreferences.Climate.GetOptions();
            ActivityOptions = TravelPreferences.Activities.GetOptions();
            CompanionOptions = TravelPreferences.Companions.GetOptions();
            LocationOptions = TravelPreferences.Locations.GetOptions();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            InitializePreferenceOptions();
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
                InitializePreferenceOptions();
                await LoadAsync(user);
                return Page();
            }

            // Save preferences using your existing method pattern
            await SavePreferenceAsync(user.Id, TravelPreferences.Keys.PreferredClimate, Input.PreferredClimate);
            await SavePreferenceAsync(user.Id, TravelPreferences.Keys.PreferredActivities,
                TravelPreferences.Utilities.FormatActivities(Input.PreferredActivities));
            await SavePreferenceAsync(user.Id, TravelPreferences.Keys.TravelCompanion, Input.TravelCompanion);
            await SavePreferenceAsync(user.Id, TravelPreferences.Keys.PreferredLocation, Input.PreferredLocation);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your travel preferences have been updated successfully.";
            return RedirectToPage();
        }

        private async Task LoadAsync(Customer user)
        {
            // Load preferences from CustomerPreferences table
            var preferences = await _context.CustomerPreferences
                .Where(p => p.CustomerId == user.Id)
                .ToListAsync();

            var climate = preferences.FirstOrDefault(p => p.PreferenceKey == TravelPreferences.Keys.PreferredClimate)?.PreferenceValue;
            var activities = preferences.FirstOrDefault(p => p.PreferenceKey == TravelPreferences.Keys.PreferredActivities)?.PreferenceValue;
            var companion = preferences.FirstOrDefault(p => p.PreferenceKey == TravelPreferences.Keys.TravelCompanion)?.PreferenceValue;
            var location = preferences.FirstOrDefault(p => p.PreferenceKey == TravelPreferences.Keys.PreferredLocation)?.PreferenceValue;

            Input = new InputModel
            {
                PreferredClimate = climate,
                PreferredActivities = TravelPreferences.Utilities.ParseActivities(activities),
                TravelCompanion = companion,
                PreferredLocation = location
            };
        }

        private async Task SavePreferenceAsync(string customerId, string key, string value)
        {
            var preference = await _context.CustomerPreferences
                .FirstOrDefaultAsync(p => p.CustomerId == customerId && p.PreferenceKey == key);

            if (string.IsNullOrEmpty(value))
            {
                // Remove preference if value is empty
                if (preference != null)
                {
                    _context.CustomerPreferences.Remove(preference);
                }
            }
            else
            {
                // Validate preference value before saving
                if (!TravelPreferences.Utilities.IsValidPreference(key, value))
                {
                    return; // Skip invalid preferences
                }

                if (preference == null)
                {
                    // Create new preference
                    preference = new CustomerPreferences
                    {
                        CustomerId = customerId,
                        PreferenceKey = key,
                        PreferenceValue = value,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.CustomerPreferences.Add(preference);
                }
                else
                {
                    // Update existing preference
                    preference.PreferenceValue = value;
                    preference.UpdatedAt = DateTime.UtcNow;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}