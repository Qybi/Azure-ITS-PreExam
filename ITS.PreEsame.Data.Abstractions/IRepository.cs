namespace ITS.PreEsame.Data.Abstractions;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(int id);
    Task<int> Add(T entity);
    Task<int> Update(T entity);
    Task<int> Delete(T entity);
}
