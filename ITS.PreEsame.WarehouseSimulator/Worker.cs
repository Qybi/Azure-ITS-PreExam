using Azure.Messaging.ServiceBus;

namespace ITS.PreEsame.WarehouseSimulator;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly string _serviceBusConnectionString;

    public Worker(string serviceBusConnectionString, ILogger<Worker> logger)
    {
        _logger = logger;
        _serviceBusConnectionString = serviceBusConnectionString;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var client = new ServiceBusClient(_serviceBusConnectionString);
        var orderReceiver = client.CreateReceiver("its-pre-esame");
        var deliveryCodeSender = client.CreateSender("its-pre-esame-set-delivery-code");

        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            var message = await orderReceiver.ReceiveMessageAsync();
            if (message == null)
            {
                await Task.Delay(2500, stoppingToken);
                continue;
            }
            
            var messageBody = message.Body.ToString().Split("|");

            Console.WriteLine($"[MSG] {messageBody[1]}");

            Console.WriteLine("Type delivery code for the order above");
            var deliveryCode = Console.ReadLine();

            do
            {
                deliveryCode =  deliveryCode.Trim();
                if (string.IsNullOrWhiteSpace(deliveryCode))
                {
                    Console.WriteLine("Delivery code cannot be empty");
                    deliveryCode = Console.ReadLine();
                }
            } while (string.IsNullOrWhiteSpace(deliveryCode));

            // order code | delivery code
            string deliveryCodeMessage = $"{messageBody[0]}|{deliveryCode}";

            await orderReceiver.CompleteMessageAsync(message);
            await deliveryCodeSender.SendMessageAsync(new ServiceBusMessage(deliveryCodeMessage));
        }

        await orderReceiver.DisposeAsync();
        await client.DisposeAsync();
        await deliveryCodeSender.DisposeAsync();
    }
}
