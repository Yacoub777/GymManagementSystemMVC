using GymManagementDAL.Entities;

namespace GymManagementDAL.Data.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        TEntity? GetById(int id);
        IEnumerable<TEntity> GetAll(Func<TEntity , bool>? Condition = null);

        void Add (TEntity entity);
        void Update (TEntity entity);
        void Delete (TEntity entity);


    }
}
