using ITS.PreEsame.Models;
using ITS.PreEsame.Models.DTO;

namespace ITS.PreEsame.Data.Abstractions.Repositories;

public interface IOrdersRepository : IRepository<Order>
{
    Task<string> AddCompoundedOrder(CompoundedOrderDTO productOrder);
    Task<int> UpdateDeliveryCode(string orderCode, string deliveryCode);
}
