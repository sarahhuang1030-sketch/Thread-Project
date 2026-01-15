using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Identity;

namespace Workshop04.Data.Models;

[Index("AgentId", Name = "EmployeesCustomers")]
public partial class Customer : IdentityUser
{

    [Key]
    public int CustomerId { get; set; }

    [Required]
    [StringLength(25)]
    public string CustFirstName { get; set; }

    [Required]
    [StringLength(25)]
    public string CustLastName { get; set; }

    [Required]
    [StringLength(75)]
    public string CustAddress { get; set; }

    [Required]
    [StringLength(50)]
    public string CustCity { get; set; }

    [Required]
    [StringLength(2)]
    public string CustProv { get; set; }

    [Required]
    [StringLength(7)]
    [RegularExpression(@"^[A-Za-z]\d[A-Za-z]\s?\d[A-Za-z]\d$",
        ErrorMessage = "Postal code must be in Canadian format: A1A 1A1")]
    [Display(Name = "Postal Code")]
    public string CustPostal { get; set; }

    [StringLength(25)]
    public string? CustCountry { get; set; }

    [StringLength(20)]
    [RegularExpression(@"^\d{3}-?\d{3}-?\d{4}$",
        ErrorMessage = "Phone must be 10 digits")]
    [Display(Name = "Home Phone")]
    public string CustHomePhone { get; set; }

    [StringLength(20)]
    [RegularExpression(@"^\d{3}-?\d{3}-?\d{4}$",
        ErrorMessage = "Phone must be 10 digits")]
    [Display(Name = "Business Phone")]
    public string? CustBusPhone { get; set; }

    [Required, EmailAddress]
    //[StringLength(50)]
    public string CustEmail { get; set; }

    // [Required]
    // [StringLength(50)]
    // public string CustUsername { get; set; }

  

    //public string? UserName { get; set; }

    // NOTE: In production, hash & salt the password. For educational purposes, we store hashed string.
    //public string? PasswordHash { get; set; }

    public string? ProfileImagePath { get; set; }

    //public Preference Preference { get; set; } = new Preference();

    //// 🔹 Navigation property to Purchases
    //public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    //public ICollection<Bookings> Bookings { get; set; }

    public int? AgentId { get; set; }

    [ForeignKey("AgentId")]
    [InverseProperty("Customers")]
    public virtual Agent Agent { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Booking> Bookings { get; set; } = [];

    [InverseProperty("Customer")]
    public virtual ICollection<CustomerPreferences> CustomerPreferences { get; set; } = [];

    // CreditBalance property 
    [Column(TypeName = "decimal(10,2)")]
    public decimal CreditBalance { get; set; } = 0;
}