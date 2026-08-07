using DAL.Context;
using DAL.Contracts;
using DAL.Exceptions;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace DAL.Repositories.Generic;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly ShippingDbContext _context;
    protected readonly DbSet<T> _dbSet;
    protected readonly ILogger<GenericRepository<T>> _logger;


    public GenericRepository(ShippingDbContext context, ILogger<GenericRepository<T>> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<T>();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    #region Get Methods

    /// <summary>
    /// Retrieves all entities of type <typeparamref name="T"/> asynchronously.
    /// Soft-deleted entities are excluded by the global query filter.
    /// </summary>
    /// <param name="tracking">Whether the returned entities should be tracked by the change tracker. Default is false.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>A collection of entities, or an empty collection if the operation fails.</returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync(bool tracking = false, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = tracking ? _dbSet.AsTracking() : _dbSet.AsNoTracking();
            return await query.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            HandleException(nameof(GetAllAsync), $"Error while retrieving all entities of type {typeof(T).Name}.", ex);
        }

        // Impossible to reach here in practice, but the compiler needs it for the return type
        return Enumerable.Empty<T>();
    }


    /// <summary>
    /// Retrieves a single entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <param name="tracking">Whether the returned entity should be tracked by the change tracker. Default is false.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>The matching entity, or null if not found or the operation fails.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="id"/> is <see cref="Guid.Empty"/>.</exception>
    public virtual async Task<T?> GetByIdAsync(Guid id, bool tracking = false, CancellationToken cancellationToken = default)
    {
        try
        {
            if (id == Guid.Empty)
                throw new ArgumentException("ID cannot be empty.", nameof(id));

            return tracking
                ? await _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
                : await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
        catch (ArgumentException)
        {
            // Exception for invalid argument, not a data access error
            throw;
        }
        catch (Exception ex)
        {
            HandleException(nameof(GetByIdAsync), $"Error while retrieving {typeof(T).Name} by Id '{id}'.", ex);
        }

        return null;
    }


    /// <summary>
    /// Retrieves the first entity that matches the specified predicate, or null if none matches.
    /// </summary>
    /// <param name="predicate">The filter expression used to match entities.</param>
    /// <param name="tracking">Whether the returned entity should be tracked by the change tracker. Default is false.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>The first matching entity, or null if none is found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> is null.</exception>
    public virtual async Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>> predicate,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(predicate);

            var query = tracking ? _dbSet.AsTracking() : _dbSet.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // If the operation was canceled, we rethrow the exception to allow the caller to handle it appropriately.
            throw;
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (Exception ex)
        {
            HandleException(nameof(GetFirstOrDefaultAsync), $"Error while getting first {typeof(T).Name}.", ex);
        }

        return null;
    }

    /// <summary>
    /// Retrieves a list of entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">The filter expression used to match entities.</param>
    /// <param name="tracking">Whether the returned entities should be tracked by the change tracker. Default is false.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>A collection of matching entities, or an empty collection if the operation fails.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> is null.</exception>
    public virtual async Task<IEnumerable<T>> GetListAsync(
        Expression<Func<T, bool>> predicate,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(predicate);

            var query = tracking ? _dbSet.AsTracking() : _dbSet.AsNoTracking();
            return await query.Where(predicate).ToListAsync(cancellationToken);
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (Exception ex)
        {
            HandleException(nameof(GetListAsync), $"Error while retrieving {typeof(T).Name} list.", ex);
        }

        return Enumerable.Empty<T>();
    }


    /// <summary>
    /// Gets an <see cref="IQueryable{T}"/> for building advanced, composable LINQ queries.
    /// </summary>
    /// <param name="tracking">Whether the returned query should track entities. Default is false.</param>
    /// <returns>An <see cref="IQueryable{T}"/> instance for the entity set.</returns>
    public virtual IQueryable<T> GetQueryable(bool tracking = false)
    {
        return tracking ? _dbSet.AsTracking() : _dbSet.AsNoTracking();
    }

    #endregion

    #region Exists / Count

    /// <summary>
    /// Checks whether an entity with the specified identifier exists.
    /// </summary>
    /// <param name="id">The unique identifier to check.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>True if an entity with the given ID exists; otherwise false.</returns>
    public virtual async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            if (id == Guid.Empty) return false;
            return await _dbSet.AnyAsync(e => e.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            HandleException(nameof(ExistsAsync), $"Error while checking existence by Id for {typeof(T).Name}.", ex);
        }

        return false;
    }


    /// <summary>
    /// Checks whether any entity matches the specified predicate.
    /// </summary>
    /// <param name="predicate">The filter expression used to match entities.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>True if at least one matching entity exists; otherwise false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="predicate"/> is null.</exception>
    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(predicate);
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (Exception ex)
        {
            HandleException(nameof(ExistsAsync), $"Error while checking existence for {typeof(T).Name}.", ex);
        }

        return false;
    }


    /// <summary>
    /// Counts the number of entities, optionally filtered by a predicate.
    /// </summary>
    /// <param name="predicate">An optional filter expression. If null, all entities are counted.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>The number of matching entities, or 0 if the operation fails.</returns>
    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        try
        {
            return predicate == null
                ? await _dbSet.CountAsync(cancellationToken)
                : await _dbSet.CountAsync(predicate, cancellationToken);
        }
        catch (Exception ex)
        {
            HandleException(nameof(CountAsync), $"Error while counting {typeof(T).Name}.", ex);
        }

        return 0;
    }

    #endregion

    #region Create / Update / Delete


    /// <summary>
    /// Creates a new entity and persists it to the database.
    /// Sets <see cref="BaseEntity.CurrentState"/> to Active and <see cref="BaseEntity.CreatedDate"/> to the current UTC time.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>The identifier of the newly created entity, or <see cref="Guid.Empty"/> if the operation fails.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
    public virtual async Task<Guid> CreateAsync(T entity, bool AutoSave = false, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(entity);

            if(entity.Id == Guid.Empty) entity.Id = Guid.NewGuid(); // Ensure a new ID is assigned if not already set
            entity.CurrentState = enEntityState.Active;
            entity.CreatedDate = DateTime.UtcNow;

            _dbSet.Add(entity);

            if (AutoSave)
                await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (Exception ex)
        {
            HandleException(nameof(CreateAsync), $"Error while adding {typeof(T).Name}.", ex);
        }

        return Guid.Empty;
    }


    /// <summary>
    /// Updates an existing entity identified by <paramref name="id"/> with the values from <paramref name="entity"/>.
    /// Preserves the original <see cref="BaseEntity.CreatedDate"/> and <see cref="BaseEntity.CreatedBy"/> values.
    /// </summary>
    /// <param name="id">The identifier of the entity to update.</param>
    /// <param name="entity">The entity containing the updated values.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>True if the update succeeded; false if the entity was not found.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity"/> is null.</exception>
    public virtual async Task<bool> UpdateAsync(Guid id, T entity, bool AutoSave = false, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.Id = id; // match the ID with the one coming from the route/command

            var existingEntity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (existingEntity is null) return false;

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            // Prevent modification of these fields even if they come with values in the entity
            _context.Entry(existingEntity).Property(x => x.CreatedDate).IsModified = false;
            _context.Entry(existingEntity).Property(x => x.CreatedBy).IsModified = false;
            _context.Entry(existingEntity).Property(x => x.CurrentState).IsModified = false;


            existingEntity.UpdatedDate = DateTime.UtcNow;

            if (AutoSave)
                await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (ArgumentNullException)
        {
            throw;
        }
        catch (Exception ex)
        {
            HandleException(nameof(UpdateAsync), $"Error while updating {typeof(T).Name}.", ex);
        }

        return false;
    }


    /// <summary>
    /// Updates specific fields of an existing entity using a custom action delegate,
    /// without replacing the entire entity. Protected fields (CreatedDate, CreatedBy, CurrentState)
    /// are always preserved regardless of what the action modifies.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to update.</param>
    /// <param name="updateAction">
    /// A delegate that receives the tracked entity and applies targeted field modifications.
    /// <example>
    /// <code>
    /// await _repository.UpdateAsync(id, entity =>
    /// {
    ///     entity.CarrierName = "New Name";
    ///     entity.SomeField = newValue;
    /// }, autoSave: true);
    /// </code>
    /// </example>
    /// </param>
    /// <param name="AutoSave">If true, persists changes to the database immediately.</param>
    /// <param name="cancellationToken">Token to cancel the async operation.</param>
    /// <returns>True if the entity was found and updated; false if not found.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the entity resolved by id is null.</exception>
    public virtual async Task<bool> UpdateAsync(
        Guid id,
        Action<T> updateAction,
        bool AutoSave = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (existingEntity is null) return false;

            updateAction(existingEntity);

            // 🔒 Protected fields — always preserved regardless of updateAction
            _context.Entry(existingEntity).Property(x => x.CreatedDate).IsModified = false;
            _context.Entry(existingEntity).Property(x => x.CreatedBy).IsModified = false;
            _context.Entry(existingEntity).Property(x => x.CurrentState).IsModified = false;

            existingEntity.UpdatedDate = DateTime.UtcNow;

            if (AutoSave)
                await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            HandleException(nameof(UpdateAsync), $"Error while updating {typeof(T).Name}.", ex);
        }

        return false;
    }


    /// <summary>
    /// Soft-deletes an entity by setting its state to <see cref="enEntityState.Deleted"/>.
    /// The entity is not physically removed from the database.
    /// </summary>
    /// <param name="id">The identifier of the entity to delete.</param>
    /// <param name="deletedBy">The identifier of the user performing the deletion.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>True if the deletion succeeded; false if the entity was not found.</returns>
    public virtual async Task<bool> DeleteAsync(Guid id, Guid deletedBy, bool AutoSave = false, CancellationToken cancellationToken = default)
    {
        try
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (existingEntity is null)
            {
                _logger.LogWarning("Entity with Id {EntityId} of type {EntityType} not found for deletion.", id, typeof(T).Name);
                return false;
            }

            // Soft Delete
            existingEntity.CurrentState = enEntityState.Deleted;
            existingEntity.UpdatedDate = DateTime.UtcNow;
            existingEntity.UpdatedBy = deletedBy;

            if (AutoSave)
                await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            HandleException(nameof(DeleteAsync), $"Error while deleting {typeof(T).Name}.", ex);
        }

        return false;
    }



    /// <summary>
    /// Changes the state of an entity (e.g. reactivating a soft-deleted entity).
    /// Bypasses the global query filter to allow retrieving entities in any state.
    /// </summary>
    /// <param name="id">The identifier of the entity whose status should be changed.</param>
    /// <param name="updatedBy">The identifier of the user performing the update.</param>
    /// <param name="status">The new state to assign to the entity. Default is Active.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>True if the status change succeeded; false if the entity was not found.</returns>
    public virtual async Task<bool> ChangeStatusAsync(
        Guid id,
        Guid updatedBy,
        enEntityState status = enEntityState.Active,
        bool AutoSave = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // محتاجين IgnoreQueryFilters هنا عشان لو الـ entity Deleted فعلاً
            // ومحتاجين نرجّعها Active تاني، الـ Global Filter بتاعك هيمنعها تتلاقي أصلاً
            var existingEntity = await _dbSet
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (existingEntity is null) return false;

            existingEntity.CurrentState = status;
            existingEntity.UpdatedDate = DateTime.UtcNow;
            existingEntity.UpdatedBy = updatedBy;

            if (AutoSave)
                await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            HandleException(nameof(ChangeStatusAsync), $"Error while changing status of {typeof(T).Name}.", ex);
        }

        return false;
    }

    #endregion

    #region Pagination

    /// <summary>
    /// Retrieves a paged subset of entities, optionally filtered and ordered.
    /// </summary>
    /// <param name="pageNumber">The 1-based page number to retrieve.</param>
    /// <param name="pageSize">The number of entities per page.</param>
    /// <param name="filter">An optional filter expression applied before paging.</param>
    /// <param name="orderBy">An optional ordering function. Defaults to ordering by <see cref="BaseEntity.CreatedDate"/> descending.</param>
    /// <param name="tracking">Whether the returned entities should be tracked by the change tracker. Default is false.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
    /// <returns>A tuple containing the page of entities and the total count of matching entities.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="pageNumber"/> or <paramref name="pageSize"/> is less than or equal to zero.</exception>
    public virtual async Task<(IEnumerable<T> Data, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        bool tracking = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            ValidatePaginationParameters(pageNumber, pageSize);

            IQueryable<T> query = tracking ? _dbSet.AsTracking() : _dbSet.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            query = orderBy != null
                ? orderBy(query)
                : query.OrderByDescending(e => e.CreatedDate);

            var totalCount = await query.CountAsync(cancellationToken);

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (data, totalCount);
        }
        catch (ArgumentException)
        {
            throw; // خطأ Validation، مش خطأ Data Access
        }
        catch (Exception ex)
        {
            HandleException(nameof(GetPagedAsync), $"Error while getting paged {typeof(T).Name}.", ex);
        }

        return (Enumerable.Empty<T>(), 0);
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Logs the given exception and throws a <see cref="DataAccessException"/> wrapping it.
    /// This method never returns normally.
    /// </summary>
    /// <param name="methodName">The name of the method where the error occurred.</param>
    /// <param name="message">A descriptive message about the error.</param>
    /// <param name="ex">The original exception that was caught.</param>
    /// <exception cref="DataAccessException">Always thrown by this method.</exception>
    [DoesNotReturn]
    protected void HandleException(string methodName, string message, Exception ex)
    {
        _logger.LogError(ex, "[{MethodName}] {Message}", methodName, message);
        throw new DataAccessException(message, ex);
    }

    /// <summary>
    /// Validates that pagination parameters are within an acceptable range.
    /// </summary>
    /// <param name="pageNumber">The page number to validate.</param>
    /// <param name="pageSize">The page size to validate.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="pageNumber"/> or <paramref name="pageSize"/> is less than or equal to zero.</exception>
    protected static void ValidatePaginationParameters(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
            throw new ArgumentException("Page number must be greater than zero.", nameof(pageNumber));

        if (pageSize <= 0)
            throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
    }

    #endregion

    #region Save (للتسريع)

    // Saves all changes made in this context to the database.
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    #endregion
}
