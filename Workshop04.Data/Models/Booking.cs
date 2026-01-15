namespace Workshop04.Data.Models;

[Index("CustomerId", Name = "BookingsCustomerId")]
[Index("CustomerId", Name = "CustomersBookings")]
[Index("PackageId", Name = "PackageId")]
[Index("PackageId", Name = "PackagesBookings")]
[Index("TripTypeId", Name = "TripTypesBookings")]
public partial class Booking
{
    [Key]
    public int BookingId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BookingDate { get; set; }

    [StringLength(50)]
    public string BookingNo { get; set; }

    public double? TravelerCount { get; set; }

    public string? CustomerId { get; set; }

    [StringLength(1)]
    public string TripTypeId { get; set; }

    public int? PackageId { get; set; }

    [InverseProperty("Booking")]
    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    [ForeignKey("CustomerId")]
    [InverseProperty("Bookings")]
    public virtual Customer Customer { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("Bookings")]
    public virtual Package Package { get; set; }
}