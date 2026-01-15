using Microsoft.AspNetCore.Mvc.Rendering;

namespace Workshop04.Constants;

/// <summary>
/// Contains all travel preference constants and options for the Travel Experts application
/// </summary>
public static class TravelPreferences
{
    /// <summary>
    /// Climate preference options
    /// </summary>
    public static class Climate
    {
        public const string Tropical = "Tropical";
        public const string Temperate = "Temperate";
        public const string Cold = "Cold";
        public const string Arid = "Arid";
        public const string Mediterranean = "Mediterranean";

        /// <summary>
        /// Gets all climate options as SelectListItems for dropdowns
        /// </summary>
        public static List<SelectListItem> GetOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = Tropical, Text = "Tropical" },
                new SelectListItem { Value = Temperate, Text = "Temperate" },
                new SelectListItem { Value = Cold, Text = "Cold/Arctic" },
                new SelectListItem { Value = Arid, Text = "Arid/Desert" },
                new SelectListItem { Value = Mediterranean, Text = "Mediterranean" }
            };
        }

        /// <summary>
        /// Gets all climate values
        /// </summary>
        public static List<string> GetValues()
        {
            return new List<string> { Tropical, Temperate, Cold, Arid, Mediterranean };
        }
    }

    /// <summary>
    /// Activity preference options
    /// </summary>
    public static class Activities
    {
        public const string Hiking = "Hiking";
        public const string Beach = "Beach";
        public const string Landscape = "Landscape";
        public const string Culture = "Culture";
        public const string Adventure = "Adventure";
        public const string Relaxation = "Relaxation";
        public const string Wildlife = "Wildlife";
        public const string Photography = "Photography";
        public const string Food = "Food";
        public const string Shopping = "Shopping";

        /// <summary>
        /// Gets all activity options as SelectListItems for checkboxes
        /// </summary>
        public static List<SelectListItem> GetOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = Hiking, Text = "Hiking & Trekking" },
                new SelectListItem { Value = Beach, Text = "Beach & Water Sports" },
                new SelectListItem { Value = Landscape, Text = "Scenic Landscapes" },
                new SelectListItem { Value = Culture, Text = "Cultural Experiences" },
                new SelectListItem { Value = Adventure, Text = "Adventure Sports" },
                new SelectListItem { Value = Relaxation, Text = "Relaxation & Spa" },
                new SelectListItem { Value = Wildlife, Text = "Wildlife & Nature" },
                new SelectListItem { Value = Photography, Text = "Photography" },
                new SelectListItem { Value = Food, Text = "Food & Culinary" },
                new SelectListItem { Value = Shopping, Text = "Shopping" }
            };
        }

        /// <summary>
        /// Gets all activity values
        /// </summary>
        public static List<string> GetValues()
        {
            return new List<string> 
            { 
                Hiking, Beach, Landscape, Culture, Adventure, 
                Relaxation, Wildlife, Photography, Food, Shopping 
            };
        }

        /// <summary>
        /// Validates if a given activity is valid
        /// </summary>
        public static bool IsValid(string activity)
        {
            return GetValues().Contains(activity);
        }
    }

    /// <summary>
    /// Travel companion preference options
    /// </summary>
    public static class Companions
    {
        public const string Solo = "Solo";
        public const string Couple = "Couple";
        public const string Family = "Family";
        public const string Group = "Group";

        /// <summary>
        /// Gets all companion options as SelectListItems for dropdowns
        /// </summary>
        public static List<SelectListItem> GetOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = Solo, Text = "Solo Travel" },
                new SelectListItem { Value = Couple, Text = "Couple/Partner" },
                new SelectListItem { Value = Family, Text = "Family (with kids)" },
                new SelectListItem { Value = Group, Text = "Group/Friends" }
            };
        }

        /// <summary>
        /// Gets all companion values
        /// </summary>
        public static List<string> GetValues()
        {
            return new List<string> { Solo, Couple, Family, Group };
        }
    }

    /// <summary>
    /// Location preference options
    /// </summary>
    public static class Locations
    {
        public const string CityCenter = "CityCenter";
        public const string Mountains = "Mountains";
        public const string Beach = "Beach";
        public const string Countryside = "Countryside";
        public const string Urban = "Urban";
        public const string Remote = "Remote";

        /// <summary>
        /// Gets all location options as SelectListItems for dropdowns
        /// </summary>
        public static List<SelectListItem> GetOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = CityCenter, Text = "City Center" },
                new SelectListItem { Value = Mountains, Text = "Mountains" },
                new SelectListItem { Value = Beach, Text = "Beach/Coastal" },
                new SelectListItem { Value = Countryside, Text = "Countryside/Rural" },
                new SelectListItem { Value = Urban, Text = "Urban/Metropolitan" },
                new SelectListItem { Value = Remote, Text = "Remote/Off-grid" }
            };
        }

        /// <summary>
        /// Gets all location values
        /// </summary>
        public static List<string> GetValues()
        {
            return new List<string> { CityCenter, Mountains, Beach, Countryside, Urban, Remote };
        }
    }

    /// <summary>
    /// Preference keys used in the CustomerPreferences table
    /// </summary>
    public static class Keys
    {
        public const string PreferredClimate = "PreferredClimate";
        public const string PreferredActivities = "PreferredActivities";
        public const string TravelCompanion = "TravelCompanion";
        public const string PreferredLocation = "PreferredLocation";

        /// <summary>
        /// Gets all preference keys
        /// </summary>
        public static List<string> GetAll()
        {
            return new List<string>
            {
                PreferredClimate,
                PreferredActivities,
                TravelCompanion,
                PreferredLocation
            };
        }
    }

    /// <summary>
    /// Utility methods for preference management
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Converts a comma-separated string to a list
        /// </summary>
        public static List<string> ParseActivities(string? activities)
        {
            if (string.IsNullOrWhiteSpace(activities))
                return new List<string>();

            return activities.Split(',', StringSplitOptions.RemoveEmptyEntries)
                             .Select(a => a.Trim())
                             .Where(a => Activities.IsValid(a))
                             .ToList();
        }

        /// <summary>
        /// Converts a list of activities to a comma-separated string
        /// </summary>
        public static string FormatActivities(List<string>? activities)
        {
            if (activities == null || !activities.Any())
                return string.Empty;

            return string.Join(",", activities.Where(a => Activities.IsValid(a)));
        }

        /// <summary>
        /// Validates a preference value against its type
        /// </summary>
        public static bool IsValidPreference(string key, string value)
        {
            return key switch
            {
                Keys.PreferredClimate => Climate.GetValues().Contains(value),
                Keys.TravelCompanion => Companions.GetValues().Contains(value),
                Keys.PreferredLocation => Locations.GetValues().Contains(value),
                Keys.PreferredActivities => ParseActivities(value).Any(),
                _ => false
            };
        }
    }
}
