using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManagementDAL.Data.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;

        public GenericRepository(GymDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public  void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            
        }

        public void Delete(TEntity Entity)
        {

            _dbContext.Remove(Entity);

        }

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? Condition = null)
        {
            if (Condition is null)
            {
                return _dbContext.Set<TEntity>().AsNoTracking().ToList();
            }
            else return _dbContext.Set<TEntity>().AsNoTracking().Where(Condition).ToList();
        }

        public TEntity? GetById(int id)
        {
         return _dbContext.Set<TEntity>().Find(id);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }
    }
}
