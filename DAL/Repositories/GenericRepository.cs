using DAL.Context;
using DAL.Contracts;
using DAL.Exceptions;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;
    private readonly ILogger<GenericRepository<T>> _logger;

    public GenericRepository(AppDbContext context, ILogger<GenericRepository<T>> logger)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _logger = logger;
    }

    public async Task<List<T>> GetAllAsync()
    {
        try
        {
            return await _dbSet
                .AsNoTracking()
                .Where(e => e.CurrentState != enEntityState.Deleted)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting all entities of type {EntityType}", typeof(T).Name);
            throw new DataAccessException($"Error while getting all {typeof(T).Name}", ex);
        }
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        try
        {
            return await _dbSet
                .FirstOrDefaultAsync(e =>
                    e.Id == id &&
                    e.CurrentState != enEntityState.Deleted);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error while getting entity by Id {EntityId} of type {EntityType}",
                id,
                typeof(T).Name);

            throw new DataAccessException(
                $"Error while getting {typeof(T).Name} by Id '{id}'.",
                ex);
        }
    }

    public async Task<bool> AddAsync(T entity)
    {
        try
        {
            entity.CurrentState = enEntityState.Active;
            entity.CreatedDate = DateTime.Now;
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding entity of type {EntityType}", typeof(T).Name);
            throw new DataAccessException($"Error while adding {typeof(T).Name}", ex);
        }
    }

    public async Task<bool> UpdateAsync(Guid id, T entity)
    {
        try
        {
            if (entity is null) return false;

            // Very important: guarantee that the ID matches the EF Core to prevent crashing
            entity.Id = id;

            // 1. هات العنصر الحالي من قاعدة البيانات - سيكون تحت التتبع تلقائياً
            var existingEntity = await _dbSet
                .Where(e => e.Id == id && e.CurrentState != enEntityState.Deleted)
                .FirstOrDefaultAsync();

            if (existingEntity is null) return false;

            // 2. السحر هنا: انقل كل القيم الجديدة للقديمة تلقائياً دون تدمير التتبع
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);


            // 3. أخبر EF Core صراحة ألا يلمس هذه الحقول (لن يتم تحديثها في الـ SQL
            _context.Entry(existingEntity).Property(x => x.CreatedDate).IsModified = false;
            _context.Entry(existingEntity).Property(x => x.CreatedBy).IsModified = false;

            // 4. حدد وقت التعديل الحالي
            existingEntity.UpdatedDate = DateTime.Now;

            // 5. احفظ التغييرات (EF سيقوم بتحديث الحقول التي تغيرت قيمتها فعلياً فقط!)
            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating entity of type {EntityType}", typeof(T).Name);
            throw new DataAccessException($"Error while updating {typeof(T).Name}", ex);
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var existingEntity = await _dbSet
                .Where(e => e.Id == id && e.CurrentState != enEntityState.Deleted)
                .FirstOrDefaultAsync();

            if (existingEntity is null) return false;

            // Soft Delete
            existingEntity.CurrentState = enEntityState.Deleted;
            existingEntity.UpdatedDate = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting entity {EntityType}", typeof(T).Name);
            throw new DataAccessException($"Error while deleting {typeof(T).Name}", ex);
        }
    }

    public async Task<bool> ChangeStatusAsync(Guid id, enEntityState status = enEntityState.Active)
    {
        try
        {
            var existingEntity = await _dbSet
                .Where(e => e.Id == id && e.CurrentState != enEntityState.Deleted)
                .FirstOrDefaultAsync();

            if (existingEntity is null) return false;

            existingEntity.CurrentState = status;
            existingEntity.UpdatedDate = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while changing status of entity {EntityType}", typeof(T).Name);
            throw new DataAccessException($"Error while changing status of {typeof(T).Name}", ex);
        }
    }
} 