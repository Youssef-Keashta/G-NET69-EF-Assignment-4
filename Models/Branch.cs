namespace G_NET69_EF_Assignment_4.Models;

public class Branch
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int ManagerId { get; set; }

    public Manager Manager { get; set; } = null!;
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
