using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using VideoRentalStore.DataAccess.Common;
using VideoRentalStore.Domain.Entities;

namespace VideoRentalStore.DataAccess.DataAccessObjects
{
    public class MovieGenresDataAccessObject : IDataAccess<MovieGenre>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public MovieGenresDataAccessObject(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<MovieGenre>> GetAllAsync()
        {
            return await _applicationDbContext.MovieGenres
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        public async Task<MovieGenre> GetByIdAsync(int id)
        {
            return await _applicationDbContext.MovieGenres
                .Where(g => g.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task AddAsync(MovieGenre entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Added;
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(MovieGenre entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Modified;
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(MovieGenre entity)
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
