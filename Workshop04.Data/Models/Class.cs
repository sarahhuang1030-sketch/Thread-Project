namespace Workshop04.Data.Models;

public partial class Class
{
    [Key]
    [StringLength(5)]
    public string ClassId { get; set; }

    [Required]
    [StringLength(20)]
    public string ClassName { get; set; }

    [StringLength(50)]
    public string ClassDesc { get; set; }

    [InverseProperty("Class")]
    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
}