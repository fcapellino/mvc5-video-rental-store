using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VideoRentalStore.DataAccess.Common
{
    public interface IDataAccess<Type> : IDisposable
    {
        Task<IEnumerable<Type>> GetAllAsync();
        Task<Type> GetByIdAsync(int id);
        Task AddAsync(Type entity);
        Task UpdateAsync(Type entity);
        Task DeleteAsync(Type entity);
    }
}
