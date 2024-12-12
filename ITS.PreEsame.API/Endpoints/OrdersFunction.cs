using Azure.Messaging.ServiceBus;
using ITS.PreEsame.Data.Abstractions.Repositories;
using ITS.PreEsame.Models;
using ITS.PreEsame.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ITS.PreEsame.API;

public class OrdersFunction
{
    private readonly ILogger<OrdersFunction> _logger;
    private readonly IOrdersRepository _orders;
    private readonly IProductsRepository _products;
    private readonly ICustomersRepository _customers;
    private readonly string _serviceBusConnectionString;

    public OrdersFunction(ILogger<OrdersFunction> logger, IConfiguration configuration,
        IOrdersRepository ordersRepository,
        ICustomersRepository customers,
        IProductsRepository products
        )
    {
        _logger = logger;
        _orders = ordersRepository;
        _serviceBusConnectionString = configuration.GetConnectionString("ServiceBus") ?? string.Empty;
        _customers = customers;
        _products = products;
    }

    [Function("new-order")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req)
    {
        var po = new CompoundedOrderDTO();
        try
        {
            po = await JsonSerializer.DeserializeAsync<CompoundedOrderDTO>(req.Body, new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deserializing the request body");
            return new BadRequestResult();
        }

        var orderCode = await _orders.AddCompoundedOrder(po);

        var client = new ServiceBusClient(_serviceBusConnectionString);
        var sender = client.CreateSender("its-pre-esame");

        var message = await WarehouseMessageCompose(po, orderCode);
        await sender.SendMessageAsync(message);

        return new OkObjectResult($"Your order has been sent to the warehouse!");
    }

    private async Task<ServiceBusMessage> WarehouseMessageCompose(CompoundedOrderDTO order, string orderCode)
    {
        var products = await _products.GetAll();
        var customers = await _customers.GetAll();

        var customer = customers.FirstOrDefault(c => c.Id == order.CustomerId);
        string messageToWarehouse = $"{orderCode}|Order {order.Code} for customer {customer.Surname} {customer.Name}:\n";
        foreach (var p in order.Products)
        {
            messageToWarehouse += $"Product {products.FirstOrDefault(pr => pr.Id == p.ProductId).Name} - Quantity: {p.Quantity}\n";
        }

        return new ServiceBusMessage(messageToWarehouse);
    }
}
