using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using ITS.PreEsame.Data.Abstractions.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ITS.PreEsame.DeliveryCodeSetter
{
    public class DeliveryCodeSetter
    {
        private readonly ILogger<DeliveryCodeSetter> _logger;
        private readonly IOrdersRepository _orders;

        public DeliveryCodeSetter(ILogger<DeliveryCodeSetter> logger, IOrdersRepository orders)
        {
            _logger = logger;
            _orders = orders;
        }

        [Function(nameof(DeliveryCodeSetter))]
        public async Task Run(
            [ServiceBusTrigger("its-pre-esame-set-delivery-code", Connection = "ConnectionStrings:ServiceBus")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);

            // order code | delivery code
            string[] messageData = message.Body.ToString().Split("|");

            var nRows = await _orders.UpdateDeliveryCode(messageData[0], messageData[1]);
            
            if (nRows == 0)
            {
                _logger.LogWarning("Order code {code} not found", messageData[0]);
                return;
            }

            await messageActions.CompleteMessageAsync(message);
            _logger.LogInformation("Message ID {id} processed", message.MessageId);
        }
    }
}
