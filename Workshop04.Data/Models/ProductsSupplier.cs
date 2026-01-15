using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workshop04.Data.Models;

[Table("Products_Suppliers")]
public partial class ProductsSupplier
{
    [Key]
    public int ProductSupplierId { get; set; }

    public int? ProductId { get; set; }

    public int? SupplierId { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    [ForeignKey("SupplierId")]
    public virtual Supplier? Supplier { get; set; }
}