using System.Linq.Expressions;

namespace OnlineCoursesPlatform.Domain.Interfaces;

/// <summary>
/// Interfaz genérica de repositorio para operaciones CRUD.
/// </summary>
/// <typeparam name="T">Tipo de entidad</typeparam>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    
    Task<IEnumerable<T>> GetAllAsync();
    
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    
    Task<T> AddAsync(T entity);
    
    Task AddRangeAsync(IEnumerable<T> entities);
    
    void Update(T entity);
    
    void Remove(T entity);
    
    void RemoveRange(IEnumerable<T> entities);
    
    Task<int> SaveChangesAsync();
}
