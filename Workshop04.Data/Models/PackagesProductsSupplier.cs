using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Workshop04.Data.Models;

[Table("Packages_Products_Suppliers")]
[Index("PackageId", Name = "PackagesPackages_Products_Suppliers")]
[Index("ProductSupplierId", Name = "ProductSupplierId")]
[Index("ProductSupplierId", Name = "Products_SuppliersPackages_Products_Suppliers")]
[Index("PackageId", "ProductSupplierId", Name = "UQ__Packages__29CA8E95AD197F06", IsUnique = true)]
public partial class PackagesProductsSupplier
{
    [Key]
    public int PackageProductSupplierId { get; set; }

    public int PackageId { get; set; }

    public int ProductSupplierId { get; set; }

    [ForeignKey("PackageId")]
    [InverseProperty("PackagesProductsSuppliers")]
    public virtual Package Package { get; set; }

    
    [ForeignKey("ProductSupplierId")]
    public virtual ProductsSupplier? ProductsSupplier { get; set; }
}