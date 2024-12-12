using ITS.PreEsame.Data.Abstractions.Repositories;
using ITS.PreEsame.Data.Context;
using ITS.PreEsame.Models;
using ITS.PreEsame.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ITS.PreEsame.Data.Repositories;

public class OrdersRepository : Repository<Order>, IOrdersRepository
{
    public OrdersRepository(PreExamDbContext context) : base(context) { }

    public async Task<string> AddCompoundedOrder(CompoundedOrderDTO productOrder)
    {
        var orderList = new List<Order>();
        var orderCode = Guid.NewGuid().ToString();
        foreach (var p in productOrder.Products)
        {
            orderList.Add(new Order()
            {
                CustomerId = productOrder.CustomerId,
                Date = productOrder.Date,
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                Code = orderCode
            });
        }

        Context.Orders.AddRange(orderList);
        await Context.SaveChangesAsync();

        return orderCode;
    }

    public async Task<int> UpdateDeliveryCode(string orderCode, string deliveryCode)
    {
        var orders = await Context.Orders.Where(o => o.Code == orderCode).ToListAsync();

        foreach (var order in orders)
        {
            order.DeliveryCode = deliveryCode;
            Context.Entry(order).State = EntityState.Modified;
        }

        return await Context.SaveChangesAsync();
    }
}
