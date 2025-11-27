using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext) {
            this._dbContext = dbContext;
        }
        public IEnumerable<Session> GetAllSessionAndTrainerAndCategory()
        {
         return _dbContext.Sessions
                .Include(S=>S.SessionTrainer)  
                .Include(S=>S.SessionCategory)
                .ToList();
        }

        public int GetCountOfBookingSlots(int sessionId)
        {
           return _dbContext.MemberSessions.Count(MS=>MS.SessionId == sessionId);
        }

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
           return _dbContext.Sessions
                .Include(S=>S.SessionTrainer)
                .Include(S=>S.SessionCategory)
                .FirstOrDefault(s=>s.Id == sessionId);
        }
    }
}
