namespace Workshop04.Data.Models;

[Index("ProductId", Name = "ProductId")]
public partial class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [StringLength(50)]
    public string ProdName { get; set; }
}