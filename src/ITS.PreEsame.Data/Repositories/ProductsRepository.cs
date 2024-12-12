using ITS.PreEsame.Data.Abstractions.Repositories;
using ITS.PreEsame.Data.Context;
using ITS.PreEsame.Models;

namespace ITS.PreEsame.Data.Repositories;

public class ProductsRepository : Repository<Product>, IProductsRepository
{
    public ProductsRepository(PreExamDbContext context) : base(context) { }
}
