namespace ITS.PreEsame.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Code { get; set; }
    public string? DeliveryCode { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public virtual Product Product { get; set; }
    public virtual Customer Customer { get; set; }
}
