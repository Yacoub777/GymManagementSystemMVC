using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        public IEnumerable<Session> GetAllSessionAndTrainerAndCategory();

        public int GetCountOfBookingSlots(int sessionId);

        public Session GetSessionWithTrainerAndCategory(int sessionId);


    }
}
