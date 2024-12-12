using ITS.PreEsame.Data.Abstractions;
using ITS.PreEsame.Data.Abstractions.Repositories;
using ITS.PreEsame.Data.Context;
using ITS.PreEsame.Models;

namespace ITS.PreEsame.Data.Repositories;

public class CustomersRepository : Repository<Customer>, ICustomersRepository
{
    public CustomersRepository(PreExamDbContext context) : base(context) { }
}
