namespace ITS.PreEsame.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;

    public virtual ICollection<Order> Orders { get; set; }
}
