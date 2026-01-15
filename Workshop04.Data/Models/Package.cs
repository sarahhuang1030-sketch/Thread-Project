using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workshop04.Data.Models;

public partial class Package
{
    [Key]
    public int PackageId { get; set; }

    [Required]
    [StringLength(50)]
    public string PkgName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PkgStartDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? PkgEndDate { get; set; }

    [StringLength(50)]
    public string PkgDesc { get; set; }

    [Column(TypeName = "money")]
    public decimal PkgBasePrice { get; set; }

    [Column(TypeName = "money")]
    public decimal? PkgAgencyCommission { get; set; }

    [InverseProperty("Package")]
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    [InverseProperty("Package")]
    public virtual ICollection<PackagesProductsSupplier> PackagesProductsSuppliers { get; set; } = new List<PackagesProductsSupplier>();

    //Computed property to check if package is active
    [NotMapped]
    public bool IsActive
    {
        get
        {
            return PkgEndDate.HasValue && PkgEndDate.Value >= DateTime.Now;
        }
    }

    //Helper property to show days until expiry
    [NotMapped]
    public int? DaysUntilExpiry
    {
        get
        {
            if (!PkgEndDate.HasValue) return null;
            return (int)(PkgEndDate.Value - DateTime.Now).TotalDays;
        }
    }
}