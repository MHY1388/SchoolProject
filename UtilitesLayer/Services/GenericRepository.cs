using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UtilitesLayer.Utilities;
using WebLayer.Data;

namespace UtilitesLayer.Services;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _dbContext;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<OperationResult> Create(TEntity entity)
    {
        try
        {
            await _dbContext.AddAsync(entity);
            return OperationResult.Success();
        }
        catch (Exception ex)
        {
            return OperationResult.Error(ex.Message);
        }
    }

    public async Task<OperationResult> Update(TEntity entity)
    {
        try
        {
            _dbContext.Update(entity);
            return OperationResult.Success();
        }
        catch (Exception ex)
        {
            return OperationResult.Error(ex.Message);
        }
    }

    public async Task<OperationResult> Delete(TEntity entity)
    {
        try
        {
            _dbContext.Remove(entity);
            return OperationResult.Success();
        }
        catch (Exception ex)
        {
            return OperationResult.Error(ex.Message);
        }
    }

    public async Task<OperationResult> Delete(int id)
    {
        try
        {
            var entity = Get(id);
            _dbContext.Remove(entity);
            return OperationResult.Success();
        }
        catch (Exception ex)
        {
            return OperationResult.Error(ex.Message);
        }
    }

    public async Task<TEntity> Get(int id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task<ICollection<TEntity>> GetAll()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> Find(Expression<Func<TEntity, bool>> expression)
    {
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(expression);
    }

    public async Task<ICollection<TEntity>> FindAll(Expression<Func<TEntity, bool>> expression)
    {
        return await _dbContext.Set<TEntity>().Where(expression).ToListAsync();

    }
}