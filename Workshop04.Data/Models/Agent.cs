namespace Workshop04.Data.Models;

public partial class Agent
{
    [Key]
    public int AgentId { get; set; }

    [StringLength(20)]
    public string AgtFirstName { get; set; }

    [StringLength(5)]
    public string AgtMiddleInitial { get; set; }

    [StringLength(20)]
    public string AgtLastName { get; set; }

    [StringLength(20)]
    public string AgtBusPhone { get; set; }

    [StringLength(50)]
    public string AgtEmail { get; set; }

    [StringLength(20)]
    public string AgtPosition { get; set; }

    public int? AgencyId { get; set; }

    [InverseProperty("Agent")]
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}