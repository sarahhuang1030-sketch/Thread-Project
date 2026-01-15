using System.ComponentModel.DataAnnotations;
using Workshop04.Data.Models;

namespace Workshop04.ViewModels
{
    public class PurchasePackageViewModel
    {
        public Package? Package { get; set; }
        public Customer? Customer { get; set; }

        public int PackageId { get; set; }
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please select the number of travelers")]
        [Range(1, 20, ErrorMessage = "Number of travelers must be between 1 and 20")]
        [Display(Name = "Number of Travelers")]
        public int NumberOfTravelers { get; set; } = 1;

        public decimal TotalAmount { get; set; }
        public decimal AvailableCredit { get; set; }

        [Required(ErrorMessage = "You must confirm your purchase")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must confirm your purchase")]
        [Display(Name = "Confirm Purchase")]
        public bool ConfirmPurchase { get; set; }
    }
}