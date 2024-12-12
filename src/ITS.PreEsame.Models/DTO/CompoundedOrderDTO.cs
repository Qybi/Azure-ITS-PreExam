namespace ITS.PreEsame.Models.DTO;

public class CompoundedOrderDTO
{
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public string Code { get; set; }
    public IEnumerable<ProductOrderDTO> Products { get; set; } = [];
}
