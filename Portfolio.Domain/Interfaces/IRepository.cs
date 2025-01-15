namespace Portfolio.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    T? GetById(int id);
    T Create(T entity);
    T Update(T entity);
    void Delete(T entity);
    void RemoveRows(List<int> ids);
}