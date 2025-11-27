using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly GymDbContext _dbContext;

        public UnitOfWork(GymDbContext dbContext , ISessionRepository sessionRepository) {
            this._dbContext = dbContext;
            SessionRepository = sessionRepository;
        }

        public ISessionRepository SessionRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            // Get the entity type (e.g., Member, Trainer, Plan, etc.)
            var EntityType = typeof(TEntity);

            //  Check if a repository for this entity type already exists in the dictionary
            if (_repositories.TryGetValue(EntityType, out var Repo))
                return (IGenericRepository<TEntity>)Repo;

            // If it doesn't exist, create a new GenericRepository instance for this entity type
            var NewRepo = new GenericRepository<TEntity>(_dbContext);

            //  Store the newly created repository in the dictionary for future reuse
            _repositories.Add(EntityType, NewRepo);

            return NewRepo;
        }

        public int SaveChanges()
        {
           return _dbContext.SaveChanges();
        }
    }
}
