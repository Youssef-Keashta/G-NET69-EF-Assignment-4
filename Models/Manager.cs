namespace G_NET69_EF_Assignment_4.Models;

public class Manager
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }

    public Branch Branch { get; set; } = null!;
}
