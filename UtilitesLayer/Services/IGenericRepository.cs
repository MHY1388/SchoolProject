using System.Linq.Expressions;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.Utilities;

namespace UtilitesLayer.Services;

public interface IGenericRepository<TEntity> where TEntity : class
{
    public Task<OperationResult> Create(TEntity entity);
    public Task<OperationResult> Update(TEntity entity);
    public Task<OperationResult> Delete(TEntity entity);
    public Task<OperationResult> Delete(int  id);
    public Task<ICollection<TEntity>> GetAll();
    public Task<TEntity> Find(Expression<Func<TEntity, bool>> expression);
    public Task<ICollection<TEntity>> FindAll(Expression<Func<TEntity, bool>> expression);
    public Task<Paggination<TEntity>> GetPaggination(int size, int page =1);
    public Task<Paggination<TEntity>> GetPaggination(int size, Expression<Func<TEntity,bool>> expression, int page = 1);
    public Task<TEntity> Get(int id);
    public Task<TEntity> GetNoTracking(int id);
    public Task<ICollection<TEntity>> GetAllWithInclude(List<Expression<Func<TEntity, dynamic>>> includes);
    public Task<TEntity> FindWithInclude(Expression<Func<TEntity, bool>> expression, List<Expression<Func<TEntity, dynamic>>> includes);
    public Task<ICollection<TEntity>> FindAllWithInclude(Expression<Func<TEntity, bool>> expression, List<Expression<Func<TEntity, dynamic>>> includes);
    public Task<Paggination<TEntity>> GetPagginationWithInclude(int size, List<Expression<Func<TEntity, dynamic>>> includes, int page =1);
    public Task<Paggination<TEntity>> GetPagginationWithInclude(int size, Expression<Func<TEntity,bool>> expression, List<Expression<Func<TEntity, dynamic>>> includes, int page = 1);
    public Task<Paggination<TEntity>> GetPaggination(int size, List<TEntity> Data, int page = 1);
    public Task<Paggination<TEntity>> GetPaggination(int size, Func<TEntity, bool> expression, List<TEntity> Data, int page = 1);


    public Task<bool> Any(Expression<Func<TEntity, bool>> expression);
}