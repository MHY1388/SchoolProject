using System.Linq.Expressions;
using System.Runtime.InteropServices;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using UtilitesLayer.DTOs.Global;
using UtilitesLayer.Utilities;
using WebLayer.Data;
using static Amazon.S3.Util.S3EventNotification;

namespace UtilitesLayer.Services;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
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
            entity.Created = DateTime.Now;
            entity.Updated = DateTime.Now;
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
            Attach(entity);
            entity.Updated= DateTime.Now;
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



    public async Task<ICollection<TEntity>> GetAllWithInclude(List<Expression<Func<TEntity, dynamic>>> includes)
    {
        var dbSet = _dbContext.Set<TEntity>();
        IIncludableQueryable<TEntity, object> x = null;
        foreach (var include in includes)
        {
            if (x is null)
            {
                x = dbSet.Include(include);

            }
            else
            {
                x = x.Include(include);
            }
        }
        return await x.ToListAsync();

    }

    public async Task<TEntity> FindWithInclude(Expression<Func<TEntity, bool>> expression, List<Expression<Func<TEntity, dynamic>>> includes)
    {
        var dbSet = _dbContext.Set<TEntity>().AsNoTracking();
        IIncludableQueryable<TEntity, object> x = null;
        foreach (var include in includes)
        {
            if (x is null)
            {
                x = dbSet.Include(include);

            }
            else
            {
                x = x.Include(include);
            }
        }
        return await x.FirstOrDefaultAsync(expression);

    }

    public async Task<ICollection<TEntity>> FindAllWithInclude(Expression<Func<TEntity, bool>> expression, List<Expression<Func<TEntity, dynamic>>> includes)
    {
        var dbSet = _dbContext.Set<TEntity>();
        IIncludableQueryable<TEntity, object> x = null;
        foreach (var include in includes)
        {
            if (x is null)
            {
                x = dbSet.Include(include);

            }
            else
            {
                x = x.Include(include);
            }
        }
        return await x.Where(expression).ToListAsync();

    }

    public async Task<Paggination<TEntity>> GetPagginationWithInclude(int size, List<Expression<Func<TEntity, dynamic>>> includes, int page = 1)
    {
        var skip = (page - 1) * size;
        var dbSet = _dbContext.Set<TEntity>();
        IIncludableQueryable<TEntity, object> x = null;
        foreach (var include in includes)
        {
            if (x is null)
            {
                x = dbSet.Include(include);

            }
            else
            {
                x= x.Include(include);
            }
        }

        var data = x.AsQueryable();
        var count = data.Count();
        var pages = (int)Math.Round((decimal)count / size, MidpointRounding.ToPositiveInfinity);
        List<TEntity> result =  await data.Skip(skip).Take(size).ToListAsync();

        return new Paggination<TEntity>() { CurrentPage = page, GetSize = size, Objects = result, PageCount = pages };

    }

    public async Task<Paggination<TEntity>> GetPagginationWithInclude(int size, Expression<Func<TEntity, bool>> expression, List<Expression<Func<TEntity, dynamic>>> includes, int page = 1)
    {

        var skip = (page - 1) * size;
        var dbSet = _dbContext.Set<TEntity>();
        IIncludableQueryable<TEntity, object> x = null;
        foreach (var include in includes)
        {
            if (x is null)
            {
                x = dbSet.Include(include);

            }
            else
            {
                x = x.Include(include);
            }
        }
        var data = x.Where(expression).AsQueryable();
        var count = data.Count();
        var pages = (int)Math.Round((decimal)count / size, MidpointRounding.ToPositiveInfinity);
        List<TEntity> result = await data.Skip(skip).Take(size).ToListAsync();

        return new Paggination<TEntity>() { CurrentPage = page, GetSize = size, Objects = result, PageCount = pages };

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
    public async Task<Paggination<TEntity>> GetPaggination(int size, List<TEntity> Data, int page = 1)
    {
        var skip = (page - 1) * size;

        var data = Data.AsQueryable();
        var count = data.Count();
        var pages = (int)Math.Round((decimal)count / size, MidpointRounding.ToPositiveInfinity);
        List<TEntity> result = data.Skip(skip).Take(size).ToList();

        return new Paggination<TEntity>() { CurrentPage = page, GetSize = size, Objects = result, PageCount = pages };
    }

    public async Task<Paggination<TEntity>> GetPaggination(int size, Func<TEntity, bool> expression, List<TEntity> Data, int page = 1)
    {
        var skip = (page - 1) * size;

        var data = Data.Where(expression).AsQueryable();
        var count = data.Count();
        var pages = (int)Math.Round((decimal)count / size, MidpointRounding.ToPositiveInfinity);
        List<TEntity> result = data.Skip(skip).Take(size).ToList();

        return new Paggination<TEntity>() { CurrentPage = page, GetSize = size, Objects = result, PageCount = pages };
    }
    public Task<bool> Any(Expression<Func<TEntity, bool>> expression)
    {
       return _dbContext.Set<TEntity>().AnyAsync(expression);
    }
    public void Attach(TEntity model)
    {
        if (_dbContext.Entry(model).State == EntityState.Detached)
        {
            _dbContext.Attach(model);
        }
    }

    public async Task<TEntity> GetNoTracking(int id)
    {
        return await _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(a=>a.Id==id);
    }
}
public static class GenericRepositoryStatic
{
    public static async Task<Paggination<T>> GetPaggination<T>(int size, IEnumerable<T> Data, int page = 1) where T : class
    {
        var skip = (page - 1) * size;

        var data = Data.AsQueryable();
        var count = data.Count();
        var pages = (int)Math.Round((decimal)count / size, MidpointRounding.ToPositiveInfinity);
        List<T> result = data.Skip(skip).Take(size).ToList();

        return new Paggination<T>() { CurrentPage = page, GetSize = size, Objects = result, PageCount = pages };
    }

    public static async Task<Paggination<T>> GetPaggination<T>(int size, Expression<Func<T, bool>> expression, IEnumerable<T> Data, int page = 1) where T : class
    {
        var skip = (page - 1) * size;

        var data = Data.AsQueryable();
        data = data.Where(expression);
        var count = data.Count();
        var pages = (int)Math.Round((decimal)count / size, MidpointRounding.ToPositiveInfinity);
        List<T> result = data.Skip(skip).Take(size).ToList();

        return new Paggination<T>() { CurrentPage = page, GetSize = size, Objects = result, PageCount = pages };
    }
}