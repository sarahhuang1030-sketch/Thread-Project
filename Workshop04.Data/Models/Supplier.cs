namespace Workshop04.Data.Models;

[Index("SupplierId", Name = "SupplierId")]
public partial class Supplier
{
    [Key]
    public int SupplierId { get; set; }

    [StringLength(255)]
    public string SupName { get; set; }
}