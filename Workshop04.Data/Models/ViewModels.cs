using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Workshop04.Data.Attributes;

namespace Workshop04.Data.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(25)]
        public string CustFirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(25)]
        public string CustLastName { get; set; }

      

        [Required(ErrorMessage = "Address is required")]
        [StringLength(100)]
        public string CustAddress { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50)]
        public string CustCity { get; set; }

        [Required(ErrorMessage = "Province is required")]
        [StringLength(50)]
        public string CustProv { get; set; }


        [Required(ErrorMessage = "Postal code is required")]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$",
        ErrorMessage = "Postal code must be in Canadian format: A1A 1A1")]
        public string CustPostal { get; set; }

        [Required(ErrorMessage = "Home Phone is required")]
        [RegularExpression(
    @"^(\+1\s?)?(\(?\d{3}\)?)[\s.-]?\d{3}[\s.-]?\d{4}$",
    ErrorMessage = "Home Phone must be a valid Canadian phone number")]

        public string CustHomePhone { get; set; }


        [Required(ErrorMessage = "Business Phone is required")]
        [RegularExpression(@"^(\+1\s?)?(\(?\d{3}\)?)[\s.-]?\d{3}[\s.-]?\d{4}$",
            ErrorMessage = "Business Phone must be a valid Canadian phone number")]
        public string CustBusPhone { get; set; }

        [Required, EmailAddress]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
    ErrorMessage = "Please enter a valid email address.")]
        public string CustEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
   ErrorMessage = "Password must contain upper/lowercase letters, a number, and a special character.")]
        //[MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png" })]
        public IFormFile? ProfileImage { get; set; }
    }

    public class LoginViewModel
    {
        [Required] 
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
      
    }
}
