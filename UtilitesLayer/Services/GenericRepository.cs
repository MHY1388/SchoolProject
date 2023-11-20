using System.Linq.Expressions;
using System.Runtime.InteropServices;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using UtilitesLayer.DTOs.Global;
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
            var entity = Get(id).Result;
            if (entity == null)
            {
                return OperationResult.NotFound("این مورد وجود ندارد");
            }
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

    public async Task<Paggination<TEntity>> GetPaggination(int size, int page = 1)
    {
        var skip = (page - 1) * size;

        var data = _dbContext.Set<TEntity>().AsQueryable();
        var count =  data.Count();
        var pages =(int) Math.Round((decimal)count / size, MidpointRounding.ToPositiveInfinity);
        List<TEntity> result =  await data.Skip(skip).Take(size).ToListAsync() ;

        return new Paggination<TEntity>() { CurrentPage = page, GetSize = size, Objects = result, PageCount = pages };
    }

    public async Task<Paggination<TEntity>> GetPaggination(int size, Expression<Func<TEntity, bool>> expression, int page = 1)
    {
        var skip = (page - 1) * size;

        var data = _dbContext.Set<TEntity>().Where(expression).AsQueryable();
        var count = data.Count();
        var pages = (int)Math.Round((decimal)count / size, MidpointRounding.ToPositiveInfinity);
        List<TEntity> result = await data.Skip(skip).Take(size).ToListAsync();

        return new Paggination<TEntity>() { CurrentPage = page, GetSize = size, Objects = result, PageCount = pages };
    }

    public Task<bool> Any(Expression<Func<TEntity, bool>> expression)
    {
       return _dbContext.Set<TEntity>().AnyAsync(expression);
    }
}