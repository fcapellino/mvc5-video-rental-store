using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using VideoRentalStore.DataAccess.Common;
using VideoRentalStore.Domain.Entities;

namespace VideoRentalStore.DataAccess.DataAccessObjects
{
    public class MoviesDataAccessObject : IDataAccess<Movie>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public MoviesDataAccessObject(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _applicationDbContext.Movies
                .Include(m => m.Genre)
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _applicationDbContext.Movies
                .Include(m => m.Genre)
                .Where(m => m.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task AddAsync(Movie entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Added;
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Movie entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Modified;
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Movie entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Deleted;
            await _applicationDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _applicationDbContext?.Dispose();
        }
    }
}
