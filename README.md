## Azure Cloud infrastructure exam preparatory exercise

### Request
Realizzare un sistema per la gestione degli acquisti da un e-commerce, che gestisca le fasi dalla raccolta ordini fino alla preparazione del pacco nel magazzino.
Prevedere un'API rest che accetti l'ordine (id cliente, data ordine), con i prodotti da acquistare (codice, nome, quantit�), salvi i dati da database e comunichi con il software di magazzino.
Prevedere un software (Console Application o Worker Service) per il magazzino, dove (in tempo reale) arrivi l'ordine da preparare e permetta l'inserimento del codice di spedizione (string), da riportare poi nell'ordine  (a database).

I dati degli ordini dovranno venire salvati in un database Azure SQL Database, in una prima fase con il codice di spedizione a NULL, che verr� poi aggiornato dalla console utilizzata dal magazziniere.

### Project structure

- **ITS.PreEsame.API**: HttpTrigger Azure functions that acts as API to receive the order DTO and save the order to DB.	
API Endpoint:
	```
	POST /api/new-order
	```

	Request body:
	```json
	{
		"customerId": 1,
		"date": "1990-01-01",
		"products": [
			{
				"productId": 1,
				"quantity": 2
			},
			{
				"productId": 2,
				"quantity": 1
			}
		]
	}
	```

	For simplicity database has preloaded the following data:
	- customers: id: 1, 2
	- products: id 1, 2, 3

- **ITS.PreEsame.Data**: Data layer with EF Core with Azure SQL Database provider. It is a basic Repository pattern. No Unit of Work pattern is implemented due to the simplicity and time required for the project.
- **ITS.PreEsame.WareHouseSimulator**: Worker service to simulate the warehouse operations. It receives in real time the orders and sends back the shipping code to the service bus.
- **ITS.PreEsame.DeliveryCodeSetter**: ServiceBusQueueTrigger Azure function that receives the shipping code from the warehouse simulator and updates the relative order rows in the database.

<hr>

#### EF Core commands
1. Adding a migration
```shell
dotnet ef migrations add <Migration Name> --output-dir ..\ITS.PreEsame.Data\Migrations --project ..\ITS.PreEsame.Data --context PreExamDbContext
```
2. Updating the database
```shell
dotnet ef database update --connection "connstring"
```