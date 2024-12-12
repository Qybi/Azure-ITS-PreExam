using ITS.PreEsame.Data.Abstractions;
using ITS.PreEsame.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ITS.PreEsame.Data;

public class Repository<T> : IRepository<T> where T : class
{
    // for simplicity of the project at this stage, uow will not be implemented and every command will trigger a save changes

    private readonly PreExamDbContext _context;
    protected PreExamDbContext Context { get { return _context; } }
    public Repository(PreExamDbContext context)
    {
        _context = context;
    }

    public Task<int> Add(T entity)
    {
        _context.Set<T>().Add(entity);
        return _context.SaveChangesAsync();
    }

    public Task<int> Delete(T entity)
    {
        _context.Remove(entity);
        return _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetById(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<int> Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }
}
