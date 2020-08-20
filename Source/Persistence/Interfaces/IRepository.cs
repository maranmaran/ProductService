using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets all entities
        /// </summary>
        /// <param name="filter">Filter predicate</param>
        /// <param name="cancellationToken"></param>
        Task<IEnumerable<TEntity>> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            CancellationToken cancellationToken = default);
    }
}
