using System.Linq.Expressions;
using UtilitesLayer.Utilities;

namespace UtilitesLayer.Services;

public interface IGenericRepository<TEntity> where TEntity : class
{
    public Task<OperationResult> Create(TEntity entity);
    public Task<OperationResult> Update(TEntity entity);
    public Task<OperationResult> Delete(TEntity entity);
    public Task<OperationResult> Delete(int  id);
    public Task<TEntity> Get(int id);
    public Task<ICollection<TEntity>> GetAll();
    public Task<TEntity> Find(Expression<Func<TEntity, bool>> expression);
    public Task<ICollection<TEntity>> FindAll(Expression<Func<TEntity, bool>> expression);
}